import { useContext } from "react";
import { AuthContextType, IsAuthenticatedContext } from "../Contexts/IsAuthenticatedContext";


const useAuth = (): AuthContextType => {
  const context = useContext(IsAuthenticatedContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};

export default useAuth;
