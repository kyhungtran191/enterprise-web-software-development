import React from 'react'
import { Line } from 'react-chartjs-2';

export default function StudentLineChart({ data }) {
  const academicYears = data.map(item => item?.academicYear);
  const totalLikes = data.map(item => item?.totalLike);
  const totalComments = data.map(item => item?.totalComment);
  const totalView = data.map(item => item?.totalView);
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
        label: 'Total Views ',
        data: totalView,
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
    responsive: true,
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
    <div className="hidden sm:block">
      <Line data={chartData} options={chartOptions} />

    </div>
  )
}
