import { AppContext } from "@/contexts/app.context"
import { useAppContext } from "@/hooks/useAppContext"
import axios from "axios"
import { jwtDecode } from "jwt-decode"
import { useContext } from "react"
import { clearLS, getAccessTokenFromLS, getRefreshToken, saveAccessTokenToLS, saveRefreshTokenToLS } from "./auth"
import { refreshTokenAPI } from "@/apis"
import { toast } from "react-toastify"
import { useNavigate } from "react-router-dom"

const instanceAxios = axios.create({
  baseURL: URL
})
const AxiosInterceptor = ({ children }) => {
  let accessToken = getAccessTokenFromLS();
  let refreshToken = getRefreshToken();
  console.log("accessToken", accessToken);
  console.log("refreshToken", refreshToken);
  const navigate = useNavigate()
  instanceAxios.interceptors.request.use(async function (config) {
    if (accessToken) {
      const decoded = jwtDecode(accessToken)
      if (decoded.exp > Date.now() / 1000) {
        config.headers.authorization = `Bearer ${accessToken}`
        return config
      } else {
        if (refreshToken) {
          console.log(refreshToken)
          await axios
            .post(
              refreshTokenAPI,
              { accessToken, refreshToken },
            )
            .then(res => {
              console.log(res)
              if (res && res?.data && res?.data?.responseData) {
                const { accessToken: newAccessToken, refreshToken: newRefreshToken } = res?.data?.responseData
                config.headers.authorization = `Bearer ${newAccessToken}`
                if (newAccessToken && newRefreshToken) {
                  saveAccessTokenToLS(newAccessToken)
                  saveRefreshTokenToLS(newRefreshToken)
                } else {
                  toast.error("Không có access và refresh")
                }
              }
            })
            .catch((error) => {
              // clearLS()
              // navigate("/login")
              console.log(error)
            })
        } else {
          clearLS()
          navigate("/login")
        }
      }
    }
    return config
  })
  instanceAxios.interceptors.response.use(response => {
    return response
  })
  return <>{children}</>
}

export default instanceAxios
export { AxiosInterceptor }
