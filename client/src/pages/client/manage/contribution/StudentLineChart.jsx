import React from 'react'
import { Line } from 'react-chartjs-2';

export default function StudentLineChart({ data }) {
  const academicYears = data.map(item => item?.academicYear);
  const totalLikes = data.map(item => item?.totalLike);
  const totalComments = data.map(item => item?.totalComment);
  const totalContributionApproved = data.map(item => item?.totalContributionApproved);
  const averageRatings = data.map(item => item?.averageRating);
  const chartData = {
    labels: academicYears,
    datasets: [
      {
        label: 'Total Likes',
        data: totalLikes,
        borderColor: 'blue',
        fill: false
      },
      {
        label: 'Total Comments',
        data: totalComments,
        borderColor: 'green',
        fill: false
      },
      {
        label: 'Total Contribution Approved',
        data: totalContributionApproved,
        borderColor: 'orange',
        fill: false
      },
      {
        label: 'Average Rating',
        data: averageRatings,
        borderColor: 'purple',
        fill: false
      }
    ]
  };
  const chartOptions = {
    plugins: {
      title: {
        display: true,
        text: `Tracking the growth of article elements over the years`,
        font: {
          size: 20,
          weight: 'bold'
        }
      },
      legend: {
        labels: {
          font: {
            size: 20,
            weight: 'bold'
          }
        }
      },
      tooltip: {
        titleFont: {
          size: 20,
          weight: 'bold'
        },
        bodyFont: {
          size: 14,
          weight: 'normal'
        }
      }
    }
  };
  return (
    <Line data={chartData} options={chartOptions} className='hidden sm:block' />
  )
}
