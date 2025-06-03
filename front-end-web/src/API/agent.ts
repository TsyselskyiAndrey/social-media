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
  confirmpassword: string;
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

interface CreatePostRequest {
  caption: string;
  tags: string[];
  postMedias: File[];
  thumbnail: File | null;
}

interface UpdatePostRequest {
  id: number;
  caption: string;
  tags: string[];
  postMedias: File[];
  thumbnail: File | null;
}

export interface Tag {
  id: number;
  name: string;
}

export interface UserProfileInfo {
  firstName: string;
  lastName: string;
  email: string;
  userName: string;
  biography: string | null;
  profileImagePath: string | null;
  birthDate: Date | null;
  followed: number;
  followers: number;
  postsAmount: number;
}

export interface Post {
  id: number;
  authorName: string;
  authorIconUrl: string;
  caption: string | null;
  postType: string;
  tags: string[];
  likes: number;
  views: number;
  isLiked: boolean;
  isSaved: boolean;
  isUninteresting: boolean;
  postMedias: PostMedia[];
}

export interface PostMedia {
  id: number;
  mediaUrl: string;
  postMediaType: string;
  thumbnailUrl: string | null;
  duration: number | null;
  format: string;
  size: number;
  isUploaded: boolean;
  position: number;
}

export interface CommentUser {
  id: number;
  userName: string;
  profileImageUrl: string | null;
}

export interface Comment {
  id: number;
  author: CommentUser;
  content: string;
  childComments: Comment[];
  isLiked: boolean;
  likes: number;
}

export interface CreateCommentRequest {
  postId: number;
  content: string;
  parentCommentId?: number | null;
}

export interface EditCommentRequest {
  commentId: number;
  content: string;
}

export interface DeleteCommentRequest {
  commentId: number;
}

export interface LikeCommentRequest {
  commentId: number;
}

export interface UpdateUserProfileInfo {
  firstName: string;
  lastName: string;
  birthday: Date;
  username: string;
  biography: string | null;
  profilePhoto: File | null;
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

  logout: () => axiosWithToken.post("/api/auth/logout", {}, { withCredentials: true }),
};

const Downloads = {
  getAndroidInstaller: async () => {
    return await axiosWithToken.get<string>("/api/downloads/getAndroidInstaller", {
      withCredentials: true,
    });
  },
};

const Payment = {
  getConfig: async () => {
    return await axiosWithToken.get<string>("/api/subscription/config", {
      withCredentials: true,
    });
  },
  getAllPlans: async () => {
    return await axiosWithToken.get("/api/subscription/availableSubscriptions", {
      withCredentials: true,
    });
  },
  createCheckoutSession: (body: { priceId: string }) =>
    axiosWithToken
      .post("/api/subscription/checkoutSession", body, {
        headers: { "Content-Type": "application/json" },
        withCredentials: true,
      })
      .then((res) => res.data),
  upgradeSubscription: (body: { priceId: string }) =>
    axiosWithToken
      .post("/api/subscription/upgrade", body, {
        headers: { "Content-Type": "application/json" },
        withCredentials: true,
      })
      .then((res) => res.data),
  cancelSubscription: (body: { priceId: string }) =>
    axiosWithToken.post("/api/subscription/cancel", body, {
      headers: { "Content-Type": "application/json" },
      withCredentials: true,
    }),
};

