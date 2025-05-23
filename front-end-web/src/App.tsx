import { createBrowserRouter, createRoutesFromElements, Route, RouterProvider } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import useAxiosWithToken from "./Hooks/useAxiosWithToken";
import NotFound from "./Pages/NotFoundPage/NotFoundPage";
import MainPage from "./Pages/MainPage/MainPage";
import "./App.module.css";
import LoginPage from "./Pages/Authentication/LoginPage/LoginPage";
import SignupPage from "./Pages/Authentication/SignupPage/SignupPage";
import ProtectedRoutes from "./Utils/ProtectedRoutes";
import ForgotPasswordPage from "./Pages/Authentication/ForgotPasswordPage/ForgotPasswordPage";
import ResetPasswordPage from "./Pages/Authentication/ResetPasswordPage/ResetPasswordPage";
import ProfilePage from "./Pages/ProfilePage/ProfilePage";
import SettingsPage from "./Pages/Settings/Settings";
import InterfaceSettingsPage from "./Pages/Settings/InterfaceSettingsPage";
import { ThemeProviderCustom } from "./Contexts/ThemeContext";
import { CssBaseline } from "@mui/material";

function App() {
  useAxiosWithToken();


  const router = createBrowserRouter(
    createRoutesFromElements(
      <Route path="/">
        <Route path="login" element={<LoginPage />} />
        <Route path="signup" element={<SignupPage />} />
        <Route path="forgotpassword" element={<ForgotPasswordPage />} />
        <Route path="resetpassword" element={<ResetPasswordPage />} />
  
          <Route path="mainpage" element={<MainPage />} />
          <Route path="profile" element={<ProfilePage />} />
        <Route path="*" element={<NotFound />}></Route>
        <Route path="settings" element={<SettingsPage />}></Route>
        <Route path="settings/interface" element={<InterfaceSettingsPage />}></Route>
      </Route>
    )
  );

  return (
    <ThemeProviderCustom>
      <RouterProvider router={router} />
      <ToastContainer position="top-right" autoClose={3000} />
    </ThemeProviderCustom>
  );
}

export default App;
