import { useCallback, useRef } from "react";
import Agent from "../API/agent";
import { AuthResponse } from "../Types/AuthResponse";
import * as axiosLib from "axios";

let isRefreshing = false; // Флаг для отслеживания текущего состояния
let refreshPromise: Promise<AuthResponse | null> | null = null; // Хранение текущего Promise, чтобы повторно использовать его

const useRefreshToken = () => {
  const isRefreshingRef = useRef(false);
  const refreshPromiseRef = useRef<Promise<AuthResponse | null> | null>(null);

  const refresh = useCallback(async (): Promise<AuthResponse | null> => {
    if (isRefreshing && refreshPromise) {
      // Если уже идет запрос, возвращаем существующий Promise
      return refreshPromise;
    }

    isRefreshing = true; // Устанавливаем блокировку

    refreshPromise = new Promise<AuthResponse | null>(async (resolve, reject) => {
      try {
        console.log("refresh called");
        const token = localStorage.getItem("accessToken");

        const response = await Agent.Auth.refreshToken(token || "");

        if (response.data.token) {
          localStorage.setItem("accessToken", response.data.token);
          resolve(response.data);
        } else {
          resolve(null);
        }
      } catch (error: unknown) {
        if (axiosLib.isAxiosError(error)) {
          console.error("Error during token refreshment:", error.response?.data || error.message);
        } else {
          console.error("Unexpected error:", error);
        }
        reject(error);
      } finally {
        isRefreshingRef.current = false;
        refreshPromiseRef.current = null;
      }
    });

    return refreshPromiseRef.current;
  }, []);

  return refresh;
};

export default useRefreshToken;
