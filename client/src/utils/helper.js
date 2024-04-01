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