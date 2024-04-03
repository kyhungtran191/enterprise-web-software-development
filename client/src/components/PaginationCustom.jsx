import React from 'react'
import { Link, createSearchParams } from 'react-router-dom'
import { Pagination, PaginationContent, PaginationItem, PaginationLink, PaginationNext, PaginationPrevious } from './ui/pagination'

const RANGE = 2
export default function PaginationCustom({ path, queryConfig, totalPage }) {
  const page = Number(queryConfig.pageindex)
  let dotAfter = false
  let dotBefore = false
  function renderDotAfter(index) {
    if (!dotAfter) {
      dotAfter = true
      return (
        <span key={index} className='px-3 py-2 mx-2 bg-white border rounded shadow-sm cursor-pointer'>
          ...
        </span>
      )
    }
  }
  function renderDotBefore(index) {
    if (!dotBefore) {
      dotBefore = true
      return (
        <span key={index} className='px-3 py-2 mx-2 bg-white border rounded shadow-sm cursor-pointer'>
          ...
        </span>
      )
    }
  }
  return <Pagination>
    <PaginationContent>
      <Link
        to={{
          pathname: path,
          search: createSearchParams({
            ...queryConfig,
            pageindex: page - 1
          }).toString()
        }}
        className={`block ${page === 1 ? 'opacity-50 pointer-events-none' : 'cursor-pointer'}`}
      >
        <PaginationItem>
          <PaginationPrevious />
        </PaginationItem>
      </Link>
      {Array(totalPage)
        .fill(0)
        .map((_, index) => {
          const pageNumber = index + 1
          if (page <= RANGE * 2 + 1 && pageNumber > page + RANGE && pageNumber <= totalPage - RANGE) {
            return renderDotAfter(index)
          } else if (page > RANGE * 2 + 1 && page < totalPage - RANGE * 2) {
            if (pageNumber < page - RANGE && pageNumber > RANGE) {
              return renderDotBefore(index)
            } else if (pageNumber > page + RANGE && pageNumber < totalPage - RANGE + 1) {
              return renderDotAfter(index)
            }
          } else if (page >= totalPage - RANGE * 2 && pageNumber > RANGE && pageNumber < page - RANGE) {
            return renderDotBefore(index)
          }

          return (
            <Link
              to={{
                pathname: path,
                search: createSearchParams({
                  ...queryConfig,
                  pageindex: pageNumber
                }).toString()
              }}
              key={index}
            >
              <PaginationItem >
                <PaginationLink href="#" isActive={Number(pageNumber) == page}>{pageNumber}</PaginationLink>
              </PaginationItem>
            </Link>
          )
        })}
      <Link
        to={{
          pathname: path,
          search: createSearchParams({
            ...queryConfig,
            pageindex: page + 1
          }).toString()
        }}
        className={`block ${page === totalPage ? 'opacity-50 pointer-events-none' : 'cursor-pointer'}`}
      >
        <PaginationItem>
          <PaginationNext />
        </PaginationItem>
      </Link>
    </PaginationContent>
  </Pagination>
}


