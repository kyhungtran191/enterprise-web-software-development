import { useSearchParams } from 'react-router-dom'

const useParamsVariables = () => {
  const [params] = useSearchParams()
  const paramVariables = Object.fromEntries([...params])
  return paramVariables
}
export default useParamsVariables