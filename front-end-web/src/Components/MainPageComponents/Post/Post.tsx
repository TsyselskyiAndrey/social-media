import React from "react";
import { Box, IconButton, Avatar, Typography, Menu, MenuItem } from "@mui/material";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import ChatBubbleOutlineIcon from "@mui/icons-material/ChatBubbleOutline";
import SendIcon from "@mui/icons-material/Send";
import MediaCarousel from "./Carousel/MediaCarousel";
import Like from "./Like/Like";
import Comments from "./Comment/Comment";
import Bookmark from "./Bookmark/Bookmark";
import { Post as PostType } from "../../../API/agent";
import Agent from "../../../API/agent";
import EditPostForm from "./../EditPost/EditPostForm";

interface PostProps {
  post: PostType;
}

const Post: React.FC<PostProps> = ({ post }) => {
  const [likesCount, setLikesCount] = React.useState(post.likes);
  const [isSaved, setIsSaved] = React.useState(post.isSaved);
  const [isLiked, setIsLiked] = React.useState(post.isLiked);
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const [showComments, setShowComments] = React.useState(false);
  const [isEditing, setIsEditing] = React.useState(false);
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

  const handleDeletePost = async () => {
    try {
      await Agent.Posts.deletePost(post.id);
      alert("Пост успішно видалено");
    }
    catch (error) {
      console.error("Помилка при видаленні поста", error);
    }
  }

  const handleLike = async () => {
    try {
      const result = await Agent.Posts.likePost(post.id);
      setIsLiked(result.data);
      setLikesCount((prev) => prev + (result ? 1 : -1));
      return result.data;
    } catch (error) {
      console.error("Помилка при вподобайці поста", error);
      return isLiked;
    }
  };

  const handleSave = async () => {
    try {
      const result = await Agent.Posts.savePost(post.id);
      setIsSaved(result.data);
      return result.data;
    } catch (error) {
      console.error("Помилка збереження поста", error);
      return isSaved;
    }
  };

  const media = post.postMedias[0];

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
      {isEditing && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
          <div className="bg-white rounded-lg shadow-lg max-h-[90vh] overflow-y-auto">
            <EditPostForm
              post={post}
              onCancel={() => setIsEditing(false)}
              onUpdated={() => {
                setIsEditing(false);
              }}
            />
          </div>
        </div>
      )}

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
            transition: "all 0.3s ease",
            "&:hover": {
              transform: "scale(1.15) rotate(90deg)",
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
              boxShadow: "0px 5px 15px rgba(0,0,0,0.2)",
              mt: 1.5,
              overflow: "visible",
              "&:before": {
                content: '""',
                display: "block",
                position: "absolute",
                top: 0,
                right: 14,
                width: 10,
                height: 10,
                bgcolor: "background.paper",
                transform: "translateY(-50%) rotate(45deg)",
                zIndex: 0,
              },
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

      {post.postMedias && post.postMedias.length > 0 && (
        <MediaCarousel medias={post.postMedias} />
      )}

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
