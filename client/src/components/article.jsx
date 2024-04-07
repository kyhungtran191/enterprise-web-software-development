import React from 'react'
import { Badge } from './ui/badge'
import { formatDate } from '@/utils/helper'
import { Link, useNavigate } from 'react-router-dom'
import { Button } from './ui/button'
import { useAppContext } from '@/hooks/useAppContext'
import { useMutateApprove } from '@/query/useMutateApprove'
import Swal from 'sweetalert2'
import { toast } from 'react-toastify'
import ActionSpinner from './ActionSpinner'
import { useQueryClient } from '@tanstack/react-query'
import { Roles } from '@/constant/roles'
import CustomRejectComponent from './CustomRejectComponent'

export default function Article({ isRevert = false, className, status, classImageCustom, article }) {
  const navigate = useNavigate()
  const { mutate, isLoading } = useMutateApprove()
  const { profile } = useAppContext()
  const queryClient = useQueryClient();
  const handleOnClickNavigate = () => {
    if (article.publicDate || article.status === "APPROVED") {
      navigate(`/contributions/${article.slug}`)
    } else {
      navigate(`/preview/${article.slug}`)
    }
  }
  const handleApproveArticle = (e, id) => {
    const ids = []
    ids.push(id);
    e.stopPropagation()
    Swal.fire({
      title: "Are you sure?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, Approve it!"
    }).then((result) => {
      if (result.isConfirmed) {
        mutate({ ids: ids }, {
          onSuccess() {
            toast.success("Approve Article Successfully!")
            queryClient.invalidateQueries('mc-contributions')
            navigate("/coodinator-manage/contributions?status=APPROVE")
          },
          onError(err) {
            console.log(err)
          }
        })
      }
    });
  }
  return (
    <div onClick={handleOnClickNavigate}>
      {isLoading && <ActionSpinner></ActionSpinner>}
      <div className={`flex ${isRevert ? "flex-col md:flex-row" : "flex-col"} items-start gap-3 ${className} cursor-pointer hover:bg-slate-100 p-2 rounded-lg`}>
        <img src={`${article?.thumbnails?.length > 0 ? article?.thumbnails[0].path : "https://variety.com/wp-content/uploads/2021/04/Avatar.jpg"}`} alt="" className={`${isRevert ? `w-full md:w-[35%] h-[300px] md:h-[300px] ${classImageCustom}` : `w-full h-[600px] ${classImageCustom}`}  object-cover rounded-md`} />
        <div className="flex-1 p-2">
          <div className="flex flex-wrap items-center justify-between gap-2">
            <div className='flex items-center gap-1 medium:gap-2'>
              <img src="https://variety.com/wp-content/uploads/2021/04/Avatar.jpg" alt="" className="flex-shrink-0 object-cover w-12 h-12 rounded-full" />
              <h3 className='text-sm font-semibold medium:text-base'>{article?.userName}</h3>
            </div>
            <div className='flex flex-col'>
              <div> {status && <Badge variant="outline" className={`${status === "PENDING" ? "text-yellow-500" : status === "APPROVED" ? "text-green-500" : "text-red-500"} font-semibold`} >{status}</Badge>}
                <Badge variant="outline">{article?.facultyName}</Badge></div>
              <></>
            </div>
          </div>
          <h2 className="text-ellipsis line-clamp-2 medium:h-[65px] font-semibold text-xl medium:text-2xl mt-3">{article?.title}</h2>
          <p className='text-sm text-ellipsis line-clamp-3 text-slate-700 medium:text-base h-[72px]'>{article.shortDescription}</p>
          <p className="mt-2 text-sm medium:text-base">{formatDate(article?.publicDate)}</p>
          {profile && profile?.roles == Roles.Coordinator && status && status == "PENDING" && <div className="flex items-center gap-10 my-10">
            <Button className="w-[150px] bg-green-600 shadow-lg" onClick={(e) => handleApproveArticle(e, article.id)}>Approve this</Button>
            <Button className="!bg-none p-0" onClick={(e) => { e.stopPropagation() }}>
              <CustomRejectComponent id={article?.id}></CustomRejectComponent>
            </Button>
          </div>}
          {profile && profile?.roles == "Student" && status && status == "PENDING" && <div onClick={(e) => {
            e.stopPropagation();
            navigate(`/student-manage/edit-contribution/${article.slug}`)
          }} className="mt-4 text-lg font-semibold text-blue-500 underline">Edit</div>}
        </div>
      </div>
    </div >

  )
}
