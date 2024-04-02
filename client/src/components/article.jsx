import React from 'react'
import { Badge } from './ui/badge'
import { formatDate } from '@/utils/helper'
import { Link } from 'react-router-dom'

export default function Article({ isRevert = false, className, status, classImageCustom, article }) {
  return (
    <Link to={`/contributions/${article?.slug}`}>
      <div className={`flex ${isRevert ? "flex-col md:flex-row" : "flex-col"} items-start gap-3 ${className} cursor-pointer hover:bg-slate-100 p-2 rounded-lg`}>
        <img src={`${article?.thumbnails?.length > 0 ? article?.thumbnails[0].path : "https://variety.com/wp-content/uploads/2021/04/Avatar.jpg"}`} alt="" className={`${isRevert ? `w-full md:w-[35%] h-[300px] md:h-[300px] ${classImageCustom}` : "w-full h-[600px"}  object-cover rounded-md`} />
        <div className="flex-1 p-2">
          <div className="flex flex-wrap items-center justify-between gap-2">
            <div className='flex items-center gap-1 medium:gap-2'>
              <img src="https://variety.com/wp-content/uploads/2021/04/Avatar.jpg" alt="" className="flex-shrink-0 object-cover w-12 h-12 rounded-full" />
              <h3 className='text-sm font-semibold medium:text-base'>{article?.userName}</h3>
            </div>
            <div className=''>
              {status && <Badge variant="outline" className={"text-blue-600 mx-1"}>Pending</Badge>}
              <Badge variant="outline">{article?.facultyName}</Badge>
            </div>
          </div>
          <h2 className="text-ellipsis line-clamp-2 medium:h-[65px] font-semibold text-xl medium:text-2xl mt-3">{article?.title}</h2>
          <p className='text-sm text-ellipsis line-clamp-3 text-slate-700 medium:text-base'>{article.shortDescription}</p>
          <p className="mt-2 text-sm medium:text-base">{formatDate(article?.publicDate)}</p>
        </div>
      </div>
    </Link>

  )
}
