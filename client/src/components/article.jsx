import React from 'react'
import { Badge } from './ui/badge'

export default function Article({ isRevert = false, className, status }) {
  return (
    <div className={`flex ${isRevert ? "flex-col md:flex-row" : "flex-col"} items-start gap-3 ${className}`}>
      <img src="https://variety.com/wp-content/uploads/2021/04/Avatar.jpg" alt="" className={`${isRevert ? "w-[100%] md:w-[40%] h-[300px] md:h-[300px]" : "w-full h-[600px"}  object-cover rounded-lg`} />
      <div className="flex-1">
        <div className="flex flex-wrap items-center justify-between gap-2">
          <div className='flex items-center gap-1 medium:gap-2'>
            <img src="https://variety.com/wp-content/uploads/2021/04/Avatar.jpg" alt="" className="flex-shrink-0 object-cover w-12 h-12 rounded-full" />
            <h3 className='text-sm font-semibold medium:text-base'>Justin Tran</h3>
          </div>
          <div className=''>
            {status && <Badge variant="outline" className={"text-blue-600 mx-1"}>Pending</Badge>}
            <Badge variant="outline">Marketing</Badge>
          </div>
        </div>
        <h2 className="text-ellipsis line-clamp-2 medium:h-[65px] font-semibold text-xl medium:text-2xl mt-3">Lorem ipsum, dolor sit amet consectetur adipisicing elit. Cumque porro quam, reprehenderit nam aut minus sit cum sunt doloremque. Ipsa sequi temporibus incidunt officia. Iusto amet ipsam distinctio maxime pariatur.
          Nesciunt, laboriosam libero. Magni, impedit vel harum saepe labore libero laudantium vero ad unde, alias excepturi autem quaerat ullam laboriosam modi accusantium ratione illum facere assumenda natus nihil aliquam eum.
          Odit quasi similique quibusdam cupiditate illum mollitia ratione impedit architecto totam esse obcaecati maiores nobis porro, ad, ea, unde aspernatur perferendis est distinctio minus eius molestiae. Illum expedita beatae veniam.</h2>
        <p className='text-sm text-ellipsis line-clamp-3 text-slate-700 medium:text-base'>Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quaerat similique at molestias deleniti tempore consectetur commodi ab, beatae soluta dolorem nemo, quo totam repudiandae corporis distinctio voluptatibus accusamus sint ad.
          Magni culpa quia quis asperiores ipsum molestias aspernatur, laboriosam possimus? Mollitia laudantium iste autem placeat aspernatur. Ducimus aperiam, adipisci excepturi quo officiis nisi et rem in, animi quod, eaque nihil.
          Eveniet, magni! Sit, sint minima amet iure quo obcaecati totam explicabo consequatur molestiae dolore maiores, asperiores sunt maxime facilis deleniti natus nesciunt cupiditate corporis impedit repellendus ea hic quam ex!
          Odit veniam ducimus nam facilis pariatur beatae hic. Exercitationem eius iusto, ex eum temporibus quas, natus qui sit pariatur iure eveniet similique aliquam sapiente porro, deleniti nemo! Provident, soluta numquam.</p>
        <p className="mt-2 text-sm medium:text-base">03 February 2024</p>
      </div>
    </div>
  )
}
