
export const LocalStorageEventTarget = new EventTarget()

export const saveAccessTokenToLS = (accessToken) => {
  localStorage.setItem('access_token', accessToken)
}

export const clearAccessTokenFromLS = () => {
  localStorage.removeItem('access_token')
}

export const getAccessTokenFromLS = () => localStorage.getItem('access_token') || ''

export const saveRefreshTokenToLS = (accessToken) => {
  localStorage.setItem('refresh_token', accessToken)
}

export const clearRefreshToken = () => {
  localStorage.removeItem('refresh_token')
}

export const getRefreshToken = () => localStorage.getItem('refresh_token') || ''
export const saveUserToLS = (user) => {
  localStorage.setItem('user', JSON.stringify(user))
}
export const getUserFromLS = () => {
  const user = localStorage.getItem('user')
  return user ? JSON.parse(user) : {}
}

export const clearUserFromLS = () => {
  localStorage.removeItem('user')
}

export const clearLS = () => {
  clearRefreshToken()
  clearAccessTokenFromLS()
  clearUserFromLS()
}