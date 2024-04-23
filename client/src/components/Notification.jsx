import React, { useEffect, useRef, useState } from 'react'
import { Popover, PopoverContent, PopoverTrigger } from './ui/popover'
import { Bell } from 'lucide-react'
import { Link } from 'react-router-dom'
import { Avatar, AvatarFallback, AvatarImage } from './ui/avatar'
import { Separator } from '@radix-ui/react-dropdown-menu'
import { getAccessTokenFromLS } from '@/utils/auth'
import { useSignalRContext } from '@/contexts/signalr.context'
import notificationNoticeSound from "/notification.mp3"
import { useQuery } from '@tanstack/react-query'
import instanceAxios from '@/utils/axiosInstance'
export default function Notification() {
  const [currentData, setCurrentData] = useState()
  const { connections } = useSignalRContext();
  const { data } = useQuery({
    queryKey: ['notification'],
    queryFn: () => instanceAxios.get("http://localhost:5272/Announcements/paging", {
      params: {
        pageIndex: 1,
        pageSize: 10
      }
    })
  })
  useEffect(() => {
    setCurrentData(data?.data?.results)
  }, [data])
  useEffect(() => {
    const connection = connections["AnnouncementHub"];
    if (connection) {
      const handleNewAnnouncement = (data) => {
        console.log(data)
        const sound = new Audio(notificationNoticeSound)
        sound.play()
        setCurrentData((prev) => {
          const currentData = [...prev]
          currentData.unshift(data)
          return currentData
        })
      };

      // Add event listener
      connection.on("GetNewAnnouncement", handleNewAnnouncement);

      // Cleanup: Remove event listener when component unmounts
      return () => {
        connection.off("GetNewAnnouncement", handleNewAnnouncement);
      };
    }
  }, [connections["AnnouncementHub"]])

  console.log(currentData)
  return (
    <div className='relative flex items-center justify-center w-12 h-12 transition-colors duration-300 ease-in-out rounded-full cursor-pointer hover:bg-slate-100'>
      <div className="absolute w-[10px] h-[10px] bg-red-500 rounded-full bottom-6 left-6 z-20"></div>
      <Popover className="">
        <PopoverTrigger className='flex items-center justify-center w-12 h-12'>
          <Bell className='relative w-6 h-6'>
          </Bell>
        </PopoverTrigger>
        <PopoverContent className="p-0 h-[350px] overflow-y-scroll">
          <div className="">
            {/* Notificate component */}
            {currentData && currentData?.length > 0 && currentData?.map((item) => (
              <div key={item.id}>
                <Link className='flex items-center w-full gap-5 px-3 py-2 hover:bg-slate-100'>
                  <Avatar>
                    <AvatarImage
                      src={`https://variety.com/wp-content/uploads/2021/04/Avatar.jpg`}
                      className='object-cover'
                    ></AvatarImage>
                    <AvatarFallback>CN</AvatarFallback>
                  </Avatar>
                  <div className="flex-1">
                    <span className='text-xs font-bold line-clamp-3'>Tran Ky Hung sent you a message Tran Ky Hung sent you a message
                      Tran Ky Hung sent you a message  Tran Ky Hung sent you a message Tran Ky Hung sent you a message Tran Ky Hung sent you a message
                      Tran Ky Hung sent you a message  Tran Ky Hung sent you a message</span>
                  </div>
                </Link>
                <Separator className="my-2" />
              </div>
            ))}
          </div>
        </PopoverContent>
      </Popover>
    </div>
  )
}
