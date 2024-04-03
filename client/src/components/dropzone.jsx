import { ArrowDown, Download } from "lucide-react";
import { useCallback, useEffect, useState } from "react";
import { useDropzone } from "react-dropzone";
import Swal from 'sweetalert2'
function Dropzone({ open, className, files, setFiles }) {

  const onDrop = useCallback(acceptedFiles => {

    // Do something with the files
    setFiles((prev) => [...prev, ...acceptedFiles])
  }, [setFiles])

  const handleFileDelete = (index) => {

    const updatedFiles = [...files];
    updatedFiles.splice(index, 1);
    setFiles(updatedFiles);
  }



  const { getRootProps, getInputProps, isDragActive } = useDropzone({
    onDrop,
  })

  const handleRenderImage = (filePath) => {
    if (filePath === "docx" || filePath === "doc") {
      return "../../word.png"
    } else if (filePath === "pdf") {
      return "../pdf.png"
    }
  }

  const handleDeleteFile = (e, index) => {
    e.stopPropagation()
    Swal.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!"
    }).then((result) => {
      if (result.isConfirmed) {
        handleFileDelete(index)
      }
    });
  }


  const handleDownloadFile = (e, file) => {
    e.stopPropagation();
    fetch(file.path)
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

  return (
    <div {...getRootProps({ className: `w-full rounded-lg border border-dashed h-[300px] overflow-y-scroll cursor-pointer ${className}` })}>
      <input {...getInputProps()} className="w-full bg-black min-h-[400px]" />
      <div className="flex flex-col items-center ">
        {
          isDragActive ?
            <div className="flex flex-col items-center justify-center w-full h-full gap-2">
              <div className="text-blue-600">
                <ArrowDown className="transition-all translate-x-3 repeat-infinite animate-down overflow" width={40} height={40}></ArrowDown>
              </div>
              <div className="text-2xl text-blue-600">Drop you files in here </div>
            </div> :
            !files.length && <p className="w-full py-4 text-xl font-semibold text-center lg:text-2xl">Drag 'n' drop some files here, or click to select files</p>
        }
      </div>
      <div className="grid grid-cols-1 gap-6 p-10 md:grid-cols-5">
        {files && files.length > 0 && files.map((item, index) => (
          <div className="z-50 flex flex-col items-center justify-center p-4 rounded-lg hover:bg-slate-100" key={index}>
            <img src={handleRenderImage(item.path.split('.')[1])} alt="" className="object-cover w-14 h-14 lg:h-24 lg:w-24 " />
            <div className="text-center">{item.name}</div>
            <div className="flex items-center justify-center gap-2">
              <div className="flex items-center justify-center w-10 h-10 text-white bg-blue-500 rounded-lg">
                <Download onClick={(e) => handleDownloadFile(e, item)}></Download>
              </div>
              <div
                onClick={(e) => handleDeleteFile(e, index)}
                className="z-50 flex items-center justify-center w-10 h-10 text-lg font-bold text-white bg-red-500 rounded-lg cursor-pointer">
                X
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}

export default Dropzone;