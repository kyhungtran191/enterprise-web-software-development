import DynamicBreadcrumb from '@/components/DynamicBreadcrumbs'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import React, { useState } from 'react'
import DOMPurify from 'dompurify';
import { useParams } from 'react-router-dom'
import { formatDate } from '@/utils/helper'
import DownloadAllButton from '@/components/DownloadAllButton'
import AdminLayout from '@/layouts/AdminLayout'
import { useAppContext } from '@/hooks/useAppContext'
import { MC_OPTIONS, STUDENT_OPTIONS } from '@/constant/menuSidebar'
import { usePreviewContribution } from '@/query/usePreviewContribution'
import Comment from '@/components/Comment'
import { Ratio, CircleX } from 'lucide-react';
import Swal from 'sweetalert2'
import { useMutateApprove } from '@/query/useMutateApprove'
import { toast } from 'react-toastify'
import { Link, useNavigate } from 'react-router-dom'
import ActionSpinner from '@/components/ActionSpinner'
import * as yup from "yup"
import { yupResolver } from "@hookform/resolvers/yup"
import { useForm } from 'react-hook-form'
import { Contributions } from '@/services/coodinator'
import { useMutation, useQueryClient } from "@tanstack/react-query"
import { Roles } from '@/constant/roles'
import CustomRejectComponent from '@/components/CustomRejectComponent'

export default function PreviewContribution() {
  const [openOptions, setOpenOptions] = useState(true)
  const { slug } = useParams()
  // React Query
  const { data, isLoading } = usePreviewContribution(slug)
  const { mutate, isLoading: isLoadingApprove } = useMutateApprove()

  const commentMutation = useMutation({
    mutationFn: (data) => Contributions.MCComment(data)
  })

  const queryClient = useQueryClient()

  const detailData = data && data?.data?.responseData
  console.log(detailData)
  const cleanHTML = DOMPurify.sanitize(detailData?.content);
  const { profile } = useAppContext()

  const schema = yup
    .object({
      comment: yup.string().required("Please provide comment"),
    })
    .required()

  const { register, handleSubmit, formState: { errors }, setError, reset } = useForm({
    resolver: yupResolver(schema)
  })



  const handleDownloadFile = (file) => {
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


  const navigate = useNavigate();
  const handleApproveArticle = (e) => {
    const ids = [];
    ids.push(detailData && detailData?.id)
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
        mutate({ ids }, {
          onSuccess() {
            toast.success("Approve Contribution successfully !")
            navigate("/coodinator-manage/contributions?status=APPROVE")
          }
        })
      }
    });
  }
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
  let links = profile && profile?.roles == "Student" ? STUDENT_OPTIONS : MC_OPTIONS
  return (
    <AdminLayout links={links}>
      {isLoading || isLoadingApprove && <ActionSpinner></ActionSpinner>}
      {!isLoading && <div className='container py-10'>
        <DynamicBreadcrumb></DynamicBreadcrumb>
        <div className="w-full p-2 mt-5 font-bold text-center text-white bg-yellow-400 medium:text-lg medium:py-4">Note! This is just preview version of the pending or reject contribution</div>
        {profile.roles == Roles.Coordinator && detailData.status === "PENDING" && <>
          <div className={`h-[100px] fixed right-0 top-1/2 -translate-y-1/2 ${openOptions ? "translate-x-0" : "translate-x-full"} bg-black text-white shadow-lg min-h-[100px] rounded-lg flex flex-col items-center justify-center z-30 transition-all duration-300 p-5`}>
            <div className="absolute inline-block cursor-pointer left-5 top-2" onClick={() => { setOpenOptions(false) }} >
              <CircleX />
            </div>
            <h2 className='my-2 font-bold'>Options</h2>
            <div className="grid grid-cols-2 gap-5 ">
              <Button className="w-full text-white bg-green-600 hover:bg-green-500" onClick={handleApproveArticle}>Approve</Button>
              <CustomRejectComponent id={detailData.id}></CustomRejectComponent>
            </div>
          </div>
          {!openOptions && <div className="fixed right-0 -translate-y-1/2 top-1/2 w-[50px]  shadow-lg h-[50px] rounded-lg flex  flex-col items-center justify-center z-30 cursor-pointer bg-black text-white transition-all duration-300" onClick={() => setOpenOptions(true)}>
            <Ratio height={30} width={30} />
          </div>}
        </>}
        {/* Top post */}
        <div className='flex flex-col items-center gap-6 my-5 medium:flex-row'>
          <img src={`${detailData?.thumbnails?.length > 0 && detailData?.thumbnails[0]?.path || 'https://plus.unsplash.com/premium_photo-1686149758342-9f0f249f2989?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxlZGl0b3JpYWwtZmVlZHwyfHx8ZW58MHx8fHx8'}`} alt="" className='rounded-lg w-full xl:h-[500px] medium:w-[60%] xl:w-auto object-cover flex-shrink-0' />
          <div className="w-full medium:flex-1">
            <div className="flex items-center justify-between">
              <Badge variant="destructive">{detailData?.facultyName}</Badge>
            </div>
            <h2 className="mt-3 text-2xl font-semibold text-ellipsis line-clamp-4 medium:text-4xl">{detailData?.title} </h2>
            <div className="flex flex-wrap items-center justify-between my-6 text-xs font-semibold text-gray-600 md:text-sm medium:text-base">
              <div className='flex flex-wrap items-center gap-1'>
                <p>{formatDate(detailData?.publicDate)}</p>
                <div className='w-1 h-1 bg-gray-600 rounded-full md:w-2 md:h-2'></div>
                <div>{detailData?.userName}</div>
              </div>
              <Button><Link to={`/student-manage/edit-contribution/${detailData.slug}`} className='w-full'>Edit now</Link></Button>
            </div>
            <div>{detailData?.shortDescription}</div>
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
              <div className="z-10 flex flex-col items-center justify-center p-4 rounded-lg cursor-pointer hover:bg-slate-100" key={index} onClick={() => handleDownloadFile(file)}>
                <img src={file?.extension == ".docx" || "doc" ? "../word.png" : "../pdf.png"} alt="" className="object-cover w-14 h-14 lg:h-24 lg:w-24 " />
                <div className="text-center">{file?.name}</div>
                <div className="flex items-center justify-center gap-2">
                </div>
              </div>
            ))}

          </div>
        </div>
        <div>
          <h2 className='font-bold'>Comments</h2>
          <form className='flex items-center gap-2 p-5 border rounded-lg' onSubmit={handleSubmit(onSubmit)}>
            <input type="text" placeholder='Write something here...' className={`flex-1 flex-shrink-0 font-semibold text-black outline-none ${commentMutation.isLoading ? "disabled bg-opacity-70" : ""}`} {...register('comment')} />
            <Button className={`${commentMutation.isLoading ? "disabled pointer-events-none" : ""} w-[150px] bg-blue-600`} type="submit">Post</Button>
          </form>
          <div className="my-3 font-semibold text-red-500">{errors && errors?.comment?.message}</div>
          <div className='mt-4'>
            {detailData?.comments?.map((item, index) => (
              <Comment key={index} comment={item}></Comment>
            ))}
          </div>
        </div>
      </div>}
    </AdminLayout>
  )
}
