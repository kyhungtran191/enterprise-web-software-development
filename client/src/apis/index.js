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
// Coodinator
export const MCContributionsAPI = `${URL}/coodinator/Contributions/paging`
export const MCContributionsApprove = `${URL}/coodinator/Contributions/approve`
export const MCContributionsReject = `${URL}/coodinator/Contributions/reject`
// ADMIN
// Academic Year
export const getAllAcademicYearsAPI = `${URL}${adminAPI}/AcademicYear/paging`
export const createAcademicYearAPI = `${URL}${adminAPI}/AcademicYear`
export const getAcademicYearByIdAPI = `${URL}${adminAPI}/AcademicYear`
export const updateAcademicYearAPI = `${URL}${adminAPI}/AcademicYear`
export const deleteAcademicYearAPI = `${URL}${adminAPI}/AcademicYear`
export const activateAcademicYearAPI = `${URL}${adminAPI}/AcademicYear/activate`
export const deactivateAcademicYearAPI = `${URL}${adminAPI}/AcademicYear/inactivate`
// Roles
export const getAllRolesAPI = `${URL}${adminAPI}/Roles`
export const getRoleByIdAPI = `${URL}${adminAPI}/Roles`
export const createRoleAPI = `${URL}${adminAPI}/Roles`
export const updateRoleAPI = `${URL}${adminAPI}/Roles`
export const deleteRoleAPI = `${URL}${adminAPI}/Roles`
// Role permissions
export const getPermissionsForRoleAPI = `${URL}${adminAPI}/Roles`
export const updateRolePermissionsAPI = `${URL}${adminAPI}/Roles`
// Users
export const getAllUsersAPI = `${URL}${adminAPI}/Users`
export const getUserByIdAPI = `${URL}${adminAPI}/Users`
export const createUserAPI = `${URL}${adminAPI}/Users`
export const updateUserAPI = `${URL}${adminAPI}/Users`
export const deleteUserAPI = `${URL}${adminAPI}/Users`

// Faculty
export const createFacultyAPI = `${URL}${adminAPI}/Faculties`
export const getAllFacultiesAPI = `${URL}${adminAPI}/Faculties`
export const getFacultyByIdAPI = `${URL}${adminAPI}/Faculties`
export const updateFacultyAPI = `${URL}${adminAPI}/Faculties`
export const deleteFacultyAPI = `${URL}${adminAPI}/Faculties`
