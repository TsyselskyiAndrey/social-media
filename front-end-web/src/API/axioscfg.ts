import axios from "axios";
const BASE_URL = "https://glowee-server-dbaeh4f7hcftd4dq.northeurope-01.azurewebsites.net";

export default axios.create({
  baseURL: BASE_URL,
});

export const axiosWithToken = axios.create({
  baseURL: BASE_URL,
});
