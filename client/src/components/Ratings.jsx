import React, { useEffect, useState } from 'react'
import { Button } from './ui/button';
import { toast } from 'react-toastify'
import { useMutation } from '@tanstack/react-query';
import { Contributions } from '@/services/client';
import { useQueryClient } from '@tanstack/react-query';
export default function Ratings({ id, currentRating }) {


  const [selectedStar, setSelectedStar] = useState(0);
  const queryClient = useQueryClient()
  const { mutate } = useMutation({
    mutationFn: (data) => Contributions.ratePublic(data)
  })

  const handleSubmitRate = () => {
    if (selectedStar == 0) {
      toast.error("Please provide rate star")
      return 0;
    } else {
      mutate({ id, rating: selectedStar }, {
        onSuccess() {
          toast.success("Submit new Rate successfully!")
        },
        onSettled() {
          queryClient.invalidateQueries(['detail-contributions']);
        },
        onError(data) {
          const errorMessage = data && data?.response?.data?.title
          toast.error(errorMessage)
        }
      })
    }
  }
  const handleStarHover = (value) => {
    setSelectedStar(value);
  };

  const handleStarLeave = () => {
    // setSelectedStar(0)
  };

  const handleStarClick = (value) => {
    setSelectedStar(value);
    // Gửi dữ liệu đánh giá (value) đến server hoặc xử lý dữ liệu tại đây
  };
  useEffect(() => {
    setSelectedStar(currentRating || 0)
  }, [currentRating])

  return (
    <>
      <h2 className='my-2 text-lg font-bold'>Rating</h2>
      <div className="flex items-center gap-2">
        {[1, 2, 3, 4, 5].map((value) => (
          <span
            key={value}
            className={`text-gray-500 text-3xl cursor-pointer 
              }`}
            onMouseEnter={() => handleStarHover(value)}
            onMouseLeave={handleStarLeave}
            onClick={() => handleStarClick(value)}
          >
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className={`w-6 h-6 ${value <= selectedStar ? 'bg-yellow-500 text-white' : 'bg-white'}`}>
              <path strokeLinecap="round" strokeLinejoin="round" d="M11.48 3.499a.562.562 0 0 1 1.04 0l2.125 5.111a.563.563 0 0 0 .475.345l5.518.442c.499.04.701.663.321.988l-4.204 3.602a.563.563 0 0 0-.182.557l1.285 5.385a.562.562 0 0 1-.84.61l-4.725-2.885a.562.562 0 0 0-.586 0L6.982 20.54a.562.562 0 0 1-.84-.61l1.285-5.386a.562.562 0 0 0-.182-.557l-4.204-3.602a.562.562 0 0 1 .321-.988l5.518-.442a.563.563 0 0 0 .475-.345L11.48 3.5Z" />
            </svg>
          </span>
        ))}
      </div>
      <Button className="w-full px-2 py-3 mt-4 bg-cyan-500 hover:bg-cyan-600" onClick={handleSubmitRate}>Submit</Button>
    </>
  )
}
