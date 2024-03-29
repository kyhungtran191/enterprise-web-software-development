import DynamicBreadcrumb from '@/components/DynamicBreadcrumbs'
import { Breadcrumb } from '@/components/ui/breadcrumb'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import AdminLayout from '@/layouts/AdminLayout'
import React, { useEffect, useRef, useState } from 'react'
import { Editor } from '@tinymce/tinymce-react';
import Dropzone from '@/components/dropzone'
import { useDropzone } from 'react-dropzone'
import { Button } from '@/components/ui/button'
export default function AddContribution() {
  const [files, setFiles] = useState([])

  const [currentFileThumbNail, setCurrentFileThumbNail] = useState({})
  const [currentThumbnail, setCurrentThumbnail] = useState()
  console.log(files)

  const handleChangeImage = (e) => {
    const file = e.target.files[0]
    if (file) {
      let url = URL.createObjectURL(file)
      setCurrentThumbnail(url)
      setCurrentFileThumbNail(file)
    }
  }
  const editorRef = useRef(null);




  return (
    <AdminLayout>
      <div>
        <DynamicBreadcrumb></DynamicBreadcrumb>
        <form className='py-4'>
          <div className='col-span-2 mt-2 max-w-[800px]'>
            <Label className="text-2xl font-semibold">Title</Label>
            <Input className="p-4 my-2 text-lg font-semibold border border-gray-300" placeholder="Your Article Title..."></Input>
          </div>
          <div className='col-span-2 my-4'>
            <label htmlFor="thumbnail" className='h-[400px] p-4  block w-full border border-dashed cursor-pointer bg-slate-100/40'>
              <input className="hidden" type="file" id="thumbnail" onChange={handleChangeImage} />
              {currentThumbnail ?
                <img src={currentThumbnail ? currentThumbnail : " "} alt="" className='object-cover w-full h-full' />
                : <div className='flex flex-col items-center justify-center h-full gap-4'>
                  <img src={"../upload-icon.png"} alt="" className='w-20 h-20' />
                  <p className='text-xl font-semibold text-center lg:text-3xl'>Add your article Thumbnail here</p>
                </div>}
            </label>
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
          </div>
          <div className='py-4'>
            <label className="text-2xl font-semibold">Attach Files</label>
            <Dropzone files={files} setFiles={setFiles}></Dropzone>
          </div>
        </form>
        <Button className="w-full text-lg bg-green-600 py-7 hover:bg-green-700">Upload</Button>
      </div>
    </AdminLayout>
  )
}
