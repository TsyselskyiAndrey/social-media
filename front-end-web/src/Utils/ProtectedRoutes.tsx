import { Outlet, Navigate } from "react-router-dom";
import useAuth from "../Hooks/useAuth";

const ProtectedRoutes = () => {
  const { auth, isLoading } = useAuth();
  if (isLoading) {
    return <></>;
  }
  if (!auth) {
    return <Navigate to="/login" replace={true}></Navigate>;
  } else {
    return <Outlet />;
  }
};

export default ProtectedRoutes;
