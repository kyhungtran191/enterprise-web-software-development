import DynamicBreadcrumb from '@/components/DynamicBreadcrumbs'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import AdminLayout from '@/layouts/AdminLayout'
import React, { useEffect, useRef, useState } from 'react'
import { Editor } from '@tinymce/tinymce-react';
import Dropzone from '@/components/dropzone'
import { Button } from '@/components/ui/button'
import * as yup from 'yup'
import { yupResolver } from '@hookform/resolvers/yup'
import { Controller, useForm } from 'react-hook-form'
import ActionSpinner from '@/components/ActionSpinner'
import { STUDENT_OPTIONS } from '@/constant/menuSidebar'
import { QueryClient, useMutation } from '@tanstack/react-query'
import { Contributions } from '@/services/client'
import { toast } from 'react-toastify'
import { useNavigate, useParams } from 'react-router-dom'
import { useContributionsEditDetail } from '@/query/useContributionInfo'
import { convertFilesToBlob } from '@/utils/helper'

const schema = yup.object({
  title: yup.string().required('Please provide post title'),
  short_description: yup.string().required('Please provide short description'),
});

export default function UpdateContribution() {
  const {
    register,
    handleSubmit,
    setError,
    clearErrors,
    setValue,
    watch,
    control,
    formState: { errors },
  } = useForm({ resolver: yupResolver(schema) });

  const [files, setFiles] = useState([])
  const [currentThumbnail, setCurrentThumbnail] = useState()
  const [thumbnailError, setThumbnailError] = useState(false);
  const [detailError, setDetailError] = useState(false);
  const [fileError, setFileError] = useState(false);
  const navigate = useNavigate()
  const { slug } = useParams();

  const { data } = useContributionsEditDetail(slug)
  let dataDetail = data && data?.data?.responseData
  const updateContributionMutation = useMutation({
    mutationFn: (body) => Contributions.updateContribution(body)
  })
  const handleChangeImage = (e) => {
    const file = e.target.files[0]
    if (file) {
      let url = URL.createObjectURL(file)
      setCurrentThumbnail(url)
      setThumbnailError(false);
    }
  }
  const editorRef = useRef(null);
  const onSubmit = async (data) => {
    if (!currentThumbnail) {
      setThumbnailError(true);
    }
    if (editorRef.current.getContent() == '') {
      setDetailError(true)
    }
    else {
      setDetailError(false)
    }
    if (files.length === 0) {
      setFileError(true);
    }
    if (thumbnailError || detailError || files.length < 0 || !watch("terms")) return;
    let thumbnailFile;
    if (typeof currentThumbnail === 'string') {
      try {
        const response = await fetch(currentThumbnail);
        const blob = await response.blob();
        thumbnailFile = new File([blob], 'thumbnail.jpg', { type: 'image/jpeg' });
      } catch (error) {
        console.error('Failed to create thumbnail file:', error);
      }
    } else if (currentThumbnail instanceof Blob) {
      thumbnailFile = new File([currentThumbnail], 'thumbnail.jpg', { type: 'image/jpeg' });
    } else {
      console.error('Invalid thumbnail type');
    }
    let formData = new FormData();
    formData.append("ContributionId", dataDetail.id)
    formData.append('Title', data.title);
    formData.append('IsConfirmed', watch("terms"));
    formData.append('Content', editorRef.current.getContent());
    formData.append('ShortDescription', data.short_description);
    if (thumbnailFile) {
      formData.append('Thumbnail', thumbnailFile);
    }
    const filesAsBlob = await convertFilesToBlob(files);
    for (let i = 0; i < filesAsBlob?.length; i++) {
      formData.append('Files', filesAsBlob[i]);
    }
    for (const entry of formData.entries()) {
      console.log(entry[0], entry[1]);
    }
    updateContributionMutation.mutate(formData, {
      onSuccess(data) {
        toast.success("Update successfully!")
        navigate('/manage/recent?status=PENDING')
      },
      onError(data) {
        toast.error(data && data?.messages[0])
      }
    })
  }

  useEffect(() => {
    setValue('title', dataDetail?.title);
    setValue('short_description', dataDetail?.shortDescription);
    setFiles(dataDetail?.files)
    if (dataDetail?.thumbnails && dataDetail?.thumbnails?.length > 0) {
      setCurrentThumbnail(dataDetail?.thumbnails[0]?.path);
    }
  }, [dataDetail, setValue])
  return (
    <AdminLayout link={STUDENT_OPTIONS}>
      <div>
        <DynamicBreadcrumb></DynamicBreadcrumb>
        {updateContributionMutation.isLoading && <ActionSpinner></ActionSpinner>
        }
        <form className='py-4' onSubmit={handleSubmit(onSubmit)}>
          <div className='mt-2'>
            <Label className="text-2xl font-semibold">Title</Label>
            <Controller
              control={control}
              name="title"
              render={({ field }) => (
                <Input
                  className='p-4 mt-2 text-lg font-bold outline-none '
                  placeholder='Title'
                  {...field}
                ></Input>
              )}
            />
            {errors.title && <p className='font-semibold text-red-500'>{errors.title.message}</p>}
          </div>
          <div className='h-full col-span-1 mt-2'>
            <Label className="text-2xl font-semibold">Short Description</Label>
            <textarea className="block p-4 h-[300px] md:w-full my-2 text-lg font-semibold border border-gray-300" placeholder="Short Description" name='short_description' {...register('short_description')}></textarea>
            {errors.short_description && <p className='font-semibold text-red-500'>{errors.short_description.message}</p>}
          </div>
          <div className='col-span-2 my-4'>
            <label htmlFor="thumbnail" className='h-[400px] p-4  block w-full border border-dashed cursor-pointer bg-slate-100/40'>
              <input className="hidden" type="file" id="thumbnail" onChange={handleChangeImage} />
              {currentThumbnail ?
                <img src={currentThumbnail ? currentThumbnail : " "} alt="" className='object-cover w-full h-full' />
                : <div className='flex flex-col items-center justify-center h-full gap-4'>
                  <img src={"../../upload-icon.png"} alt="" className='w-20 h-20' />
                  <p className='text-xl font-semibold text-center lg:text-3xl'>Add your article Thumbnail here</p>
                </div>}
            </label>
            {thumbnailError && !currentThumbnail && <p className='font-semibold text-red-500'>Please provide thumbnail image</p>}
          </div>
          <div className='py-4'>
            <label className="text-2xl font-semibold">Write your content here </label>
            <Editor
              onInit={(evt, editor) => editorRef.current = editor}
              apiKey='574fp9fplywzbz45xdnqnmxdfpflkpljtav7czrto0idn25z'
              initialValue={dataDetail?.content}
              init={{
                height: 400,
                selector: 'textarea',
                menubar: false,
                resize: 'both',
                plugins: [
                  'a11ychecker', 'advlist', 'advcode', 'advtable', 'autolink', 'checklist', 'export',
                  'lists', 'link', 'image', 'charmap', 'preview', 'anchor', 'searchreplace', 'visualblocks',
                  'powerpaste', 'fullscreen', 'formatpainter', 'insertdatetime', 'media', 'table', 'help', 'wordcount'
                ],
                toolbar: 'undo redo | casechange blocks | bold italic backcolor | ' +
                  'alignleft aligncenter alignright alignjustify | ' +
                  'bullist numlist checklist outdent indent | removeformat | a11ycheck code table help',
                content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:16px }',
              }}
            />
            {detailError && !editorRef?.current?.getContent() && <p className='font-semibold text-red-500'>Please provide content</p>}
          </div>
          <div className='py-4'>
            <div className="flex items-center gap-2">
              <label className="text-2xl font-semibold">Attach Files</label>
            </div>
            <Dropzone files={files} setFiles={setFiles}></Dropzone>
            {!files?.length && <p className='font-semibold text-red-500'>Please provide related Files</p>}
          </div>
          <div className="flex items-center w-full space-x-2">
            <input type="checkbox" defaultChecked={false} {...register('terms')} />
            <Label htmlFor="terms" className="text-lg font-bold">Accept terms and conditions</Label>
            {!watch("terms") && <p className='font-semibold text-red-500'>Please Confirm Terms & Conditions before submit</p>}
          </div>
          <Button type="submit" className="w-full mt-5 text-lg bg-green-600 py-7 hover:bg-green-700">Update</Button>
        </form>
      </div>
    </AdminLayout>
  )
}