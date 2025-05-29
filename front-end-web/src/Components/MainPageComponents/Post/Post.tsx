import React from "react";
import { Box, IconButton, Avatar, Typography, Menu, MenuItem } from "@mui/material";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import ChatBubbleOutlineIcon from "@mui/icons-material/ChatBubbleOutline";
import SendIcon from "@mui/icons-material/Send";

import Like from "./Like/Like";
import Comments from "./Comment/Comment";
import Bookmark from "./Bookmark/Bookmark";
import { Post as PostType} from "../../../API/agent";
import Agent from "../../../API/agent";

interface PostProps {
  post: PostType;
}

const Post: React.FC<PostProps> = ({ post }) => {
  const [likesCount, setLikesCount] = React.useState(post.likes);
  const [isSaved, setIsSaved] = React.useState(post.isSaved);
  const [isLiked, setIsLiked] = React.useState(post.isLiked);
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const [showComments, setShowComments] = React.useState(false);

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
        <IconButton onClick={handleMenuClick} aria-label="settings" size="small">
          <MoreVertIcon />
        </IconButton>
        <Menu
          anchorEl={anchorEl}
          open={Boolean(anchorEl)}
          onClose={handleMenuClose}
          anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
          transformOrigin={{ vertical: "top", horizontal: "right" }}
        >
          <MenuItem onClick={() => alert("Редагувати пост")}>Редагувати</MenuItem>
          <MenuItem onClick={() => alert("Видалити пост")}>Видалити</MenuItem>
        </Menu>
      </Box>

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
            initialCount={likesCount}
            initiallyLiked={isLiked}
            onLike={handleLike}
          />

          <IconButton aria-label="comment" onClick={handleCommentIconClick} size="small">
            <ChatBubbleOutlineIcon />
          </IconButton>
          <IconButton aria-label="share" size="small">
            <SendIcon />
          </IconButton>
        </Box>
        <Bookmark
          initiallySaved={isSaved}
          onSave={handleSave}
        />
      </Box>

      <Box sx={{ px: 2, pb: 1 }}>
        <Typography component="span">{post.caption}</Typography>
      </Box>

      <Box sx={{ px: 3, pb: 2 }}>
        {showComments && (
          <Comments initialComments={[]} showCommentInput={showComments} />
        )}
      </Box>
    </Box>
  );
};

export default Post;