const Posts = {
  getPosts: async (postTitle: string | null, postAmount: number, postId: number | null, tags: number[] | null, userId: number | null) => {
    const params: any = { postAmount };

    if (postTitle !== null) params.postTitle = postTitle;
    if (postId !== null) params.postId = postId;
    if (tags !== null) params.tags = tags;
    if (userId !== null) params.userId = userId;

    return await axiosWithToken.get<Post[]>("/api/Post/getPosts", {
      params,
      withCredentials: true,
    });
  },

  getSavedPosts: async () => {
    return await axiosWithToken.get<Post[]>("/api/Post/getSavedPosts", {
      withCredentials: true,
    });
  },

  createPost: async ({ caption, tags, postMedias, thumbnail }: CreatePostRequest) => {
    const formData = new FormData();
    formData.append("Caption", caption);
    tags.forEach((tag, index) => formData.append(`Tags[${index}]`, tag));
    postMedias.forEach((file) => formData.append("PostMedias", file));
    if (thumbnail) formData.append("Thumbnail", thumbnail);

    return await axiosWithToken.post("/api/Post/createPost", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
      withCredentials: true,
    });
  },

  updatePost: async (postData: UpdatePostRequest) => {
    const formData = new FormData();
    formData.append("Id", postData.id.toString());
    formData.append("Caption", postData.caption ?? "");

    postData.tags.forEach((tag) => {
      formData.append("Tags", tag);
    });

    if (postData.thumbnail) {
      formData.append("Thumbnail", postData.thumbnail);
    }

    postData.postMedias.forEach((file) => {
      formData.append("PostMedias", file);
    });

    return await axiosWithToken.put("/api/post/updatePost", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
      withCredentials: true,
    });
  },

  deletePost: async (postId: number) => {
    return await axiosWithToken.delete(`/api/post/deletePost/${postId}`, {
      headers: { "Content-Type": "application/json" },
      withCredentials: true,
    });
  },

  likePost: async (postId: number) => {
    return await axiosWithToken.post<boolean>(
      `/api/post/like`,
      { PostId: postId },
      {
        headers: { "Content-Type": "application/json" },
        withCredentials: true,
      }
    );
  },

  savePost: async (postId: number) => {
    return await axiosWithToken.post<boolean>(
      `/api/post/save`,
      { PostId: postId },
      {
        headers: { "Content-Type": "application/json" },
        withCredentials: true,
      }
    );
  },
};

const Comments = {
  getComments: async (postId: number) => {
    return await axiosWithToken.get<Comment[]>("/api/comment/getComments", {
      params: { postId },
      withCredentials: true,
    });
  },

  createComment: async (data: CreateCommentRequest) => {
    const formData = new FormData();
    formData.append("Content", data.content);
    formData.append("PostId", data.postId.toString());
    if (data.parentCommentId !== undefined && data.parentCommentId !== null) {
      formData.append("ParentCommentId", data.parentCommentId.toString());
    }

    const response = await axiosWithToken.post("/api/comment/createComment", formData, {
      headers: { "Content-Type": "multipart/form-data" },
      withCredentials: true,
    });

    return response.data as Comment;
  },

  editComment: async (data: EditCommentRequest) => {
    return await axiosWithToken.patch("/api/comment/editComment", data, {
      headers: { "Content-Type": "application/json" },
      withCredentials: true,
    });
  },

  deleteComment: async (data: DeleteCommentRequest) => {
    return await axiosWithToken.delete("/api/comment/deleteComment", {
      data,
      headers: { "Content-Type": "application/json" },
      withCredentials: true,
    });
  },

  likeComment: async (data: LikeCommentRequest) => {
    return await axiosWithToken.patch<boolean>("/api/comment/likeComment", data, {
      headers: { "Content-Type": "application/json" },
      withCredentials: true,
    });
  },
};

const Tags = {
  getAllTags: async () => {
    return await axiosWithToken.get<Tag[]>("/api/post/getAllTags");
  },
};

const User = {
  getUserProfileInfo: async () => {
    return await axiosWithToken.get<UserProfileInfo>("/api/user/getUserProfileInfo");
  },

  updateUserProfileInfo: async (data: UpdateUserProfileInfo) => {
    const formData = new FormData();
    formData.append("FirstName", data.firstName);
    formData.append("LastName", data.lastName);
    formData.append("Birthday", data.birthday.toISOString());
    formData.append("Username", data.username);
    if (data.biography) formData.append("Biography", data.biography);
    if (data.profilePhoto) formData.append("ProfilePhoto", data.profilePhoto);

    return await axiosWithToken.put("/api/user/updateUserProfileInfo", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
      withCredentials: true,
    });
  },
};

const Agent = {
  Auth,
  Payment,
  Downloads,
  Posts,
  Comments,
  Tags,
  User,
};

export default Agent;
