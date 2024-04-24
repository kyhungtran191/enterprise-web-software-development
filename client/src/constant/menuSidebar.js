import {
  CircleGauge,
  BookText,
  UserCog,
  Settings,
  CalendarDays,
  Heart,
  GraduationCap,
  User,
  MoreHorizontal,
  ArrowUpDown,
  Clock,
  BarChart
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
    title: 'Faculties',
    icon: GraduationCap,
    href: '/admin/faculties'
  }
]
export const STUDENT_OPTIONS = [
  {
    title: 'My Dashboard',
    icon: BarChart,
    href: '/student-manage/dashboard'
  },
  {
    title: 'Recents Posts',
    icon: CircleGauge,
    href: '/student-manage/recent'
  },
  {
    title: 'Profile',
    icon: UserCog,
    href: '/profile'
  },
  {
    title: 'Read Later',
    icon: Clock,
    href: '/student-manage/read-later'
  },
  {
    title: 'Favorites',
    icon: Heart,
    href: '/student-manage/favorites'
  }
]

export const MC_OPTIONS = [
  {
    title: 'Faculty Contributions',
    icon: CircleGauge,
    href: '/coodinator-manage/contributions'
  },
  {
    title: 'Profile',
    icon: UserCog,
    href: '/profile'
  },
  {
    title: 'Settings GAC',
    icon: Clock,
    href: '/coodinator-manage/setting-guest'
  }
]

export const MM_OPTIONS = [
  {
    title: 'Dashboard',
    icon: CircleGauge,
    href: '/mm/dashboard'
  },
  {
    title: 'Contributions',
    icon: BookText,
    href: '/mm/contributions'
  },
  {
    title: 'Users',
    icon: User,
    href: '/mm/users'
  },
  {
    title: 'Not comment contributions',
    icon: BookText,
    href: '/mm/not-comment-contributions'
  }
]
