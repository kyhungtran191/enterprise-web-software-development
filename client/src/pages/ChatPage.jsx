import { Separator } from '@/components/ui/separator'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { useAppContext } from '@/hooks/useAppContext'
import GeneralLayout from '@/layouts'
import React, { useState } from 'react'
const messages = [
  {
    "SenderId": 1,
    "AvatarSender": "",
    "ReceiverId": 2,
    "AvatarReceiver": "",
    "Content": "Hi Alice! I'm good, just finished a great book. How about you?",
    "DateCreated": 1,
  },
  {
    "SenderId": 1,
    "AvatarSender": "",
    "ReceiverId": 2,
    "AvatarReceiver": "",
    "Content": "How are you doing today?",
    "DateCreated": 2,
  },
  {
    "SenderId": 2,
    "AvatarSender": "",
    "ReceiverId": 1,
    "AvatarReceiver": "",
    "Content": "Hey Bob, how's it going?",
    "DateCreated": 3,
  },
  {
    "SenderId": 1,
    "AvatarSender": "",
    "ReceiverId": 2,
    "AvatarReceiver": "",
    "Content": "Did you watch the game last night?",
    "DateCreated": 4,
  }]
export default function ChatPage() {
  const [openNav, setOpenNav] = useState(true)
  const [currentSelect, setCurrentSelect] = useState("online")
  console.log(currentSelect)
  const { profile } = useAppContext()
  const [firstChatMessages, setFirstChatMessages] = useState()
  const [currentReceiver, setCurrentReceiver] = useState()
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
                {Array(10).fill(0).map((item) => (
                  <div key={item} className="flex flex-col items-center p-2 rounded-md cursor-pointer medium:mb-4 hover:bg-gray-100 medium:flex-row" onClick={() => { setCurrentSelect("history") }}>
                    <div className="flex-shrink-0 w-12 h-12 mr-3 bg-gray-300 rounded-full">
                      <img src="https://placehold.co/200x/ffa8e4/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="User Avatar" className="w-12 h-12 rounded-full" />
                    </div>
                    <div className="flex-1 ">
                      <h2 className="text-lg font-semibold">Alice</h2>
                      {openNav && <p className="text-gray-600">Hoorayy!!</p>}
                    </div>
                  </div>))}
              </div>
            </TabsContent>
            <TabsContent value="history">
              <div className="flex flex-row h-full p-3 overflow-hidden overflow-x-auto medium:overflow-y-auto medium:pb-10 medium:h-screen medium:mb-9 medium:flex-col">
                {Array(10).fill(0).map((item) => (<div key={item} className="flex flex-col items-center p-2 rounded-md cursor-pointer medium:mb-4 hover:bg-gray-100 medium:flex-row">
                  <div className="flex-shrink-0 w-12 h-12 mr-3 bg-gray-300 rounded-full">
                    <img src="https://placehold.co/200x/ffa8e4/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="User Avatar" className="w-12 h-12 rounded-full" />
                  </div>
                  <div className="flex-1 ">
                    <h2 className="text-lg font-semibold">Alice</h2>
                    {openNav && <p className="text-gray-600">Hoorayy!!</p>}
                  </div>
                </div>))}
              </div>
            </TabsContent>
          </Tabs>

        </div>
        <div className="flex-1">
          {currentSelect === "history" && <div>
            <header className="p-4 text-white text-gray-700 bg-purple-500">
              <h1 className="text-2xl font-semibold">Alice</h1>
            </header>
            <div className="h-[50vh] medium:h-[70vh] p-4 overflow-y-auto pb-36 relative">

              {/* Receiver */}
              {messages?.map((item) => {
                if (item?.SenderId === 1) {
                  return (
                    <div className="flex justify-end mb-4 cursor-pointer" key={item.id}>
                      <div className="flex gap-3 p-3 text-white bg-indigo-500 rounded-lg max-w-96">
                        <p>{item?.Content}</p>
                      </div>
                      <div className="flex items-center justify-center ml-2 rounded-full w-9 h-9">
                        <img src="https://placehold.co/200x/b7a8ff/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="My Avatar" className="w-8 h-8 rounded-full" />
                      </div>
                    </div>
                  )
                }
                return (
                  <div className="flex mb-4 cursor-pointer" key={item.id}>
                    <div className="flex items-center justify-center mr-2 rounded-full w-9 h-9">
                      <img src="https://placehold.co/200x/ffa8e4/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="User Avatar" className="w-8 h-8 rounded-full" />
                    </div>
                    <div className="flex gap-3 p-3 bg-white rounded-lg max-w-96">
                      <p className="text-gray-700">{item?.Content}</p>
                    </div>
                  </div>
                )
              })}


              {/* Sender */}

            </div>
            <div className="flex items-center justify-center w-full">
              <div className="fixed bottom-0 w-full p-4 mx-auto bg-white border-gray-300 medium:absolute medium:w-3/4 ">
                <Separator class></Separator>
                <div className="flex items-center">
                  <input type="text" placeholder="Type a message..." className="w-full p-2 border border-gray-400 rounded-md focus:outline-none focus:border-blue-500" />
                  <button className="px-4 py-2 ml-2 text-white bg-indigo-500 rounded-md">Send</button>
                </div>
              </div>
            </div>
          </div>}
        </div>
      </div>
    </GeneralLayout>
  )
}



