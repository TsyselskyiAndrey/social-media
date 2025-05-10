import { createBrowserRouter, createRoutesFromElements, Route, RouterProvider } from "react-router-dom";
import useAxiosWithToken from "./Hooks/useAxiosWithToken";
import NotFound from "./Pages/NotFoundPage/NotFoundPage";
import MainPage from "./Pages/MainPage/MainPage";
import "./App.css";
import LoginPage from "./Pages/AuthPage/LoginPage/LoginPage";
import SignupPage from "./Pages/AuthPage/SignupPage/SignupPage";

function App() {
  useAxiosWithToken();

  const router = createBrowserRouter(
    createRoutesFromElements(
      <Route path="/">
        <Route path="login" element={<LoginPage />} />
        <Route path="signup" element={<SignupPage />} />
        <Route index element={<MainPage />} />
        <Route path="*" element={<NotFound />}></Route>
      </Route>
    )
  );

  return <RouterProvider router={router}></RouterProvider>;
}

export default App;
