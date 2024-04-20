import Dashboard from '@/components/Dashboard'
import { STUDENT_OPTIONS } from '@/constant/menuSidebar'
import AdminLayout from '@/layouts/AdminLayout'
import React, { useEffect, useState } from 'react'
import { Bar, Doughnut } from 'react-chartjs-2'
import instanceAxios from '@/utils/axiosInstance'
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { Separator } from '@/components/ui/separator'
import StudentLineChart from './StudentLineChart'
import ExcelExport from '@/components/ExcelExport'

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend)
export default function StudentDashboard() {

  const [currentYear, setCurrentYear] = useState('2024-2025');
  const [chartData, setChartData] = useState(null);
  const [allData, setAllData] = useState(null);
  const [currentData, setCurrentData] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await instanceAxios.get('http://localhost:5272/api/admin/Contributions/report-total-contributions-following-status-for-each-academic-years');
        setAllData(response?.data?.response);
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };
    fetchData();
  }, [currentYear]);

  useEffect(() => {
    if (currentYear !== '' && allData) {
      const dataForYear = allData.find((item) => item.academicYear === currentYear);
      setCurrentData(dataForYear)
      const chartData = processChartData(dataForYear.dataSets);
      setChartData(chartData);

    }
  }, [currentYear, allData]);

  const processChartData = (apiData) => {
    const labels = apiData.map((item) => item.status);
    const dataValues = apiData.map((item) => item.data);
    const backgroundColors = apiData.map((item) => {
      switch (item.status) {
        case 'Pending':
          return 'yellow';
        case 'Reject':
          return 'red';
        case 'Approve':
          return 'green';

      }
    });
    return {
      labels: labels,
      datasets: [
        {
          label: currentYear,
          data: dataValues,
          backgroundColor: backgroundColors
        }
      ]
    };
  };

  console.log(allData)
  return (
    <AdminLayout links={STUDENT_OPTIONS}>
      <div className="px-3 py-6">
        <ExcelExport data={allData}></ExcelExport>
        <Select onValueChange={setCurrentYear} className="" defaultValue={currentYear} >
          <SelectTrigger className='w-[200px] py-4 text-lg font-bold my-4'>
            <SelectValue placeholder='Academic Year' />
          </SelectTrigger>
          <SelectContent >
            {allData &&
              allData.map((item) => (
                <SelectItem key={item?.academicYear} value={item?.academicYear}>{item?.academicYear}</SelectItem>
              ))}
          </SelectContent>
        </Select>
        <div className="grid w-full gap-10 medium:grid-cols-2">
          {currentData && <div className="p-4 rounded-lg shadow-xl drop-shadow-lg">
            <div className="flex flex-wrap items-center justify-center gap-5 mt-2 rounded-full sm:justify-start">
              <h2 className='font-bold text-blue-500'>Average Rating of {currentYear}</h2>
              <div className="relative flex flex-col items-center justify-center flex-shrink-0 text-blue-500 h-52 w-52">
                <span className='text-3xl font-bold'>{currentData?.averageRating}</span>
                <span>of</span>
                <span className='font-bold'> {currentData?.totalContributionApproved} Contributions</span>
                <div className="absolute inset-0 border-8 border-blue-500 rounded-full" />
              </div>
            </div>

            <div className='grid w-full gap-2 my-5 sm:grid-cols-3'>
              <div className='flex items-center justify-around px-2 py-5 text-center text-black rounded-lg bg-yellow-400/80'>
                <div className='flex flex-col'>
                  <span className='text-2xl font-bold'>{currentData.totalLike}</span>
                  <span className='text-sm font-medium'>Total Likes</span>
                </div>
                <div className='flex items-center justify-center w-10 h-10 text-white rounded-full shadow-2xl drop-shadow-2xl bg-yellow-600/60'>
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-6 h-6">
                    <path strokeLinecap="round" strokeLinejoin="round" d="M6.633 10.25c.806 0 1.533-.446 2.031-1.08a9.041 9.041 0 0 1 2.861-2.4c.723-.384 1.35-.956 1.653-1.715a4.498 4.498 0 0 0 .322-1.672V2.75a.75.75 0 0 1 .75-.75 2.25 2.25 0 0 1 2.25 2.25c0 1.152-.26 2.243-.723 3.218-.266.558.107 1.282.725 1.282m0 0h3.126c1.026 0 1.945.694 2.054 1.715.045.422.068.85.068 1.285a11.95 11.95 0 0 1-2.649 7.521c-.388.482-.987.729-1.605.729H13.48c-.483 0-.964-.078-1.423-.23l-3.114-1.04a4.501 4.501 0 0 0-1.423-.23H5.904m10.598-9.75H14.25M5.904 18.5c.083.205.173.405.27.602.197.4-.078.898-.523.898h-.908c-.889 0-1.713-.518-1.972-1.368a12 12 0 0 1-.521-3.507c0-1.553.295-3.036.831-4.398C3.387 9.953 4.167 9.5 5 9.5h1.053c.472 0 .745.556.5.96a8.958 8.958 0 0 0-1.302 4.665c0 1.194.232 2.333.654 3.375Z" />
                  </svg>
                </div>
              </div>
              <div className='flex items-center justify-around px-2 py-5 text-center text-black rounded-lg bg-purple-400/80'>
                <div className='flex flex-col'>
                  <span className='text-2xl font-bold'>{currentData?.totalComment}</span>
                  <span className='text-sm font-medium'>Total Comments</span>
                </div>
                <div className='flex items-center justify-center w-10 h-10 text-white rounded-full shadow-2xl drop-shadow-2xl bg-purple-600/60'>
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-6 h-6">
                    <path strokeLinecap="round" strokeLinejoin="round" d="M7.5 8.25h9m-9 3H12m-9.75 1.51c0 1.6 1.123 2.994 2.707 3.227 1.129.166 2.27.293 3.423.379.35.026.67.21.865.501L12 21l2.755-4.133a1.14 1.14 0 0 1 .865-.501 48.172 48.172 0 0 0 3.423-.379c1.584-.233 2.707-1.626 2.707-3.228V6.741c0-1.602-1.123-2.995-2.707-3.228A48.394 48.394 0 0 0 12 3c-2.392 0-4.744.175-7.043.513C3.373 3.746 2.25 5.14 2.25 6.741v6.018Z" />
                  </svg>
                </div>
              </div>
              <div className='flex items-center justify-around px-2 py-5 text-center text-black rounded-lg bg-green-400/80'>
                <div className='flex flex-col'>
                  <span className='text-2xl font-bold'>{currentData?.totalView || 1}</span>
                  <span className='text-sm font-medium'>Total Views</span>
                </div>
                <div className='flex items-center justify-center w-10 h-10 text-white rounded-full shadow-2xl drop-shadow-2xl bg-green-600/60'>
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-6 h-6">
                    <path strokeLinecap="round" strokeLinejoin="round" d="M2.036 12.322a1.012 1.012 0 0 1 0-.639C3.423 7.51 7.36 4.5 12 4.5c4.638 0 8.573 3.007 9.963 7.178.07.207.07.431 0 .639C20.577 16.49 16.64 19.5 12 19.5c-4.638 0-8.573-3.007-9.963-7.178Z" />
                    <path strokeLinecap="round" strokeLinejoin="round" d="M15 12a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z" />
                  </svg>

                </div>
              </div>
            </div>
          </div>}

          <div>
            {chartData && (
              <>
                <div className="w-full sm:h-[450px] bg-white shadow-2xl mx-auto flex items-center justify-center p-4 rounded-lg">
                  <Doughnut
                    data={chartData}
                    options={{
                      responsive: true,
                      plugins: {
                        title: {
                          display: true,
                          text: `Number of contributions depend on status for ${currentYear}`,
                          font: {
                            size: 20, // Kích thước font chữ
                            weight: 'bold' // Độ đậm của font chữ
                          }
                        },
                        legend: {
                          labels: {
                            font: {
                              size: 20, // Kích thước font chữ cho nhãn trong legend
                              weight: 'bold' // Độ đậm của font chữ cho nhãn trong legend
                            }
                          }
                        },
                        tooltip: {
                          titleFont: {
                            size: 20, // Kích thước font chữ cho tiêu đề tooltip
                            weight: 'bold' // Độ đậm của font chữ cho tiêu đề tooltip
                          },
                          bodyFont: {
                            size: 14, // Kích thước font chữ cho nội dung tooltip
                            weight: 'normal' // Độ đậm của font chữ cho nội dung tooltip
                          }
                        }
                      }
                    }}
                  />
                </div>
              </>)}
          </div>
        </div>
      </div>
      <Separator className="mt-4"></Separator>
      <StudentLineChart data={allData && allData || []}></StudentLineChart>
    </AdminLayout>
  )
}
