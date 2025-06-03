import React, { useState } from "react";
import { Dialog, DialogPanel } from "@headlessui/react";
import { Post } from "../../../API/agent";
import { Box, IconButton, Avatar, Typography, Menu, MenuItem } from "@mui/material";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import ChatBubbleOutlineIcon from "@mui/icons-material/ChatBubbleOutline";
import SendIcon from "@mui/icons-material/Send";
import ArrowBackIosNewIcon from "@mui/icons-material/ArrowBackIosNew";
import ArrowForwardIosIcon from "@mui/icons-material/ArrowForwardIos";

import Like from "../../MainPageComponents/Post/Like/Like";
import Comments from "../../MainPageComponents/Post/Comment/Comment";
import Bookmark from "../../MainPageComponents/Post/Bookmark/Bookmark";
import Agent from "../../../API/agent";
import EditPostForm from "../../MainPageComponents/EditPost/EditPostForm";

interface PostModalProps {
  isOpen: boolean;
  onClose: () => void;
  post: Post;
  onPostUpdate?: (updatedPost: Post) => void;
}

const PostModal: React.FC<PostModalProps> = ({ isOpen, onClose, post, onPostUpdate }) => {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [showComments, setShowComments] = useState(false);
  const [currentPost, setCurrentPost] = useState<Post>(post);
  const [currentMediaIndex, setCurrentMediaIndex] = useState(0);
  const [isEditing, setIsEditing] = useState(false);

  const handleMenuClick = (event: React.MouseEvent<HTMLElement>) => {
    event.stopPropagation();
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleCommentIconClick = () => {
    setShowComments((prev) => !prev);
  };

  const handleDeletePost = async () => {
    try {
      await Agent.Posts.deletePost(post.id);
      onClose();
      if (onPostUpdate) {
        onPostUpdate({...post});
      }
    } catch (error) {
      console.error("Помилка при видаленні поста", error);
      alert("Не вдалося видалити пост.");
    }
  };

  const handleLike = async () => {
    try {
      const result = await Agent.Posts.likePost(post.id);
      setCurrentPost(prev => ({
        ...prev,
        isLiked: result.data,
        likes: prev.likes + (result.data ? 1 : -1)
      }));
      return result.data;
    } catch (error) {
      console.error("Помилка при лайку поста", error);
      return currentPost.isLiked;
    }
  };

  const handleSave = async () => {
    try {
      const result = await Agent.Posts.savePost(post.id);
      const updatedPost = {
        ...currentPost,
        isSaved: result.data
      };
      setCurrentPost(updatedPost);
      if (onPostUpdate) {
        onPostUpdate(updatedPost);
      }
      return result.data;
    } catch (error) {
      console.error("Помилка при збереженні поста", error);
      return currentPost.isSaved;
    }
  };

  const handlePrevMedia = () => {
    if (currentMediaIndex > 0) {
      setCurrentMediaIndex(prev => prev - 1);
    }
  };

  const handleNextMedia = () => {
    if (currentMediaIndex < currentPost.postMedias.length - 1) {
      setCurrentMediaIndex(prev => prev + 1);
    }
  };

  const media = currentPost.postMedias[currentMediaIndex];
  const isVideo = media?.postMediaType.startsWith("Video");
  const hasMultipleMedia = currentPost.postMedias.length > 1;

  return (
    <Dialog
      open={isOpen}
      onClose={onClose}
      className="fixed inset-0 z-50 flex items-center justify-center p-4"
    >
      <div className="fixed inset-0 bg-black/60" aria-hidden="true" />

      {isEditing && (
        <div className="fixed inset-0 z-[60] flex items-center justify-center bg-black bg-opacity-50">
          <div className="bg-white dark:bg-gray-800 rounded-lg shadow-lg max-h-[90vh] overflow-y-auto">
            <EditPostForm
              post={currentPost}
              onCancel={() => setIsEditing(false)}
              onUpdated={() => {
                setIsEditing(false);
                if (onPostUpdate) {
                  onClose();
                }
              }}
            />
          </div>
        </div>
      )}

      <DialogPanel className="relative bg-white dark:bg-gray-800 rounded-lg shadow-lg max-w-md w-full max-h-[90vh] flex flex-col overflow-hidden">
        <button
          onClick={onClose}
          className="absolute top-1 right-2 text-red-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-white text-3xl font-bold leading-none z-10"
          aria-label="Close modal"
        >
          &times;
        </button>

        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
            p: 2,
            borderBottom: "1px solid",
            borderColor: "divider",
          }}
        >
          <Box sx={{ display: "flex", alignItems: "center", gap: 2 }}>
            <Avatar src={currentPost.authorIconUrl} />
            <Typography fontWeight="bold">{currentPost.authorName}</Typography>
          </Box>
          <IconButton 
            onClick={handleMenuClick} 
            aria-label="settings" 
            size="small"
            sx={{
              transition: 'all 0.3s ease',
              '&:hover': {
                transform: 'scale(1.15) rotate(90deg)',
                color: 'primary.main',
              },
            }}
          >
            <MoreVertIcon />
          </IconButton>
          <Menu
            anchorEl={anchorEl}
            open={Boolean(anchorEl)}
            onClose={handleMenuClose}
            anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
            transformOrigin={{ vertical: "top", horizontal: "right" }}
            sx={{
              '& .MuiPaper-root': {
                borderRadius: 2,
                minWidth: 180,
                boxShadow: '0px 5px 15px rgba(0,0,0,0.2)',
                mt: 1.5,
                overflow: 'visible',
                '&:before': {
                  content: '""',
                  display: 'block',
                  position: 'absolute',
                  top: 0,
                  right: 14,
                  width: 10,
                  height: 10,
                  bgcolor: 'background.paper',
                  transform: 'translateY(-50%) rotate(45deg)',
                  zIndex: 0,
                },
                '& .MuiMenu-list': {
                  padding: '8px 0',
                },
              },
            }}
            TransitionProps={{
              enter: true,
              appear: true,
              timeout: 250,
            }}
          >
            <MenuItem
              onClick={() => {
                setIsEditing(true);
                handleMenuClose();
              }}
              sx={{
                transition: "all 0.2s ease",
                "&:hover": {
                  bgcolor: "primary.light",
                  color: "white",
                  pl: 2,
                },
              }}
            >
              <span className="flex items-center gap-2">
                <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                </svg>
                Редагувати
              </span>
            </MenuItem>
            <MenuItem 
              onClick={async () => {
                if (window.confirm("Ви впевнені, що хочете видалити цей пост?")) {
                  await handleDeletePost();
                }
                handleMenuClose();
              }}
              sx={{
                transition: "all 0.2s ease",
                "&:hover": {
                  bgcolor: "error.light",
                  color: "white",
                  pl: 2,
                },
              }}
            >
              <span className="flex items-center gap-2">
                <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                </svg>
                Видалити
              </span>
            </MenuItem>
          </Menu>
        </Box>

        <div className="relative">
          {isVideo ? (
            <video
              src={media.mediaUrl}
              controls
              className="w-full max-h-[50vh] object-contain bg-black"
              poster={media.thumbnailUrl ?? undefined}
            />
          ) : (
            <img
              src={media.mediaUrl}
              alt="Post"
              className="w-full max-h-[50vh] object-contain"
            />
          )}
          
          {hasMultipleMedia && (
            <div className="absolute bottom-4 left-0 right-0 flex justify-center gap-1">
              {currentPost.postMedias.map((_, index) => (
                <div 
                  key={index} 
                  className={`w-2 h-2 rounded-full ${index === currentMediaIndex ? 'bg-blue-500' : 'bg-gray-300'}`}
                  onClick={() => setCurrentMediaIndex(index)}
                />
              ))}
            </div>
          )}
          
          {hasMultipleMedia && currentMediaIndex > 0 && (
            <button
              onClick={handlePrevMedia}
              className="absolute left-2 top-1/2 transform -translate-y-1/2 bg-white/70 dark:bg-black/70 rounded-full p-1 hover:bg-white/90 dark:hover:bg-black/90 transition-colors"
            >
              <ArrowBackIosNewIcon fontSize="small" />
            </button>
          )}
          
          {hasMultipleMedia && currentMediaIndex < currentPost.postMedias.length - 1 && (
            <button
              onClick={handleNextMedia}
              className="absolute right-2 top-1/2 transform -translate-y-1/2 bg-white/70 dark:bg-black/70 rounded-full p-1 hover:bg-white/90 dark:hover:bg-black/90 transition-colors"
            >
              <ArrowForwardIosIcon fontSize="small" />
            </button>
          )}
        </div>

        <Box
          sx={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
            px: 2,
            py: 1.5,
            borderTop: "1px solid",
            borderColor: "divider",
          }}
        >
          <Box sx={{ display: "flex", alignItems: "center", gap: 1.5 }}>
            <Like
              initialCount={currentPost.likes}
              initiallyLiked={currentPost.isLiked}
              onLike={handleLike}
            />

            <IconButton 
              aria-label="comment" 
              onClick={handleCommentIconClick} 
              size="small"
            >
              <ChatBubbleOutlineIcon />
            </IconButton>
            <IconButton aria-label="share" size="small">
              <SendIcon />
            </IconButton>
          </Box>
          <Bookmark
            initiallySaved={currentPost.isSaved}
            onSave={handleSave}
          />
        </Box>

        <Box sx={{ px: 2, pb: 1 }}>
          <Typography component="span">{currentPost.caption}</Typography>
        </Box>

        <Box sx={{ px: 2, pb: 2, maxHeight: "30vh", overflowY: "auto" }}>
          {showComments && (
            <Comments postId={currentPost.id} showCommentInput={showComments} />
          )}
        </Box>
      </DialogPanel>
    </Dialog>
  );
};

export default PostModal;
