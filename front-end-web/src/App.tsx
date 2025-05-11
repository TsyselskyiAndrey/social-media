import { createBrowserRouter, createRoutesFromElements, Route, RouterProvider } from "react-router-dom";
import useAxiosWithToken from "./Hooks/useAxiosWithToken";
import NotFound from "./Pages/NotFoundPage/NotFoundPage";
import MainPage from "./Pages/MainPage/MainPage";
import "./App.css";
import LoginPage from "./Pages/Authentication/LoginPage/LoginPage";
import SignupPage from "./Pages/Authentication/SignupPage/SignupPage";
import ProtectedRoutes from "./Utils/ProtectedRoutes";

function App() {
  useAxiosWithToken();

  const router = createBrowserRouter(
    createRoutesFromElements(
      <Route path="/">
        <Route path="login" element={<LoginPage />} />
        <Route path="signup" element={<SignupPage />} />
        <Route element={<ProtectedRoutes />}>
          <Route index element={<MainPage />} />
        </Route>
        <Route path="*" element={<NotFound />}></Route>
      </Route>
    )
  );

  return <RouterProvider router={router}></RouterProvider>;
}

export default App;
