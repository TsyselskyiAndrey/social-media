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
import AutoPlayVideo from "./Video/AutoplayVideo";

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
      console.error("Ошибка при удалении поста", error);
    }
  }

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
          onClick={() => {
              setIsEditing(true);
              handleMenuClose();
            }}
          >
            Редагувати
          </MenuItem>

          <MenuItem onClick={async () => await handleDeletePost() }>Видалити</MenuItem>
        </Menu>
      </Box>

      {post.postMedias[0] && post.postMedias[0].postMediaType === "Photo" && (
        <img
          src={post.postMedias[0].mediaUrl}
          alt="Post"
          style={{ width: "100%", height: "auto", objectFit: "cover" }}
        />
      )}

      {post.postMedias[0] && post.postMedias[0].postMediaType === "Video" && (
        <AutoPlayVideo
          src={post.postMedias[0].mediaUrl}
          poster={post.postMedias[0].thumbnailUrl ?? undefined}
          controls
          style={{ width: "100%", height: "auto", objectFit: "cover" }}
        />
      )}

      {post.postType === "Carousel" && post.postMedias.length > 0 && (
        <Box sx={{ width: "100%", position: "relative" }}>
          <MediaCarousel medias={post.postMedias} />
        </Box>
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
