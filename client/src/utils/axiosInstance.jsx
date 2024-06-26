import axios from "axios"
import { jwtDecode } from "jwt-decode"
import { clearLS, getAccessTokenFromLS, getRefreshToken, saveAccessTokenToLS, saveRefreshTokenToLS } from "./auth"
import { refreshTokenAPI } from "@/apis"
import { toast } from "react-toastify"
import { useNavigate } from "react-router-dom"
import { useAppContext } from "@/hooks/useAppContext"
import { useQueryClient } from "@tanstack/react-query"

const instanceAxios = axios.create({
  baseURL: URL
});

let isRefreshing = false;
let refreshQueue = [];

const AxiosInterceptor = ({ children }) => {
  const navigate = useNavigate()
  let { setProfile, setIsAuthenticated } = useAppContext()
  let queryClient = useQueryClient()
  instanceAxios.interceptors.request.use(async function (config) {

    let accessToken = getAccessTokenFromLS();
    let refreshToken = getRefreshToken();
    if (accessToken) {
      const decoded = jwtDecode(accessToken)
      if (decoded.exp > ((Date.now() / 1000))) {
        config.headers.authorization = ` Bearer ${accessToken}`
        return config
      } else {
        if (refreshToken) {
          if (!isRefreshing) {
            isRefreshing = true;
            await axios
              .post(
                refreshTokenAPI,
                { accessToken, refreshToken },
              ).then(res => {
                if (res && res?.data?.responseData) {
                  const { accessToken: newAccessToken, refreshToken: newRefreshToken } = res?.data?.responseData

                  if (newRefreshToken) {
                    saveRefreshTokenToLS(newRefreshToken)
                  }
                  if (newAccessToken) {
                    config.headers.authorization = `Bearer ${newAccessToken}`
                    saveAccessTokenToLS(newAccessToken);
                    refreshQueue.forEach((cb) => cb(newAccessToken));
                    refreshQueue = [];
                    // Check false
                    isRefreshing = false
                  }
                  else {
                    toast.error("Không có access và refresh")
                  }
                }
                return config;
              })
              .catch((error) => {
                toast.error("Refresh Token Timeout!")
                clearLS()
                setProfile({})
                setIsAuthenticated(false)
                queryClient.clear()
                return navigate("/login")
              })

          } else {
            return new Promise((resolve) => {
              refreshQueue.push((newAccessToken) => {
                config.headers.authorization = `Bearer ${newAccessToken}`;
                return resolve(config);
              });
            });
          }
        } else {
          console.log(1)
          clearLS()
          setProfile({})
          setIsAuthenticated(false)
          queryClient.clear()
          return navigate("/login")
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