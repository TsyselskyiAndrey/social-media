import axios, { axiosWithToken } from "./axioscfg";
import { AuthResponse } from "../Types/AuthResponse";

interface LoginData {
  login: string;
  password: string;
  deviceId: string;
}

interface GoogleAuthData {
  codeOrIdToken: string;
  deviceId: string;
  isMobile: boolean;
}

interface FacebookAuthData {
  accessToken: string;
  deviceId: string;
}

interface Step1Data {
  email: string;
  username: string;
  password: string;
  rePassword: string;
}

interface Step2Data {
  firstname: string;
  lastname: string;
  birthdate: string;
}

interface Step3Data {
  code: string;
}

interface UploadAvatarResponse {
  profilePictureUrl: string;
}

interface ForgotPasswordData {
  email: string;
  clientUri: string;
}

interface ResetPasswordData {
  password: string;
  confirmpassword: string;
  email: string | null;
  token: string | null;
}

interface CreatePostRequest{
  caption: string;
  tags: string[];
  postMedias: File[];
  thumbnail: File | null;
}

export interface Tag{
  id: number;
  name: string;
}

export interface UserProfileInfo{
  FirstName : string;
  LastName : string;
  Email : string;
  UserName : string;
  Biography : string | null;
  ProfileImagePath : string | null;
  BirthDate : Date | null;
}

const Auth = {
  login: (loginData: LoginData) =>
    axios.post<AuthResponse>("/api/auth/login", loginData, {
      headers: { "Content-Type": "application/json" },
      withCredentials: true,
    }),

  googleLogin: (data: GoogleAuthData) =>
    axios.post<AuthResponse>("/api/auth/google-login", data, {
      headers: { "Content-Type": "application/json" },
      withCredentials: true,
    }),

  facebookLogin: (data: FacebookAuthData) =>
    axios.post<AuthResponse>("/api/auth/facebook-login", data, {
      headers: { "Content-Type": "application/json" },
      withCredentials: true,
    }),

  refreshToken: (accessToken: string) =>
    axios.post<AuthResponse>(
      "/api/auth/refresh-token",
      { accessToken },
      {
        headers: { "Content-Type": "application/json" },
        withCredentials: true,
      }
    ),

  registerStep1: (data: Step1Data) =>
    axios.post("/api/auth/registration-step-1", data, {
      headers: { "Content-Type": "application/json" },
      withCredentials: true,
    }),

  registerStep2: (data: Step2Data) =>
    axios.post("/api/auth/registration-step-2", data, {
      headers: { "Content-Type": "application/json" },
      withCredentials: true,
    }),

  registerStep3: (data: Step3Data) =>
    axios.post("/api/auth/registration-step-3", data, {
      headers: { "Content-Type": "application/json" },
      withCredentials: true,
    }),

  uploadAvatar: (file: File) => {
    const formData = new FormData();
    formData.append("file", file);

    return axios.post<UploadAvatarResponse>("/api/auth/upload-profile-image", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
      withCredentials: true,
    });
  },

  forgotPassword: (forgotPasswordData: ForgotPasswordData) =>
    axios.post("/api/auth/forgotpassword", forgotPasswordData, {
      headers: { "Content-Type": "application/json" },
      withCredentials: true,
    }),

  resetPassword: (resetPasswordData: ResetPasswordData) =>
    axios.post("/api/auth/resetpassword", resetPasswordData, {
      headers: { "Content-Type": "application/json" },
      withCredentials: true,
    }),
};

const Posts = {
  getPosts: () => {

  },
  createPost: async ({ caption, tags, postMedias, thumbnail }: CreatePostRequest) => {
    const formData = new FormData();
    formData.append('Caption', caption);
    tags.forEach((tag, index) => formData.append(`Tags[${index}]`, tag));
    postMedias.forEach(file => formData.append('PostMedias', file));
    if (thumbnail) formData.append('Thumbnail', thumbnail);

    return await axiosWithToken.post("/api/post/createPost", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
      withCredentials: true,
    });
  }
}

const Tags = {
  getAllTags: async () =>{
    return await axiosWithToken.get<Tag[]>("/api/post/getAllTags");
  }
}

const User = {
  getUserProfileInfo: async () => {
    return await axiosWithToken.get<UserProfileInfo>("/api/user/getUserProfileInfo");
  }
}

const Agent = {
  Auth,
  Posts,
  Tags,
  User,
};

export default Agent;
