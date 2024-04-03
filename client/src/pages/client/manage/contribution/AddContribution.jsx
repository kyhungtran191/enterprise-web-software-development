import DynamicBreadcrumb from '@/components/DynamicBreadcrumbs'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import AdminLayout from '@/layouts/AdminLayout'
import React, { useRef, useState } from 'react'
import { Editor } from '@tinymce/tinymce-react';
import Dropzone from '@/components/dropzone'
import { Button } from '@/components/ui/button'
import * as yup from 'yup'
import { EMAIL_REG } from '@/utils/regex'
import { yupResolver } from '@hookform/resolvers/yup'
import { Controller, useForm } from 'react-hook-form'
import { file } from 'jszip'
import Spinner from '@/components/Spinner'
import ActionSpinner from '@/components/ActionSpinner'

const schema = yup.object({
  title: yup.string().required('Please provide post title'),
  short_description: yup.string().required('Please provide short description'),
});

export default function AddContribution() {
  const {
    register,
    handleSubmit,
    setError,
    clearErrors,
    watch,
    control,
    formState: { errors },
  } = useForm({ resolver: yupResolver(schema) });

  const [files, setFiles] = useState([])
  const [currentThumbnail, setCurrentThumbnail] = useState()
  const [thumbnailError, setThumbnailError] = useState(false);
  const [detailError, setDetailError] = useState(false);
  const [fileError, setFileError] = useState(false);
  const fetchThumbNailFile = (blobUrl) => {

  }
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
    if (thumbnailError || detailError || files.length < 0) return;
    // HandleFile
    let file = await fetch(currentThumbnail)
    let thumbNailFile = file ? new File([file], 'thumbnail.jpg', { type: 'image/jpeg' }) : undefined;

    let info = {
      Title: data.title,
      Thumbnail: thumbNailFile,
      Files: files,
      IsConfirmed: watch("terms"),
      Content: editorRef.current.getContent(),
      ShortDescription: data.short_description
    }
    console.log(info)
  }

  const isLoading = true;
  return (
    <AdminLayout isAdmin={false}>
      <div>
        <DynamicBreadcrumb></DynamicBreadcrumb>
        {isLoading && <ActionSpinner></ActionSpinner>
        }
        <form className='py-4' onSubmit={handleSubmit(onSubmit)}>
          <div className='mt-2'>
            <Label className="text-2xl font-semibold">Title</Label>
            <Controller
              control={control}
              name="title"
              render={({ field }) => (
                <Input
                  className='p-4 mt-2 outline-none'
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
              initialValue=''
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
            {!files.length && <p className='font-semibold text-red-500'>Please provide related Files</p>}
          </div>
          <div className="flex items-center w-full space-x-2">
            <input type="checkbox" defaultChecked={false} {...register('terms')} />
            <Label htmlFor="terms" className="text-lg font-bold">Accept terms and conditions</Label>
            {!watch("terms") && <p className='font-semibold text-red-500'>Please Confirm Terms & Conditions before submit</p>}
          </div>
          <Button type="submit" className="w-full mt-5 text-lg bg-green-600 py-7 hover:bg-green-700">Upload</Button>
        </form>
      </div>
    </AdminLayout>
  )
}