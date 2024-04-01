import { AppContext } from "@/contexts/app.context"
import { useAppContext } from "@/hooks/useAppContext"
import axios from "axios"
import { jwtDecode } from "jwt-decode"
import { useContext } from "react"

const instanceAxios = axios.create({
  baseURL: URL
})
const AxiosInterceptor = ({ children }) => {
  let accessToken = "";
  let refreshToken = "";
  const { isAuthenticated } = useAppContext()
  instanceAxios.interceptors.request.use(async function (config) {
    if (accessToken) {
      const decoded = jwtDecode(accessToken)
      if (decoded.exp > Date.now() / 1000) {
        config.headers.authorization = `Bearer ${accessToken}`
        return config
      } else {
        if (refreshToken) {
          const decodedRF = jwtDecode(refreshToken)
          if (decodedRF.exp > Date.now() / 1000) {
            await axios
              .post(
                `/refresh-token`,
                {},
                {
                  headers: {
                    Authorization: `Bearer ${refreshToken}`
                  }
                }
              )
              .then(res => {
                if (res.data.data.access_token) {
                  const newAccessToken = res.data.data.access_token
                  if (newAccessToken) {
                    config.headers.authorization = `Bearer ${newAccessToken}`
                    // setLocalUserData(JSON.stringify(user), newAccessToken, refreshToken)
                  } else {
                    // handleRedirectLogin(router, setUser)
                  }
                }
              })
              .catch(() => {
                // handleRedirectLogin(router, setUser)
              })
          } else {
            // handleRedirectLogin(router, setUser)
          }
        }
      }
    } else {
      // handleRedirectLogin(router, setUser)
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
