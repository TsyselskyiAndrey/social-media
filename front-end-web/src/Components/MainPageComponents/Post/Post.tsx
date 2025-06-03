import React from "react";
import { Box, IconButton, Avatar, Typography, Menu, MenuItem } from "@mui/material";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import ChatBubbleOutlineIcon from "@mui/icons-material/ChatBubbleOutline";
import SendIcon from "@mui/icons-material/Send";

import Like from "./Like/Like";
import Comments from "./Comment/Comment";
import Bookmark from "./Bookmark/Bookmark";
import { Post as PostType } from "../../../API/agent";
import Agent from "../../../API/agent";
import ArrowBackIosNewIcon from "@mui/icons-material/ArrowBackIosNew";
import ArrowForwardIosIcon from "@mui/icons-material/ArrowForwardIos";

interface PostProps {
  post: PostType;
}

const Post: React.FC<PostProps> = ({ post }) => {
  const [likesCount, setLikesCount] = React.useState(post.likes);
  const [isSaved, setIsSaved] = React.useState(post.isSaved);
  const [isLiked, setIsLiked] = React.useState(post.isLiked);
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const [showComments, setShowComments] = React.useState(false);
  const [currentMediaIndex, setCurrentMediaIndex] = React.useState(0);

  const handleMenuClick = (event: React.MouseEvent<HTMLElement>) => {
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
      setIsLiked(result.data);
      setLikesCount((prev) => prev + (result ? 1 : -1));
      return result.data;
    } catch (error) {
      console.error("Ошибка при лайке поста", error);
      return isLiked;
    }
  };

  const handleSave = async () => {
    try {
      const result = await Agent.Posts.savePost(post.id);
      setIsSaved(result.data);
      return result.data;
    } catch (error) {
      console.error("Ошибка при сохранении поста", error);
      return isSaved;
    }
  };

  // Додати функції для навігації між медіа
  const handlePrevMedia = () => {
    if (currentMediaIndex > 0) {
      setCurrentMediaIndex(prev => prev - 1);
    }
  };
  
  const handleNextMedia = () => {
    if (currentMediaIndex < post.postMedias.length - 1) {
      setCurrentMediaIndex(prev => prev + 1);
    }
  };
  
  // Замінити const media = post.postMedias[0]; на:
  const media = post.postMedias[currentMediaIndex];
  const hasMultipleMedia = post.postMedias.length > 1;

  return (
    <Box
      sx={{
        width: 450,
        mx: "auto",
        bgcolor: "background.paper",
        borderRadius: 2,
        boxShadow: 3,
        overflow: "hidden",
        display: "flex",
        flexDirection: "column",
      }}
    >
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
          <Avatar src={post.authorIconUrl} />
          <Typography fontWeight="bold">{post.authorName}</Typography>
        </Box>
        <IconButton
          onClick={handleMenuClick}
          aria-label="settings"
          size="small"
          sx={{
            transition: "transform 0.2s",
            "&:hover": {
              transform: "scale(1.1)",
              color: "primary.main",
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
            "& .MuiPaper-root": {
              borderRadius: 2,
              minWidth: 180,
              boxShadow: "0px 5px 15px rgba(0,0,0,0.15)",
              mt: 1.5,
              "& .MuiMenu-list": {
                padding: "8px 0",
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
              fontWeight: "bold",
              fontSize: "0.95rem",
              py: 1.2,
              "&:hover": {
                bgcolor: "action.hover",
                transition: "all 0.2s",
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
              fontWeight: "bold",
              fontSize: "0.95rem",
              py: 1.2,
              color: "error.main",
              "&:hover": {
                bgcolor: "error.light",
                color: "error.dark",
                transition: "all 0.2s",
              },
            }}
          >
            Видалити
          </MenuItem>
        </Menu>
      </Box>

      {/* Замінюємо блок з відображенням медіа на карусель */}
      <div className="relative">
        {media && media.postMediaType === "Photo" && (
          <img 
            src={media.mediaUrl} 
            alt="Post" 
            style={{ width: "100%", height: "auto", objectFit: "cover" }} 
          />
        )}
      
        {media && media.postMediaType === "Video" && (
          <video 
            controls 
            poster={media.thumbnailUrl ?? undefined} 
            src={media.mediaUrl} 
            style={{ width: "100%", height: "auto", objectFit: "cover" }} 
          />
        )}
        
        {hasMultipleMedia && (
          <div className="absolute bottom-4 left-0 right-0 flex justify-center gap-1">
            {post.postMedias.map((_, index) => (
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
        
        {hasMultipleMedia && currentMediaIndex < post.postMedias.length - 1 && (
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
          <Like initialCount={likesCount} initiallyLiked={isLiked} onLike={handleLike} />

          <IconButton aria-label="comment" onClick={handleCommentIconClick} size="small">
            <ChatBubbleOutlineIcon />
          </IconButton>
          <IconButton aria-label="share" size="small">
            <SendIcon />
          </IconButton>
        </Box>
        <Bookmark initiallySaved={isSaved} onSave={handleSave} />
      </Box>

      <Box sx={{ px: 2, pb: 1 }}>
        <Typography component="span">{post.caption}</Typography>
      </Box>

      <Box sx={{ px: 3, pb: 2 }}>
        {showComments && (
          <Comments postId={post.id} showCommentInput={showComments} />
        )}
      </Box>
    </Box>
  );
};

export default Post;
