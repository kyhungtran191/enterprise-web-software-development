import { Icon } from '@iconify/react'
import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom';

export default function Search({ className }) {
  const [searchQuery, setSearchQuery] = useState('');
  console.log("Search Rerender")
  let navigate = useNavigate()
  const handleKeyPress = (e) => {
    if (e.key === 'Enter') {
      // Nếu phím gõ là "Enter", thực hiện chuyển hướng đến trang `/contributions?search=`
      navigate(`/contributions?search=${searchQuery}`)
    }
  };
  const handleChange = (e) => {
    setSearchQuery(e.target.value);
  };
  return (
    <div className={`flex items-center w-full px-5 py-4 border rounded-lg gap-x-2 ${className}`}>
      <Icon icon="ic:outline-search" className="flex-shrink-0 w-6 h-6 text-slate-700"></Icon>
      <input type="text" className='flex-1 border-none outline-none' placeholder="What you're looking for ?" value={searchQuery}
        onChange={handleChange}
        onKeyPress={handleKeyPress} />
    </div>
  )
}
