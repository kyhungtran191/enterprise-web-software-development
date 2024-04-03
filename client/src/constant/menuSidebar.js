import {
  CircleGauge,
  BookText,
  UserCog,
  Settings,
  CalendarDays,
  Heart,
  User,
  MoreHorizontal,
  ArrowUpDown,
  Clock
} from 'lucide-react'


export const ADMIN_OPTIONS = [
  {
    title: 'Dashboard',
    icon: CircleGauge,
    href: '/admin/dashboard'
  },
  {
    title: 'Contributions',
    icon: BookText,
    href: '/admin/contributions'
  },
  {
    title: 'Academic Years',
    icon: CalendarDays,
    href: '/admin/academic-years'
  },
  {
    title: 'Users',
    icon: User,
    href: '/admin/users'
  },
  {
    title: 'Roles & Permissions',
    icon: UserCog,
    href: '/admin/roles'
  },
  {
    title: 'Settings',
    icon: Settings,
    href: '/admin/settings'
  }
]
export const STUDENT_OPTIONS = [
  {
    title: 'Recents Posts',
    icon: CircleGauge,
    href: '/student-manage/recent'
  },
  {
    title: 'Profile',
    icon: UserCog,
    href: '/student-manage/profile'
  },
  {
    title: 'Read Later',
    icon: Clock,
    href: '/student-manage/read-later'
  },
  {
    title: 'Favorites',
    icon: Heart,
    href: '/student-manage/academic-years'
  },
  {
    title: 'Settings',
    icon: Settings,
    href: '/student-manage/settings'
  }
]
