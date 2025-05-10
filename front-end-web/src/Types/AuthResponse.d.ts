import { User } from "./user";

export interface AuthResponse extends User {
  token: string;
}
