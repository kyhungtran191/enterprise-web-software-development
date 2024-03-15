import Search from '@/components/Search'
import { Button } from '@/components/ui/button'
import AdminLayout from '@/layouts/AdminLayout'
import { ArrowDown10Icon, Plus } from 'lucide-react'
import React from 'react'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuLabel,
  DropdownMenuRadioGroup,
  DropdownMenuRadioItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import {
  Pagination,
  PaginationContent,
  PaginationEllipsis,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from "@/components/ui/pagination"
import Contributor from '@/components/contributor'
import Article from '@/components/article'

export default function StudentContribution() {
  const [position, setPosition] = React.useState("publish")
  return (
    <AdminLayout>
      <div className='flex flex-wrap items-center gap-3'>
        <Search className={"md:max-w-[70%]"}></Search>
        <div className='flex flex-wrap items-center gap-2'>
          <Button className="flex-1 py-7">
            <Plus></Plus>
            Add new Article
          </Button>
          <div className='flex-1'>
            <DropdownMenu >
              <DropdownMenuTrigger asChild className='w-full'>
                <Button variant="default" className="gap-4 border-none outline-none py-7 min-w-[145px]">{position.toUpperCase()} <ArrowDown10Icon></ArrowDown10Icon></Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent className="w-56">
                <DropdownMenuRadioGroup value={position} onValueChange={setPosition}>
                  <DropdownMenuRadioItem value="pending">Pending</DropdownMenuRadioItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuRadioItem value="published">Published</DropdownMenuRadioItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuRadioItem value="rejected" >Rejected</DropdownMenuRadioItem>
                </DropdownMenuRadioGroup>
              </DropdownMenuContent>
            </DropdownMenu>
          </div>

        </div>

      </div>
      <div>{Array(5).fill(0).map((item, index) => (<Article key={index} isRevert={true} className={"my-4"}></Article>))}</div>
      <Pagination>
        <PaginationContent>
          <PaginationItem>
            <PaginationPrevious href="#" />
          </PaginationItem>
          <PaginationItem>
            <PaginationLink href="#">1</PaginationLink>
          </PaginationItem>
          <PaginationItem>
            <PaginationLink href="#" isActive>
              2
            </PaginationLink>
          </PaginationItem>
          <PaginationItem>
            <PaginationLink href="#">3</PaginationLink>
          </PaginationItem>
          <PaginationItem>
            <PaginationEllipsis />
          </PaginationItem>
          <PaginationItem>
            <PaginationNext href="#" />
          </PaginationItem>
        </PaginationContent>
      </Pagination>
    </AdminLayout>
  )
}
