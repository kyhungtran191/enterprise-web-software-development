import { Separator } from '@/components/ui/separator'
import GeneralLayout from '@/layouts'
import React, { useState } from 'react'

export default function ChatPage() {
  const [openNav, setOpenNav] = useState(false)
  return (
    <GeneralLayout>
      <div className="flex flex-wrap h-screen overflow-hidden medium:flex">
        <div className={`${!openNav ? "medium:w-[250px]" : "medium:w-1/4"} w-full md:h-auto h-[120px] flex medium:flex-col flex-row transition-all duration-300 ease-in-out bg-white border-r border-gray-300`}>
          <header className="items-center justify-between hidden w-full p-4 text-white bg-indigo-600 border-b border-gray-300 medium:flex">
            <h1 className="text-2xl font-semibold medium:">History</h1>
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
          <div className="flex flex-row h-full p-3 overflow-hidden overflow-x-auto medium:overflow-y-auto medium:pb-20 medium:h-screen mb-9 medium:flex-col">
            <div className="flex flex-col items-center p-2 mb-4 rounded-md cursor-pointer hover:bg-gray-100 medium:flex-row">
              <div className="flex-shrink-0 w-12 h-12 mr-3 bg-gray-300 rounded-full">
                <img src="https://placehold.co/200x/ffa8e4/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="User Avatar" className="w-12 h-12 rounded-full" />
              </div>
              <div className="flex-1 ">
                <h2 className="text-lg font-semibold">Alice</h2>
                {openNav && <p className="text-gray-600">Hoorayy!!</p>}
              </div>
            </div>
            <div className="flex flex-col items-center p-2 mb-4 rounded-md cursor-pointer hover:bg-gray-100 medium:flex-row">
              <div className="flex-shrink-0 w-12 h-12 mr-3 bg-gray-300 rounded-full">
                <img src="https://placehold.co/200x/ffa8e4/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="User Avatar" className="w-12 h-12 rounded-full" />
              </div>
              <div className="flex-1 ">
                <h2 className="text-lg font-semibold">Alice</h2>
                {openNav && <p className="text-gray-600">Hoorayy!!</p>}
              </div>
            </div>
            <div className="flex flex-col items-center p-2 mb-4 rounded-md cursor-pointer hover:bg-gray-100 medium:flex-row">
              <div className="flex-shrink-0 w-12 h-12 mr-3 bg-gray-300 rounded-full">
                <img src="https://placehold.co/200x/ffa8e4/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="User Avatar" className="w-12 h-12 rounded-full" />
              </div>
              <div className="flex-1 ">
                <h2 className="text-lg font-semibold">Alice</h2>
                {openNav && <p className="text-gray-600">Hoorayy!!</p>}
              </div>
            </div>
            <div className="flex flex-col items-center p-2 mb-4 rounded-md cursor-pointer hover:bg-gray-100 medium:flex-row">
              <div className="flex-shrink-0 w-12 h-12 mr-3 bg-gray-300 rounded-full">
                <img src="https://placehold.co/200x/ffa8e4/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="User Avatar" className="w-12 h-12 rounded-full" />
              </div>
              <div className="flex-1 ">
                <h2 className="text-lg font-semibold">Alice</h2>
                {openNav && <p className="text-gray-600">Hoorayy!!</p>}
              </div>
            </div>
            <div className="flex flex-col items-center p-2 mb-4 rounded-md cursor-pointer hover:bg-gray-100 medium:flex-row">
              <div className="flex-shrink-0 w-12 h-12 mr-3 bg-gray-300 rounded-full">
                <img src="https://placehold.co/200x/ffa8e4/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="User Avatar" className="w-12 h-12 rounded-full" />
              </div>
              <div className="flex-1 ">
                <h2 className="text-lg font-semibold">Alice</h2>
                {openNav && <p className="text-gray-600">Hoorayy!!</p>}
              </div>
            </div>
            <div className="flex flex-col items-center p-2 mb-4 rounded-md cursor-pointer hover:bg-gray-100 medium:flex-row">
              <div className="flex-shrink-0 w-12 h-12 mr-3 bg-gray-300 rounded-full">
                <img src="https://placehold.co/200x/ffa8e4/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="User Avatar" className="w-12 h-12 rounded-full" />
              </div>
              <div className="flex-1 ">
                <h2 className="text-lg font-semibold">Alice</h2>
                {openNav && <p className="text-gray-600">Hoorayy!!</p>}
              </div>
            </div>
            <div className="flex flex-col items-center p-2 mb-4 rounded-md cursor-pointer hover:bg-gray-100 medium:flex-row">
              <div className="flex-shrink-0 w-12 h-12 mr-3 bg-gray-300 rounded-full">
                <img src="https://placehold.co/200x/ffa8e4/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="User Avatar" className="w-12 h-12 rounded-full" />
              </div>
              <div className="flex-1 ">
                <h2 className="text-lg font-semibold">Alice</h2>
                {openNav && <p className="text-gray-600">Hoorayy!!</p>}
              </div>
            </div>
          </div>
        </div>
        <div className="flex-1">
          <header className="p-4 text-gray-700 bg-white">
            <h1 className="text-2xl font-semibold">Alice</h1>
          </header>
          <div className="h-[50vh] medium:h-[70vh] p-4 overflow-y-auto pb-36 relative">
            <div className="flex mb-4 cursor-pointer">
              <div className="flex items-center justify-center mr-2 rounded-full w-9 h-9">
                <img src="https://placehold.co/200x/ffa8e4/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="User Avatar" className="w-8 h-8 rounded-full" />
              </div>
              <div className="flex gap-3 p-3 bg-white rounded-lg max-w-96">
                <p className="text-gray-700">Hey Bob, how's it going?</p>
              </div>
            </div>
            <div className="flex mb-4 cursor-pointer">
              <div className="flex items-center justify-center mr-2 rounded-full w-9 h-9">
                <img src="https://placehold.co/200x/ffa8e4/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="User Avatar" className="w-8 h-8 rounded-full" />
              </div>
              <div className="flex gap-3 p-3 bg-white rounded-lg max-w-96">
                <p className="text-gray-700">Hey Bob, how's it going?</p>
              </div>
            </div>
            <div className="flex mb-4 cursor-pointer">
              <div className="flex items-center justify-center mr-2 rounded-full w-9 h-9">
                <img src="https://placehold.co/200x/ffa8e4/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="User Avatar" className="w-8 h-8 rounded-full" />
              </div>
              <div className="flex gap-3 p-3 bg-white rounded-lg max-w-96">
                <p className="text-gray-700">Hey Bob, how's it going?</p>
              </div>
            </div>
            <div className="flex mb-4 cursor-pointer">
              <div className="flex items-center justify-center mr-2 rounded-full w-9 h-9">
                <img src="https://placehold.co/200x/ffa8e4/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="User Avatar" className="w-8 h-8 rounded-full" />
              </div>
              <div className="flex gap-3 p-3 bg-white rounded-lg max-w-96">
                <p className="text-gray-700">Hey Bob, how's it going?</p>
              </div>
            </div>
            <div className="flex mb-4 cursor-pointer">
              <div className="flex items-center justify-center mr-2 rounded-full w-9 h-9">
                <img src="https://placehold.co/200x/ffa8e4/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="User Avatar" className="w-8 h-8 rounded-full" />
              </div>
              <div className="flex gap-3 p-3 bg-white rounded-lg max-w-96">
                <p className="text-gray-700">Hey Bob, how's it going?</p>
              </div>
            </div>
            <div className="flex mb-4 cursor-pointer">
              <div className="flex items-center justify-center mr-2 rounded-full w-9 h-9">
                <img src="https://placehold.co/200x/ffa8e4/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="User Avatar" className="w-8 h-8 rounded-full" />
              </div>
              <div className="flex gap-3 p-3 bg-white rounded-lg max-w-96">
                <p className="text-gray-700">Hey Bob, how's it going?</p>
              </div>
            </div>
            <div className="flex mb-4 cursor-pointer">
              <div className="flex items-center justify-center mr-2 rounded-full w-9 h-9">
                <img src="https://placehold.co/200x/ffa8e4/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="User Avatar" className="w-8 h-8 rounded-full" />
              </div>
              <div className="flex gap-3 p-3 bg-white rounded-lg max-w-96">
                <p className="text-gray-700">Hey Bob, how's it going?</p>
              </div>
            </div>
            <div className="flex mb-4 cursor-pointer">
              <div className="flex items-center justify-center mr-2 rounded-full w-9 h-9">
                <img src="https://placehold.co/200x/ffa8e4/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="User Avatar" className="w-8 h-8 rounded-full" />
              </div>
              <div className="flex gap-3 p-3 bg-white rounded-lg max-w-96">
                <p className="text-gray-700">Hey Bob, how's it going?</p>
              </div>
            </div>
            <div className="flex justify-end mb-4 cursor-pointer">
              <div className="flex gap-3 p-3 text-white bg-indigo-500 rounded-lg max-w-96">
                <p>Hi Alice! I'm good, just finished a great book. How about you?</p>
              </div>
              <div className="flex items-center justify-center ml-2 rounded-full w-9 h-9">
                <img src="https://placehold.co/200x/b7a8ff/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="My Avatar" className="w-8 h-8 rounded-full" />
              </div>
            </div>
            <div className="flex mb-4 cursor-pointer">
              <div className="flex items-center justify-center mr-2 rounded-full w-9 h-9">
                <img src="https://placehold.co/200x/ffa8e4/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="User Avatar" className="w-8 h-8 rounded-full" />
              </div>
              <div className="flex gap-3 p-3 bg-white rounded-lg max-w-96">
                <p className="text-gray-700">That book sounds interesting! What's it about?</p>
              </div>
            </div>
            <div className="flex justify-end mb-4 cursor-pointer">
              <div className="flex gap-3 p-3 text-white bg-indigo-500 rounded-lg max-w-96">
                <p>Anytime! Let me know how you like it. 😊</p>
              </div>
              <div className="flex items-center justify-center ml-2 rounded-full w-9 h-9">
                <img src="https://placehold.co/200x/b7a8ff/ffffff.svg?text=ʕ•́ᴥ•̀ʔ&font=Lato" alt="My Avatar" className="w-8 h-8 rounded-full" />
              </div>
            </div>
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

        </div>
      </div>
    </GeneralLayout>
  )
}


