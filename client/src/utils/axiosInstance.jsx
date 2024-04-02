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
  const navigate = useNavigate()
  instanceAxios.interceptors.request.use(async function (config) {
    let accessToken = getAccessTokenFromLS();
    let refreshToken = getRefreshToken();
    if (accessToken) {
      const decoded = jwtDecode(accessToken)
      if (decoded.exp > Date.now() / 1000) {
        config.headers.authorization = `Bearer ${accessToken}`
        return config
      } else {
        if (refreshToken) {
          console.log("current", refreshToken)
          await axios
            .post(
              refreshTokenAPI,
              { accessToken, refreshToken },
            ).then(res => {
              if (res && res?.data?.responseData) {
                const { accessToken: newAccessToken, refreshToken: newRefreshToken } = res?.data?.responseData
                if (newAccessToken) {
                  config.headers.authorization = `Bearer ${newAccessToken}`
                  saveAccessTokenToLS(newAccessToken)
                }
                if (newRefreshToken) {
                  saveRefreshTokenToLS(newRefreshToken)
                }
                else {
                  toast.error("Không có access và refresh")
                }
              }
              return config;
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
