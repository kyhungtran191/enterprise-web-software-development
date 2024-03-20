import { path } from '@/configs'
import GeneralLayout from '@/layouts'
import Home from '@/pages/general/Home'
import React from 'react'
import { useRoutes } from 'react-router-dom'


export default function useRoutesElements() {
  const routes = useRoutes([
    {
      path: path.home,
      element: (
        <GeneralLayout>
          <Home></Home>
        </GeneralLayout>
      )
    },
    {
      path: path.my_contribution.index,
      element: (
        <GeneralLayout>
          <Home></Home>
        </GeneralLayout>
      ),
      children: [
        { path: path.my_contribution.children.recent, element: <></> }
      ]
    }
  ])
  return routes
}