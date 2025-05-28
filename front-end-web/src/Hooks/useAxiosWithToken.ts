import { axiosWithToken } from "../API/axioscfg";
import { useEffect, useState } from "react";
import { InternalAxiosRequestConfig, AxiosRequestConfig, AxiosError, AxiosResponse } from "axios";
import useRefreshToken from "./useRefreshToken";
import useAuth from "./useAuth";
import { AuthResponse } from "../Types/AuthResponse";

const useAxiosWithToken = () => {
  const refresh = useRefreshToken();
  const { setAuth } = useAuth();
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const requestIntercept = axiosWithToken.interceptors.request.use(
      (config: InternalAxiosRequestConfig) => {
        if (!config.headers?.Authorization) {
          const token = localStorage.getItem("accessToken");
          if (token) {
            config.headers.set("Authorization", `Bearer ${token}`);
          }
        }
        return config;
      },
      (error) => Promise.reject(error)
    );

    const responseIntercept = axiosWithToken.interceptors.response.use(
      (response: AxiosResponse): AxiosResponse => response,
      async (error: AxiosError): Promise<AxiosResponse | Promise<never>> => {
        const originalRequest = error?.config as AxiosRequestConfig & {
          sent?: boolean;
        };
        if (error?.response?.status === 401 && !originalRequest?.sent) {
          originalRequest.sent = true;
          try {
            const newData: AuthResponse | null = await refresh();
            const newAccessToken = newData?.token;
            if (newAccessToken) {
              originalRequest.headers = {
                ...originalRequest.headers,
                Authorization: `Bearer ${newAccessToken}`,
              };
              const { token, ...user } = newData;
              setAuth(user);
              return axiosWithToken(originalRequest);
            }
          } catch (err) {
            console.error("Token refresh failed:", err);
          }
        }
        return Promise.reject(error);
      }
    );

    setIsReady(true);

    return () => {
      axiosWithToken.interceptors.request.eject(requestIntercept);
      axiosWithToken.interceptors.response.eject(responseIntercept);
    };
  }, [refresh, setAuth]);

  return isReady;
};

export default useAxiosWithToken;
