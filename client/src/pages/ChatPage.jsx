import { conservationUsers, recentContributionAPI } from '@/apis'
import { Separator } from '@/components/ui/separator'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { useSignalRContext } from '@/contexts/signalr.context'
import { useAppContext } from '@/hooks/useAppContext'
import GeneralLayout from '@/layouts'
import { addNewMessage, createNewConservation, getCurrentOnlineUser, getDetailConservations, getPrivateConservations } from '@/services/chat'
import { yupResolver } from '@hookform/resolvers/yup'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import React, { useEffect, useState } from 'react'
import { useForm } from 'react-hook-form'
import { toast } from 'react-toastify'
import * as yup from 'yup'
import notificationNoticeSound from "/notification.mp3"
// const messages = [
//   {
//     "SenderId": 1,
//     "AvatarSender": "",
//     "ReceiverId": 2,
//     "AvatarReceiver": "",
//     "Content": "Hi Alice! I'm good, just finished a great book. How about you?",
//     "DateCreated": 1,
//   },
//   {
//     "SenderId": 1,
//     "AvatarSender": "",
//     "ReceiverId": 2,
//     "AvatarReceiver": "",
//     "Content": "How are you doing today?",
//     "DateCreated": 2,
//   },
//   {
//     "SenderId": 2,
//     "AvatarSender": "",
//     "ReceiverId": 1,
//     "AvatarReceiver": "",
//     "Content": "Hey Bob, how's it going?",
//     "DateCreated": 3,
//   },
//   {
//     "SenderId": 1,
//     "AvatarSender": "",
//     "ReceiverId": 2,
//     "AvatarReceiver": "",
//     "Content": "Did you watch the game last night?",
//     "DateCreated": 4,
//   }]
export default function ChatPage() {
  const schema = yup
    .object({
      message: yup.string().required('')
    })
    .required()
  const {
    handleSubmit,
    control,
    reset,
    register,
    formState: { errors }
  } = useForm({ resolver: yupResolver(schema) })

  const queryClient = useQueryClient()
  const [openNav, setOpenNav] = useState(true)
  const [currentSelect, setCurrentSelect] = useState("history")
  const [senderIsTyping, setSenderIsTyping] = useState(true)
  const { profile } = useAppContext()
  const [firstChatMessages, setFirstChatMessages] = useState()
  const [currentReceiver, setCurrentReceiver] = useState()
  const { connections } = useSignalRContext();

  useEffect(() => {
    const connection = connections["ChatHub"];
    if (connection) {

      const handleNewAnnouncement = (data) => {
        const sound = new Audio(notificationNoticeSound)
        sound.play()
        console.log("data" + data);
        // let newMessage = null;
        // if (profile?.id != data?.senderId) {
        //   newMessage = {
        //     senderId: data?.receiverId,
        //     receiverId: data?.senderId,
        //     avatarReceiver: data?.avatarSender,
        //     avatarSender: data?.avatarReceiver,
        //     content: data?.content,
        //     chatId: currentReceiver?.chatId
        //   }
        // }
        setFirstChatMessages((prev) => {
          const currentData = [...prev]
          currentData.push(data);
          return currentData;
        });
      };

      // Add event listener
      connection.on("ReceiveNewPrivateMessage", handleNewAnnouncement);

      // Cleanup: Remove event listener when component unmounts
      return () => {
        // connection.off("ReceiveNewPrivateMessage", handleNewAnnouncement);
      };
    }
  }, [connections["ChatHub"]])

  const { data: allUsersData } = useQuery({
    queryKey: ['allUser'],
    queryFn: (_) => getCurrentOnlineUser()
  })

  const { data: historyData } = useQuery({
    queryKey: ['historyData'],
    queryFn: (_) => getPrivateConservations(),
    enabled: currentSelect == "history"
  })

  useEffect(() => {
    if (historyData && historyData?.data) {
      setCurrentReceiver(historyData?.data[0])
      setFirstChatMessages(historyData?.data[0]?.currentMessagesReceiver)
    }
  }, [historyData])

  const createConversationMutation = useMutation({
    mutationFn: (receiverId) => createNewConservation(receiverId)
  })


  const handleClickUser = (receiverId) => {
    if (receiverId) {
      createConversationMutation.mutate(receiverId, {
        onSuccess() {
          setCurrentSelect("history")
        },
        onSettled() {
          queryClient.invalidateQueries(['historyData'])
        },
        onError(err) {
          console.log(err)
        }
      })
    } else {
      toast.error("Please provide receiver ID")
    }
  }

  console.log(allUsersData?.data)

  const handleChangeUserChat = async (item) => {
    setCurrentReceiver(item)
    const data = await getDetailConservations(item?.receiverId)
    const currentReceiver = data?.data.find((listId) => listId.receiverId === item.receiverId)
    setFirstChatMessages(currentReceiver?.currentMessagesReceiver)
  }
  const addMessage = useMutation({
    mutationFn: (body) => addNewMessage(body)
  })
  console.log(firstChatMessages)
  const onAddMessage = (data) => {
    let message = {
      senderId: currentReceiver?.currentUserId,
      receiverId: currentReceiver?.receiverId,
      content: data?.message,
      chatId: currentReceiver?.chatId
    }
    addMessage.mutate(message, {
      onSuccess(data) {
        reset({ message: "" })
      },
      onError(err) {
        console.log(err)
      }
    })
  }

  return (
    <GeneralLayout>
      <div className="flex flex-wrap h-screen overflow-hidden medium:flex">
        <div className={`${!openNav ? "medium:w-[250px]" : "medium:w-1/4"} w-full md:h-auto h-[120px] flex flex-col transition-all duration-300 ease-in-out bg-white border-r border-gray-300`}>
          <header className="items-center justify-between hidden w-full p-4 text-white bg-purple-500 border-b border-gray-300 medium:flex">
            <h1 className="text-2xl font-semibold medium:">My chat</h1>
            <div className="relative">
              <button id="menuButton" className="hidden focus:outline-none sm:block" onClick={() => setOpenNav(!openNav)}>
                {openNav ? <svg xmlns="http://www.w3.org/2000/svg" className="w-5 h-5 text-gray-100" viewBox="0 0 20 20" fill="currentColor">
                  <path d="M10 12a2 2 0 100-4 2 2 0 000 4z" />
                  <path d="M2 10a2 2 0 012-2h12a2 2 0 012 2 2 2 0 01-2 2H4a2 2 0 01-2-2z" />
                </svg> : <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-6 h-6">
                  <path strokeLinecap="round" strokeLinejoin="round" d="M12 4.5v15m7.5-7.5h-15" />
                </svg>
                }
              </button>
            </div>
          </header>
          <Tabs defaultValue={"online"} value={currentSelect} className="" onValueChange={setCurrentSelect}>
            <TabsList>
              <TabsTrigger value="online" >Current Online </TabsTrigger>
              <TabsTrigger value="history">History</TabsTrigger>
            </TabsList>
            <TabsContent value="online">
              <div className="flex flex-row h-full p-3 overflow-hidden overflow-x-auto medium:overflow-y-auto medium:pb-10 medium:h-screen medium:mb-9 medium:flex-col">
                {allUsersData?.data && allUsersData.data.map((item) => (
                  <div key={item?.receiverId} className="flex flex-col items-center p-2 rounded-md cursor-pointer medium:mb-4 hover:bg-gray-100 medium:flex-row" onClick={() => handleClickUser(item?.receiverId)}>
                    <div className="relative flex-shrink-0 w-12 h-12 mr-3 bg-gray-300 rounded-full" key={item?.id}>
                      <img src={`${item?.avatar ? item?.avatar : "https://cdn-icons-png.freepik.com/256/1077/1077063.png?semt=ais_hybrid"}`} alt="User Avatar" className="w-12 h-12 rounded-full" />
                      {item?.isOnline && <div className="absolute bottom-0 w-3 h-3 bg-green-400 rounded-full right-1"></div>}
                    </div>
                    <div className="flex-1 ">
                      <h2 className="text-lg font-semibold">{item?.username}</h2>
                      <span className="font-medium text-md">{item?.role}</span>
                    </div>
                  </div>))}
              </div>
            </TabsContent>
            <TabsContent value="history">
              <div className="flex flex-row h-full p-3 overflow-hidden overflow-x-auto medium:overflow-y-auto medium:pb-10 medium:h-screen medium:mb-9 medium:flex-col">
                {historyData?.data && historyData?.data.map((item) => (
                  <div key={item.receiverId} className={`flex flex-col items-center p-2 rounded-md cursor-pointer medium:mb-4 hover:bg-gray-100 medium:flex-row ${currentReceiver?.receiverId === item?.receiverId ? "bg-purple-400 text-white" : ""}`} onClick={() => { handleChangeUserChat(item) }}>
                    <div className="flex-shrink-0 w-12 h-12 mr-3 bg-gray-300 rounded-full">
                      <img src={item?.avatar} alt="User Avatar" className="w-12 h-12 rounded-full" />
                    </div>
                    <div className="flex-1 ">
                      <h2 className="text-lg font-semibold">{item?.username}</h2>
                      <p className="font-semibold text-black text-md">
                        {item?.role}
                      </p>
                    </div>
                  </div>))}
              </div>
            </TabsContent>
          </Tabs>

        </div>
        <div className="flex-1">
          {currentSelect === "history" && <div>
            <header className="p-4 text-white text-gray-700 bg-purple-500">
              <h1 className="text-2xl font-semibold">{currentReceiver?.username}</h1>
            </header>
            <div className="h-[50vh] medium:h-[70vh] p-4 overflow-y-auto pb-36 relative">

              {/* Receiver */}
              {firstChatMessages && firstChatMessages?.map((item) => {
                if (item?.senderId === profile?.id) {
                  return (
                    <div className="flex justify-end mb-4 cursor-pointer" key={item.id}>
                      <div className="flex gap-3 p-3 text-white bg-indigo-500 rounded-lg max-w-96">
                        <p>{item?.content}</p>
                      </div>
                      <div className="flex items-center justify-center ml-2 rounded-full w-9 h-9">
                        <img src={item?.avatarSender} alt="My Avatar" className="w-8 h-8 rounded-full" />
                      </div>
                    </div>
                  )
                }
                return (
                  <div className="flex mb-4 cursor-pointer" key={item.id}>
                    <div className="flex items-center justify-center mr-2 rounded-full w-9 h-9">
                      <img src={item?.avatarReceiver} alt="User Avatar" className="w-8 h-8 rounded-full" />
                    </div>
                    <div className="flex gap-3 p-3 bg-white rounded-lg max-w-96">
                      <p className="text-gray-700">{item?.content}</p>
                    </div>
                  </div>
                )
              })}
              {/* Typing UI */}
              {senderIsTyping && <div className="flex mb-4 cursor-pointer">
                <div className="flex items-center justify-center mr-2 rounded-full w-9 h-9">
                  <img src={"https://s120-ava-talk.zadn.vn/9/7/9/4/21/120/e0813d7d04dcb0ed3eba0438a00aa62b.jpg"} alt="User Avatar" className="w-8 h-8 rounded-full" />
                </div>
                <div className="flex gap-3 p-3 bg-white rounded-lg max-w-96">
                  <p className="text-gray-700">
                    <span className="flex items-center gap-1 jumping-dots">
                      <span className="dot-1">
                        <div className="w-2 h-2 rounded-full bg-slate-400"></div>
                      </span>
                      <span className="dot-2">
                        <div className="w-2 h-2 rounded-full bg-slate-400"></div>
                      </span>
                      <span className="dot-3">
                        <div className="w-2 h-2 rounded-full bg-slate-400"></div>
                      </span>
                    </span>
                  </p>
                </div>
              </div>
              }
              {/* Sender */}

            </div>
            <div className="flex items-center justify-center w-full">
              <div className="fixed bottom-0 w-full p-4 mx-auto bg-white border-gray-300 medium:absolute medium:w-3/4 ">
                <Separator class></Separator>
                <form className="flex items-center" onSubmit={handleSubmit(onAddMessage)}>
                  <input type="text" placeholder="Type a message..." className={`w-full p-2 border border-gray-400 rounded-md focus:outline-none focus:border-blue-500`} {...register('message')} />
                  <button className={`px-4 py-2 ml-2 text-white bg-indigo-500 rounded-md ${addMessage.isLoading ? "disabled" : ""}`}>Send</button>
                </form>
              </div>
            </div>
          </div>}
        </div>
      </div>
    </GeneralLayout>
  )
}



