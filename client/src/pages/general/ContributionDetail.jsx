import DynamicBreadcrumb from '@/components/DynamicBreadcrumbs'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Label } from '@/components/ui/label'
import GeneralLayout from '@/layouts'
import { Download, Eye, Heart, LinkedinIcon, ViewIcon } from 'lucide-react'
import React from 'react'
import DOMPurify from 'dompurify';
export default function ContributionDetail() {
  const content = `<p>
  <meta charset="utf-8"><span data-metadata=""></span><span data-buffer=""></span><span style="white-space:pre-wrap;"><strong>Chương 1</strong></span>
</p>
<p>
  <meta charset="utf-8"><span data-metadata=""></span><span data-buffer=""></span><span style="white-space:pre-wrap;">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span><br><br><span style="white-space:pre-wrap;">Juicy meatballs brisket slammin' baked shoulder. Juicy smoker soy sauce burgers brisket. polenta mustard hunk greens. Wine technique snack skewers chuck excess. Oil heat slowly. slices natural delicious, set aside magic tbsp skillet, bay leaves brown centerpiece. fruit soften edges frond slices onion snack pork steem on wines excess technique cup; Cover smoker soy sauce fruit snack. Sweet one-dozen scrape delicious, non sheet raw crunch mustard. Minutes clever slotted tongs scrape, brown steem undisturbed rice.</span><br><br><span style="white-space:pre-wrap;">Food qualities braise chicken cuts bowl through slices butternut snack. Tender meat juicy dinners. One-pot low heat plenty of time adobo fat raw soften fruit. sweet renders bone-in marrow richness kitchen, fricassee basted pork shoulder. Delicious butternut squash hunk. Flavor centerpiece plate, delicious ribs bone-in meat, excess chef end. sweet effortlessly pork, low heat smoker soy sauce flavor meat, rice fruit fruit. Romantic fall-off-the-bone butternut chuck rice burgers.</span>
</p>
<figure class="image image_resized" style="width:61.75%;" data-ckbox-resource-id="viPjShbsnIlO">
  <picture>
      <source srcset="https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/103.webp 103w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/206.webp 206w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/309.webp 309w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/412.webp 412w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/515.webp 515w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/618.webp 618w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/721.webp 721w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/824.webp 824w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/927.webp 927w,https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/1024.webp 1024w" sizes="(max-width: 1024px) 100vw, 1024px" type="image/webp"><img src="https://ckbox.cloud/0e7010f6ec2b53fa2ac6/assets/viPjShbsnIlO/images/1024.jpeg" width="1024" height="682">
  </picture>
</figure>
<p>
  <meta charset="utf-8"><span data-metadata=""></span><span data-buffer=""></span>
  </strong><span style="white-space:pre-wrap;"><strong>Chương 2</strong></span>
</p>
<p>
  <meta charset="utf-8"><span data-metadata=""></span><span data-buffer=""></span>
</p>
<p><span style="white-space:pre-wrap;">Gastronomy atmosphere set aside. Slice butternut cooking home. Delicious romantic undisturbed raw platter will meld. Thick Skewers skillet natural, smoker soy sauce wait roux. slices rosette bone-in simmer precision alongside baby leeks. Crafting renders aromatic enjoyment, then slices taco. Minutes undisturbed cuisine lunch magnificent mustard curry. Juicy share baking sheet pork. Meals ramen rarities selection, raw pastries richness magnificent atmosphere. Sweet soften dinners, cover mustard infused skillet, Skewers on culinary experience.</span></p>`
  const cleanHTML = DOMPurify.sanitize(content);
  return (
    <GeneralLayout>
      <div className='container py-10'>
        <DynamicBreadcrumb></DynamicBreadcrumb>
        {/* Top post */}
        <div className='flex flex-col items-center gap-6 my-5 medium:flex-row'>
          <img src="https://plus.unsplash.com/premium_photo-1686149758342-9f0f249f2989?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxlZGl0b3JpYWwtZmVlZHwyfHx8ZW58MHx8fHx8" alt="" className='rounded-lg w-full xl:h-[500px] medium:w-[60%] xl:w-auto object-cover flex-shrink-0' />
          <div className="flex-1">
            <div className="flex items-center justify-between">
              <Badge variant="destructive">Marketing</Badge>
              <Button className="bg-transparent border border-black text-black-500 hover:bg-red-500 hover:text-white hover:border-white"><Heart></Heart></Button>
            </div>
            <h2 className="mt-3 text-2xl font-semibold text-ellipsis line-clamp-4 medium:text-4xl">Lorem ipsum, dolor sit amet consectetur adipisicing elit. Cumque porro quam, reprehenderit nam aut minus sit cum sunt doloremque. Ipsa sequi temporibus incidunt officia. Iusto amet ipsam distinctio maxime pariatur.
              Nesciunt, laboriosam libero. Magni, impedit vel harum saepe labore libero laudantium vero ad unde, alias excepturi autem quaerat ullam laboriosam modi accusantium ratione illum facere assumenda natus nihil aliquam eum.
              Odit quasi similique quibusdam cupiditate illum mollitia ratione impedit architecto totam esse obcaecati maiores nobis porro, ad, ea, unde aspernatur perferendis est distinctio minus eius molestiae. Illum expedita beatae veniam.</h2>
            <div className="flex flex-wrap items-center justify-between my-6 text-xs font-semibold text-gray-600 md:text-sm medium:text-base">
              <div className='flex flex-wrap items-center gap-1'>
                <p>March, 2023</p>
                <div className='w-1 h-1 bg-gray-600 rounded-full md:w-2 md:h-2'></div>
                <div>Andiez Le</div>
              </div>
              <div className='flex flex-wrap items-center gap-3'>
                <div className='flex items-center gap-1'>
                  <Eye className='w-4 h-4 md:w-5 md:h-5'></Eye>
                  <p>12312</p>
                </div>
                <div className='flex items-center gap-1'>
                  <Heart className='w-4 h-4 md:w-5 md:h-5'></Heart>
                  <p>12312</p>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div dangerouslySetInnerHTML={{ __html: cleanHTML }} className='richtext-css' />
        <div className='my-5'>
          <div className="flex items-center justify-between my-4">
            <h2 className='font-bold'>File attached</h2>
            <Button className="bg-blue-500">Download all</Button>
          </div>
          <div className="grid-cols-2 gap-6 p-10 rounded-lg shadow-lg h-[250px] overflow-y-scroll md:overflow-auto grid md:h-auto md:grid-cols-5">
            <div className="z-10 flex flex-col items-center justify-center p-4 rounded-lg cursor-pointer hover:bg-slate-100" >
              <img src={"../word.png"} alt="" className="object-cover w-14 h-14 lg:h-24 lg:w-24 " />
              <div className="text-center">File name 1</div>
              <div className="flex items-center justify-center gap-2">
              </div>
            </div>
            <div className="z-10 flex flex-col items-center justify-center p-4 rounded-lg cursor-pointer hover:bg-slate-100" >
              <img src={"../word.png"} alt="" className="object-cover w-14 h-14 lg:h-24 lg:w-24 " />
              <div className="text-center">File name 1</div>
              <div className="flex items-center justify-center gap-2">
              </div>
            </div>
            <div className="z-10 flex flex-col items-center justify-center p-4 rounded-lg cursor-pointer hover:bg-slate-100" >
              <img src={"../word.png"} alt="" className="object-cover w-14 h-14 lg:h-24 lg:w-24 " />
              <div className="text-center">File name 1</div>
              <div className="flex items-center justify-center gap-2">
              </div>
            </div>
            <div className="z-10 flex flex-col items-center justify-center p-4 rounded-lg cursor-pointer hover:bg-slate-100" >
              <img src={"../word.png"} alt="" className="object-cover w-14 h-14 lg:h-24 lg:w-24 " />
              <div className="text-center">File name 1</div>
              <div className="flex items-center justify-center gap-2">
              </div>
            </div>
            <div className="z-10 flex flex-col items-center justify-center p-4 rounded-lg cursor-pointer hover:bg-slate-100" >
              <img src={"../word.png"} alt="" className="object-cover w-14 h-14 lg:h-24 lg:w-24 " />
              <div className="text-center">File name 1</div>
              <div className="flex items-center justify-center gap-2">
              </div>
            </div>
          </div>
        </div>
      </div>
    </GeneralLayout>
  )
}
