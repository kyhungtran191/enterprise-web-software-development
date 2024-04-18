import React from 'react'
import { Button } from './ui/button'
import JSZip from 'jszip';

export default function DownloadAllButton({ files }) {
  const handleDownloadAll = async () => {
    const zip = new JSZip();
    await Promise.all(files.map(async (file) => {
      const response = await fetch(file.path);
      const blob = await response.blob();
      zip.file(file.name, blob);
    }));

    zip.generateAsync({ type: 'blob' }).then((content) => {
      const zipBlob = new Blob([content]);
      const zipUrl = window.URL.createObjectURL(zipBlob);
      const link = document.createElement('a');
      link.href = zipUrl;
      link.setAttribute('download', 'all_files.zip');

      link.click();
      window.URL.revokeObjectURL(zipUrl);

    });
  };


  return (
    <Button className="bg-blue-500" onClick={handleDownloadAll}>Download all</Button>
  )
}
