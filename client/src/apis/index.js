export const URL = 'http://localhost:5272/api'
export const adminAPI = `/admin`
export const clientAPI = `/client`

// Authenticate API
export const loginAPI = `http://localhost:5272/Auth/login`
export const forgotPasswordAPI = `${URL}${clientAPI}/Users/forgot-password`
export const resetPasswordAPI = `${URL}${clientAPI}/Users/reset-password`
/**Client(Authenticated) */
export const recentContributionAPI = `${URL}${clientAPI}/Users/recent-contribution`
export const profileAPI = `${URL}${clientAPI}/Users/my-profile`
export const editProfileAPI = `${URL}${clientAPI}/Users/edit-profile`
export const ContributionAPI = `${URL}${clientAPI}/Contributions`
export const editContributionsInfo = `${URL}${clientAPI}/Users/contribution`
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
export const allFacultiesAPI = `${URL}${clientAPI}/Faculties`
// Academic Year
export const allAcademicYear = `${URL}${clientAPI}/AcademicYear`
// Contributor
export const topContributors = `${URL}${clientAPI}/PublicContribution/top-contributors`
// Refresh Token 
export const refreshTokenAPI = `${URL}/Tokens/Refresh`
// Validate reset password
export const validateResetPasswordAPI = `${URL}${clientAPI}/Users/validate-forgot-token`
// ADMIN
// Academic Year
export const getAllAcademicYearsAPI = `${URL}${adminAPI}/AcademicYear/paging`
export const createAcademicYearAPI = `${URL}${adminAPI}/AcademicYear`
export const getAcademicYearByIdAPI = `${URL}${adminAPI}/AcademicYear`
export const updateAcademicYearAPI = `${URL}${adminAPI}/AcademicYear`
