export const LocalStorageEventTarget = new EventTarget()

export const saveAccessTokenToLS = (accessToken) => {
  localStorage.setItem('access_token', accessToken)
}

export const clearAccessTokenFromLS = () => {
  localStorage.removeItem('access_token')
}

export const getAccessTokenFromLS = () =>
  localStorage.getItem('access_token') || ''

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

export const savePermissions = (permissions) => {
  try {
    const permissionsJSON = JSON.stringify(permissions)
    localStorage.setItem('permissions', permissionsJSON)
  } catch (error) {
    console.error('Error saving permissions to local storage', error)
  }
}

export const getPermissions = () => {
  try {
    const permissionsJSON = localStorage.getItem('permissions')
    return permissionsJSON ? JSON.parse(permissionsJSON) : null
  } catch (error) {
    console.error('Error retrieving permissions from local storage', error)
    return null
  }
}

export const deletePermissions = () => {
  try {
    localStorage.removeItem('permissions')
  } catch (error) {
    console.error('Error deleting permissions from local storage', error)
  }
}
