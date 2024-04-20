import React, { useMemo, useState } from 'react'

import DynamicBreadcrumb from './DynamicBreadcrumbs'
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue
} from '@/components/ui/select'
import BarChart from './BarChart'
import PieChart from './PieChart'
import { useAcademicYearAdmin } from '@/query/useAcademicYearAdmin'
import { useQuery } from '@tanstack/react-query'
import { Report } from '@/services/admin'
import { colors } from '@/utils/color'
import Spinner from './Spinner'

const Dashboard = () => {
  const [selectedChartType, setSelectedChartType] = useState('contributions')
  const { academicYearsData } = useAcademicYearAdmin()
  const [selectedAcademicYear, setSelectedAcademicYear] = useState(
    academicYearsData[0]?.name
  )
  const options =
    selectedChartType !== 'percentage'
      ? {
          responsive: true,
          plugins: {
            legend: {
              position: 'top'
            },
            title: {
              display: true,
              text:
                selectedChartType === 'contributions'
                  ? 'Number of contributions within each Faculty for each academic year'
                  : 'Number of contributors within each Faculty for each academic year'
            }
          },
          scales: {
            x: {
              stacked: true
            },
            y: {
              stacked: true
            }
          }
        }
      : {
          responsive: true,
          plugins: {
            legend: {
              position: 'top'
            },
            title: {
              display: true,
              text: `Percentage of contributions within each Faculty for academic year ${selectedAcademicYear}`
            },
            datalabels: {
              formatter: function (value, context) {
                console.log(value)
                return context.chart.data.labels[context.dataIndex]
              }
            }
          }
        }

  const handleSelectChartType = (chartType) => {
    setSelectedChartType(chartType)
  }
  const handleSelectAcademicYear = (academicYear) => {
    console.log(academicYear)
    setSelectedAcademicYear(academicYear)
  }
  const { data: chartData, isLoading } = useQuery({
    queryKey: [
      'adminChartData',
      selectedChartType,
      selectedChartType === 'percentage' ? selectedAcademicYear : undefined
    ],
    queryFn: () => {
      switch (selectedChartType) {
        case 'contributions':
          return Report.getTotalContributionsPerFacultyForAllAcademicYears()
        case 'percentage':
          return Report.getContributionPercentageWithinFacultyByAcademicYear(
            selectedAcademicYear
          )
        case 'contributors':
          return Report.getTotalContributorsPerFacultyForAllAcademicYears()
        default:
          return null
      }
    },
    keepPreviousData: true,
    staleTime: 3 * 60 * 1000,
    enabled: selectedChartType !== '',
    refetchOnWindowFocus: false // Optionally set this to false to prevent refetch when window gains focus
  })
  const data = useMemo(() => {
    if (!chartData) return
    if (
      selectedChartType === 'contributions' ||
      selectedChartType === 'contributors'
    ) {
      const labels = chartData.data.response.map(
        (academicYearData) => academicYearData.academicYear
      )

      const faculties = [
        ...new Set(
          chartData.data.response.flatMap((academicYearData) =>
            academicYearData.dataSets.map((dataSet) => dataSet.faculty)
          )
        )
      ]

      const datasets = faculties.map((faculty) => {
        return {
          label: faculty,
          data: chartData.data.response.map((academicYearData) => {
            const facultyData = academicYearData.dataSets.find(
              (dataSet) => dataSet.faculty === faculty
            )
            return facultyData ? facultyData.data : 0
          }),
          backgroundColor: colors[Math.floor(Math.random() * colors.length)]
        }
      })
      return { labels, datasets }
    } else {
      const labels = [
        ...new Set(
          chartData.data.response.flatMap((data) =>
            data.dataSets.map((dataSet) => dataSet.faculty)
          )
        )
      ]
      const dataset = {
        label: 'Percentage of Contributions',
        data: chartData.data.response.flatMap((data) =>
          data.dataSets.map((dataSet) => dataSet.data)
        ),
        backgroundColor: labels.map(
          () => colors[Math.floor(Math.random() * colors.length)]
        )
      }
      return { labels, datasets: [dataset] }
    }
  }, [chartData, selectedChartType])
  return (
    <div className='w-full p-4'>
      <div className='flex flex-row justify-between'>
        <DynamicBreadcrumb />
      </div>
      <div className='flex flex-row my-4 space-x-2'>
        <Select
          onValueChange={handleSelectChartType}
          defaultValue={selectedChartType}
        >
          <SelectTrigger className='w-[280px]'>
            <SelectValue placeholder='Select chart type' />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value='contributions'>
              Number of contributions
            </SelectItem>
            <SelectItem value='percentage'>
              Percentage of contributions
            </SelectItem>
            <SelectItem value='contributors'>Number of contributors</SelectItem>
          </SelectContent>
        </Select>
        {selectedChartType === 'percentage' && (
          <Select
            onValueChange={handleSelectAcademicYear}
            defaultValue={selectedAcademicYear}
          >
            <SelectTrigger className='w-[180px]'>
              <SelectValue placeholder='Academic Year' />
            </SelectTrigger>
            <SelectContent>
              {academicYearsData.length &&
                academicYearsData.map((year) => (
                  <SelectItem key={year.name} value={year.name}>
                    {year.name}
                  </SelectItem>
                ))}
            </SelectContent>
          </Select>
        )}
      </div>
      {isLoading && selectedChartType !== '' && (
        <div className='container flex items-center justify-center min-h-screen'>
          <Spinner className={'border-blue-500'}></Spinner>
        </div>
      )}
      {!isLoading && (
        <div className='h-full w-full flex justify-center px-4 py-6 lg:px-8'>
          {selectedChartType === 'percentage' ? (
            <div className='w-[40%]'>
              <PieChart data={data} options={options} />
            </div>
          ) : (
            <div className='w-[80%]'>
              <BarChart data={data} options={options} />
            </div>
          )}
        </div>
      )}
    </div>
  )
}
export default Dashboard
