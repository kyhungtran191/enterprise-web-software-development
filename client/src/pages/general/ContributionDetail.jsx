import DynamicBreadcrumb from '@/components/DynamicBreadcrumbs'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Label } from '@/components/ui/label'
import GeneralLayout from '@/layouts'
import { CircleX, Download, Eye, Heart, LinkedinIcon, Ratio, Star, ViewIcon } from 'lucide-react'
import React, { useEffect, useState } from 'react'
import DOMPurify from 'dompurify';
import { Link, useParams } from 'react-router-dom'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Contributions } from '@/services/client'
import { formatDate, roundToDecimal } from '@/utils/helper'
import Spinner from '@/components/Spinner'
import DownloadAllButton from '@/components/DownloadAllButton'
import { useQueryContributionDetail } from '@/query/useQueryContributionDetail'
import { useLikedContribution } from '@/query/useLikedContribution'
import Comment from '@/components/Comment'
import * as yup from "yup"
import { yupResolver } from "@hookform/resolvers/yup"
import { useForm } from 'react-hook-form'
import { toast } from 'react-toastify'
import { useLikeMutation } from '@/query/useLikeMutation'
import { useAppContext } from '@/hooks/useAppContext'
import { Roles } from '@/constant/roles'
import { Tooltip, TooltipContent, TooltipProvider, TooltipTrigger } from '@/components/ui/tooltip'
import Ratings from '@/components/Ratings'



