import { createBrowserRouter, createRoutesFromElements, Route, RouterProvider } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import useAxiosWithToken from "./Hooks/useAxiosWithToken";
import NotFound from "./Pages/NotFoundPage/NotFoundPage";
import MainPage from "./Pages/MainPage/MainPage";
import "./App.css";
import LoginPage from "./Pages/Authentication/LoginPage/LoginPage";
import SignupPage from "./Pages/Authentication/SignupPage/SignupPage";
import ProtectedRoutes from "./Utils/ProtectedRoutes";
import ForgotPasswordPage from "./Pages/Authentication/ForgotPasswordPage/ForgotPasswordPage";
import ResetPasswordPage from "./Pages/Authentication/ResetPasswordPage/ResetPasswordPage";

function App() {
  useAxiosWithToken();

  const router = createBrowserRouter(
    createRoutesFromElements(
      <Route path="/">
        <Route path="login" element={<LoginPage />} />
        <Route path="signup" element={<SignupPage />} />
        <Route path="forgotpassword" element={<ForgotPasswordPage />} />
        <Route path="resetpassword" element={<ResetPasswordPage />} />
        <Route element={<ProtectedRoutes />}>
          <Route index element={<MainPage />} />
        </Route>
        <Route path="*" element={<NotFound />}></Route>
      </Route>
    )
  );

  return (
    <>
      <RouterProvider router={router} />
      <ToastContainer position="top-right" autoClose={3000} />
    </>
  );
}

export default App;
