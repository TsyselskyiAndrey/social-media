import {
  createContext,
  useState,
  useEffect,
  ReactNode,
  Dispatch,
  SetStateAction,
} from "react";
import useRefreshToken from "../Hooks/useRefreshToken";
import { User } from "../Types/user";

interface Props {
  children: ReactNode;
}

export interface AuthContextType {
  isLoading: boolean;
  setIsLoading: Dispatch<SetStateAction<boolean>>;
  auth: User | null;
  setAuth: Dispatch<SetStateAction<User | null>>;
}

export const IsAuthenticatedContext = createContext<
  AuthContextType | undefined
>(undefined);

export function IsAuthenticatedProvider({ children }: Props) {
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [auth, setAuth] = useState<User | null>(null);
  const refresh = useRefreshToken();

  useEffect(() => {
    const checkAuth = async () => {
      setIsLoading(true);
      const accessToken = localStorage.getItem("accessToken");

      if (!accessToken) {
        setIsLoading(false);
        setAuth(null);
        return;
      }
      try {
        const newData = await refresh();
        const newAccessToken = newData?.token;
        if (newAccessToken) {
          const { token, ...user } = newData;
          setAuth(user);
        } else {
          setAuth(null);
        }
      } catch (error) {
        console.error("Error during auth check:", error);
        setAuth(null);
      } finally {
        setIsLoading(false);
      }
    };
    checkAuth();
  }, [refresh]);

  const contextValue: AuthContextType = {
    isLoading,
    setIsLoading,
    auth,
    setAuth,
  };

  return (
    <IsAuthenticatedContext.Provider value={contextValue}>
      {children}
    </IsAuthenticatedContext.Provider>
  );
}
