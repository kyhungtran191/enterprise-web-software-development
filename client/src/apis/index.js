export const URL = "http://localhost:5272/api"
export const adminAPI = `/admin`
export const clientAPI = `/client`

// Authenticate API
export const loginAPI = `http://localhost:5272/Auth/login`
export const forgotPasswordAPI = `${URL}${clientAPI}/Users/forgot-password`
export const resetPasswordAPI = `${URL}${clientAPI}/Users/reset-password`
/**Client(Authenticated) */
// Done
export const recentContributionAPI = `${URL}${clientAPI}/Users/recent-contribution`

export const profileAPI = `${URL}${clientAPI}/Users/my-profile`
export const editProfileAPI = `${URL}${clientAPI}/Users/edit-profile`

// Like
export const favoriteAPI = `${URL}${clientAPI}/Users/my-favorite`
export const toggleFavoriteAPI = `${URL}${clientAPI}/PublicContribution/toggle-like`
// Read Later
export const readLaterListAPI = `${URL}${clientAPI}/Users/read-later`
export const toggleReadLaterAPI = `${URL}${clientAPI}/PublicContribution/toggle-read-later`
/** Client */
//Contribution
export const allContributionAPI = `${URL}${clientAPI}/PublicContribution/paging`
export const latestContribution = `${URL}${clientAPI}/PublicContribution/latest`
export const featuredContribution = `${URL}${clientAPI}/PublicContribution/featured-contribution`
export const publicContributionAPI = `${URL}${clientAPI}/PublicContribution`
// Faculty
// Done
export const allFacultiesAPI = `${URL}${clientAPI}/Faculties`
// Contributor
export const topContributors = `${URL}${clientAPI}/PublicContribution/top-contributors`
// Refresh Token 
export const refreshTokenAPI = `${URL}/Tokens/Refresh`