export interface User {
  id: number;
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  profileImageUrl?: string;
  roles: string[];
}
