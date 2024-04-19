import useParamsVariables from '@/hooks/useParams';
import React from 'react';

import '@react-pdf-viewer/core/lib/styles/index.css';
export default function ViewFile() {
  const { path, ext } = useParamsVariables()
  console.log(path, ext)
  if (!path || !ext) return <div className="flex items-center justify-center w-full min-h-screen text-lg font-bold text-center">Error when loading Data</div>
  return (
    //   <FileViewer
    //   fileType={ext}
    //   filePath={path}
    // />

    <div className="w-full h-screen">
      {ext === "pdf" ? <embed type="application/pdf" src={'https://docs.google.com/viewer?url=' + path + '&embedded=true'} width="100%" height="100%"></embed> : <embed src={`https://docs.google.com/viewer?url=${path}&embedded=true`} width="100%" height="100%" frameBorder="0"></embed>
      }
    </div>

  )
}
