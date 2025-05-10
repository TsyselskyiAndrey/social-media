import axios from "./axioscfg";
import { AuthResponse } from "../Types/AuthResponse";

interface LoginData {
  login: string;
  password: string;
  deviceId: string;
}

interface Step1Data {
  email: string;
  username: string;
  password: string;
  confirmpassword: string;
}

interface Step2Data {
  firstname: string;
  lastname: string;
  birthdate: string;
}

interface Step3Data {
  code: string;
}

const Auth = {
  login: (loginData: LoginData) =>
    axios.post<AuthResponse>("/api/auth/login", loginData, {
      headers: { "Content-Type": "application/json" },
      withCredentials: true,
    }),
  refreshToken: (token: string) =>
    axios.post<AuthResponse>(
      "/api/auth/refresh-token",
      { token },
      {
        headers: { "Content-Type": "application/json" },
        withCredentials: true,
      }
    ),
  registerStep1: (data: Step1Data) =>
    axios.post("/api/auth/registration-step-1", data, {
      headers: { "Content-Type": "application/json" },
      withCredentials: true,
    }),
  registerStep2: (data: Step2Data) =>
    axios.post("/api/auth/registration-step-2", data, {
      headers: { "Content-Type": "application/json" },
      withCredentials: true,
    }),
  registerStep3: (data: Step3Data) =>
    axios.post("/api/auth/registration-step-3", data, {
      headers: { "Content-Type": "application/json" },
      withCredentials: true,
    }),
};

const Agent = {
  Auth,
};

export default Agent;
