import React, { useState } from 'react'
import * as XLSX from 'xlsx'
import { Button } from './ui/button'

const ExcelExport = ({
  data,
  className = '',
  ignoreField,
  title = 'Export All Records to Excel'
}) => {
  if (!data) return null
  const { dataSets, ...rest } = data[0]
  const columns = rest && Object?.keys(rest)

  const exportToExcel = () => {
    const excelData = data.map((item) => {
      const rowData = []
      for (const key in item) {
        if (key !== 'dataSets') {
          rowData.push(item[key])
        }
      }
      return rowData
    })
    const dataCurrent = [columns, ...excelData]
    const ws = XLSX.utils.aoa_to_sheet(dataCurrent)
    const wb = XLSX.utils.book_new()
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1')
    XLSX.writeFile(wb, 'data.xlsx')
  }

  return (
    <div className={`text-right ${className} my-2`}>
      <Button
        className='font-semibold text-center text-white bg-green-500'
        onClick={exportToExcel}
      >
        {title}
      </Button>
    </div>
  )
}

export default ExcelExport