export default function ContributionDetail() {
  const [openOptions, setOpenOptions] = useState(true)
  const schema = yup
    .object({
      comment: yup.string().required("Please provide comment"),
    })
    .required()

  const { register, handleSubmit, formState: { errors }, setError, reset } = useForm({
    resolver: yupResolver(schema)
  })

  const commentMutation = useMutation({
    mutationFn: (data) => Contributions.commentPublic(data)
  })

  const queryClient = useQueryClient()
  const { id } = useParams()
  const { profile } = useAppContext()
  const { data, isLoading } = useQueryContributionDetail(id)
  const detailData = data && data?.data?.responseData
  const likeMutation = useLikeMutation(detailData?.id)
  const cleanHTML = DOMPurify.sanitize(detailData?.content);
  const handleDownloadFile = (e, file) => {
    e.stopPropagation()
    e.preventDefault()
    fetch(file?.path)
      .then(response => response.blob())
      .then(blob => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = file.name;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
      })
      .catch(error => {
        console.error('Error downloading file:', error);
      });
  };

  const isFavorite = useLikedContribution(detailData?.id)
  const onSubmit = ({ comment }) => {
    commentMutation.mutate({ id: detailData.id, body: { content: comment } }, {
      onSuccess() {
        toast.success("Comment Post Successfully!")
        reset()
        queryClient.invalidateQueries('preview')
      },
      onError() {
        toast.error("Error when posting comment")
      }
    })
  }
  const handleToggleLike = (e, id) => {
    e.stopPropagation()
    likeMutation.mutate(id, {
      onSuccess() {
        toast.success("Toggle Like Successfully!")
      },
      onError(data) {
        const errorMessage = data && data?.response?.data?.title
        toast.error(errorMessage)
      },
      onSettled() {
        queryClient.invalidateQueries(['favorite-list']);
        queryClient.invalidateQueries(['detail-contributions']);
      },
    })
  }
  return (

    <GeneralLayout>
      {isLoading && <div className="container flex items-center justify-center min-h-screen"><Spinner className={"border-blue-500"}></Spinner></div>}
      {!isLoading && <div className='container py-10'>

        <DynamicBreadcrumb></DynamicBreadcrumb>
        {/* Top post */}
        <div className='flex flex-col items-center gap-6 my-5 medium:flex-row'>
          <img src={`${detailData?.thumbnails?.length > 0 && detailData?.thumbnails[0]?.path || 'https://plus.unsplash.com/premium_photo-1686149758342-9f0f249f2989?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxlZGl0b3JpYWwtZmVlZHwyfHx8ZW58MHx8fHx8'}`} alt="" className='rounded-lg w-full xl:h-[500px] medium:w-[60%] xl:w-auto object-cover flex-shrink-0 ' />
          <div className="w-full md:flex-1">
            <div className="flex items-end font-bold justify-center text-white bg-yellow-500 rounded-sm w-[150px] py-3 px-2 ml-auto my-4 gap-2">{roundToDecimal(detailData?.averageRating, 1)} <Star width={20}></Star></div>
            <div className="flex items-center justify-between">
              <Badge variant="destructive">{detailData?.facultyName}</Badge>
              {profile?.roles !== Roles?.Guest && <Button className={`bg-transparent hover:bg-red-500 hover:text-white hover:border-white  ${isFavorite ? "bg-red-500 text-white" : "bg-white text-black"} `} onClick={(e) => handleToggleLike(e, detailData)}><Heart></Heart></Button>}
            </div>
            <h2 className="mt-3 text-2xl font-semibold text-ellipsis line-clamp-4 medium:text-4xl">{detailData?.title} </h2>
            <div className="flex flex-wrap items-center justify-between my-6 text-xs font-semibold text-gray-600 md:text-sm medium:text-base">
              <div className='flex flex-wrap items-center gap-1'>
                <p>{formatDate(detailData?.publicDate)}</p>
                <div className='w-1 h-1 bg-gray-600 rounded-full md:w-2 md:h-2'></div>
                <div>{detailData?.userName}</div>
              </div>
              <div className='flex flex-wrap items-center gap-3'>
                <div className='flex items-center gap-1'>
                  <Eye className='w-4 h-4 md:w-5 md:h-5'></Eye>
                  <p>{detailData?.view}</p>
                </div>
                <div className='flex items-center gap-1'>
                  <Heart className='w-4 h-4 md:w-5 md:h-5'></Heart>
                  <p>{detailData?.like}</p>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div dangerouslySetInnerHTML={{ __html: cleanHTML }} className='richtext-css' />
        <div className='my-5'>
          <div className="flex items-center justify-between my-4">
            <h2 className='font-bold'>File attached</h2>
            <DownloadAllButton files={detailData?.files}></DownloadAllButton>
          </div>
          <div className="grid-cols-2 gap-6 p-10 rounded-lg shadow-lg h-[250px] overflow-y-scroll md:overflow-auto grid md:h-auto md:grid-cols-5">
            {detailData?.files?.map((file, index) => (
              <TooltipProvider key={file}>
                <Tooltip>
                  <TooltipTrigger>
                    <Link
                      to={`/view-file?path=${file?.path}&ext=${file?.extension?.split('.')[1]}`}
                      target='_blank'
                      className="z-10 flex flex-col items-center justify-center p-4 rounded-lg cursor-pointer hover:bg-slate-100"
                      key={index}
                    >
                      <img src={file?.extension === ".doc" || file.extension == ".docx" ? "../word.png" : "../pdf.png"} alt="" className="object-cover w-14 h-14 lg:h-24 lg:w-24 " />
                      <div className="text-center">{file?.name}</div>
                      <div className="flex items-center justify-center gap-2">
                        <div className="flex items-center justify-center w-10 h-10 mt-2 text-white bg-blue-500 rounded-lg">
                          <Download onClick={(e) => handleDownloadFile(e, file)}></Download>
                        </div>
                      </div>
                    </Link></TooltipTrigger>
                  <TooltipContent>
                    <p>View Directly</p>
                  </TooltipContent>
                </Tooltip>
              </TooltipProvider>
            ))}
          </div>
          <div className={`md:min-w-[300px] fixed right-0 top-1/2 -translate-y-1/2 ${openOptions ? "translate-x-0" : "translate-x-full"} bg-black text-white shadow-lg min-h-[100px] rounded-lg flex flex-col items-center justify-center z-30 transition-all duration-300 p-5`}>
            <div className="absolute inline-block cursor-pointer left-5 top-2" onClick={() => { setOpenOptions(false) }} >
              <CircleX />
            </div>
            <Ratings id={detailData?.id} currentRating={detailData?.myRating}></Ratings>
          </div>
          {!openOptions && <div className="fixed right-0 -translate-y-1/2 top-1/2 w-[50px]  shadow-lg h-[50px] rounded-lg flex  flex-col items-center justify-center z-30 cursor-pointer bg-black text-white transition-all duration-300" onClick={() => setOpenOptions(true)}>
            <Star height={30} width={30} />
          </div>}
          <div className='mt-5'>
            <h2 className='font-bold'>Comments</h2>
            <form className='flex items-center gap-2 p-5 border rounded-lg' onSubmit={handleSubmit(onSubmit)}>
              <input type="text" placeholder='Write something here...' className={`flex-1 flex-shrink-0 font-semibold text-black outline-none ${commentMutation.isLoading ? "disabled bg-opacity-70" : ""}`} {...register('comment')} />
              <Button className={`${commentMutation.isLoading ? "disabled pointer-events-none" : ""} w-[150px] bg-blue-600`} type="submit">Post</Button>
            </form>
            <div className="my-3 font-semibold text-red-500">{errors && errors?.comment?.message}</div>
            {detailData?.comments?.length > 0 && <div className='mt-4'>
              {detailData?.comments.map((item, index) => (
                <Comment key={index} comment={item}></Comment>
              ))}
            </div>}
          </div>
        </div>
      </div>}

    </GeneralLayout>
  )
}
