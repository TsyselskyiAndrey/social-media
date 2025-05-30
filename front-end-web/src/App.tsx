import React from 'react';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { createBrowserRouter, createRoutesFromElements, Route, RouterProvider } from "react-router-dom";
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
import NotificationsPage from "./Pages/Settings/NotificationsPage";
import ActivityPage from "./Pages/Settings/ActivityPage";
import { ThemeProviderCustom } from "./Contexts/ThemeContext";
import { CssBaseline } from "@mui/material";
import SubscriptionsPage from "./Pages/SubscriptionsPage/SubscriptionsPage";
import PaymentSuccessPage from "./Pages/PaymentCompletionPages/PaymentSuccessPage";
import PaymentCancelPage from "./Pages/PaymentCompletionPages/PaymentCancelPage";
import EditPage from "./Pages/EditPage/EditPage";
import Saved from "./Pages/Settings/SavedPosts";
import SearchPage from "./Pages/SearchPage/SearchPage";
import { ToastProvider } from "./Contexts/ToastContext";
import MuiThemeProvider from "./Contexts/MuiThemeProvider";

function App() {
  const interceptorsReady = useAxiosWithToken();

  const router = createBrowserRouter(
    createRoutesFromElements(
      <Route path="/">
        <Route path="login" element={<LoginPage />} />
        <Route path="signup" element={<SignupPage />} />
        <Route path="forgotpassword" element={<ForgotPasswordPage />} />
        <Route path="resetpassword" element={<ResetPasswordPage />} />
        <Route element={<ProtectedRoutes />}>
          <Route index element={<MainPage />} />
          <Route path="editpage" element={<EditPage />} />
          <Route path="search" element={<SearchPage />} />
          <Route path="subscriptions" element={<SubscriptionsPage />} />
          <Route path="payment-success" element={<PaymentSuccessPage />} />
          <Route path="payment-cancel" element={<PaymentCancelPage />} />
          <Route path="profile" element={<ProfilePage />} />
          <Route path="settings" element={<SettingsPage />} />
          <Route path="settings/interface" element={<InterfaceSettingsPage />} />
          <Route path="settings/activity" element={<ActivityPage />} />
          <Route path="settings/notifications" element={<NotificationsPage />} />
          <Route path="settings/saved" element={<Saved />} />
       </Route>
        <Route path="*" element={<NotFound />} />
      </Route>
    )
  );

  if (!interceptorsReady) return null;

  return (
    <ThemeProviderCustom>
      <ToastProvider>
        <RouterProvider router={router} />
        
        <ToastContainer
          position="top-right"
          autoClose={3000}
          hideProgressBar={false}
          newestOnTop={false}
          closeOnClick
          rtl={false}
          pauseOnFocusLoss
          draggable
          pauseOnHover
          theme="light"
        />
      </ToastProvider>
    </ThemeProviderCustom>
  );
}

export default App;
