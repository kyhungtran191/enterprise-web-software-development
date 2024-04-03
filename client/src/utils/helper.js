export function formatDate(inputDate) {
  const months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];

  const date = new Date(inputDate);

  const day = date.getDate();
  const monthIndex = date.getMonth();
  const year = date.getFullYear();

  const monthName = months[monthIndex];

  const result = `${day} ${monthName} ${year}`;

  return result;
}

export async function convertFilesToBlob(filesArray) {
  const filesPromises = filesArray.map(async fileObj => {
    try {
      const response = await fetch(fileObj.path);
      const blob = await response.blob();
      return new File([blob], fileObj.name, { type: blob.type });
    } catch (error) {
      console.error(`Failed to create File for ${fileObj.name}:`, error);
      return null;
    }
  });

  return Promise.all(filesPromises);
}