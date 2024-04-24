import { MessageSquare } from 'lucide-react'
import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import notificationNoticeSound from "/notification.mp3"
import { useSignalRContext } from '@/contexts/signalr.context';
export default function Message() {
  const { connections } = useSignalRContext();
  const [isNew, setIsNew] = useState(false)
  useEffect(() => {
    const connection = connections["ChatHub"];
    if (connection) {

      const handleNewAnnouncement = (data) => {
        setIsNew(true)
      };

      // Add event listener
      connection.on("ReceiveNewPrivateMessage", handleNewAnnouncement);

      // Cleanup: Remove event listener when component unmounts
      return () => {
        // connection.off("ReceiveNewPrivateMessage", handleNewAnnouncement);
      };
    }
  }, [connections["ChatHub"]])
  return (
    <Link
      to='/message'
      className='relative flex items-center justify-center w-12 h-12 transition-colors duration-300 ease-in-out rounded-full cursor-pointer hover:bg-slate-100'
    >
      {isNew && <div className="absolute w-3 h-3 bg-red-500 rounded-full top-3 right-3"></div>}
      <MessageSquare></MessageSquare>
    </Link>
  )
}
