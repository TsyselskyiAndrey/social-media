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
      // Додаємо виклик функції оновлення поста в списку
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
              transition: 'transform 0.2s',
              '&:hover': {
                transform: 'scale(1.1)',
                color: 'primary.main'
              }
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
                boxShadow: '0px 5px 15px rgba(0,0,0,0.15)',
                mt: 1.5,
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
              onClick={() => alert("Редагувати пост")}
              sx={{
                mx: 1,
                borderRadius: 1,
                fontWeight: 'bold',
                fontSize: '0.95rem',
                py: 1.2,
                '&:hover': {
                  bgcolor: 'action.hover',
                  transition: 'all 0.2s',
                },
              }}
            >
              Редагувати
            </MenuItem>
            <MenuItem 
              onClick={() => alert("Видалити пост")}
              sx={{
                mx: 1,
                borderRadius: 1,
                fontWeight: 'bold',
                fontSize: '0.95rem',
                py: 1.2,
                color: 'error.main',
                '&:hover': {
                  bgcolor: 'error.light',
                  color: 'error.dark',
                  transition: 'all 0.2s',
                },
              }}
            >
              Видалити
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
