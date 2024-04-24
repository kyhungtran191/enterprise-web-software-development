import React from 'react'
import { useLocation, Link } from 'react-router-dom'
import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbLink,
  BreadcrumbList,
  BreadcrumbSeparator
} from '@/components/ui/breadcrumb'
import { routesMapping } from '@/configs/global'

const DynamicBreadcrumb = () => {
  const location = useLocation()
  const pathNames = location.pathname.split('/').filter((x) => x)
  const breadcrumbItems = pathNames.map((segment, index) => {
    const routeTo = `/${pathNames.slice(0, index + 1).join('/')}`
    const isLast = index === pathNames.length - 1
    // Use the readable name if available, otherwise capitalize the segment
    const label =
      routesMapping[segment] ||
      segment
        .split('-')
        .map((s) => s.charAt(0).toUpperCase() + s.slice(1))
        .join(' ')

    return (
      <React.Fragment key={segment}>
        <BreadcrumbItem isCurrentPage={isLast}>
          <BreadcrumbLink as={isLast ? 'span' : Link} to={routeTo}>
            {label}
          </BreadcrumbLink>
        </BreadcrumbItem>
        {!isLast && <BreadcrumbSeparator />}
      </React.Fragment>
    )
  })

  return (
    <Breadcrumb>
      <BreadcrumbList>
        <BreadcrumbItem>
          <BreadcrumbLink href='/admin'>Home</BreadcrumbLink>
        </BreadcrumbItem>
        {location.pathname !== '/' && <BreadcrumbSeparator />}
        {breadcrumbItems}
      </BreadcrumbList>
    </Breadcrumb>
  )
}

export default DynamicBreadcrumb
