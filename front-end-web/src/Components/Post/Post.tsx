import React from "react";
import { Box, IconButton, Avatar, Typography, Menu, MenuItem } from "@mui/material";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import ChatBubbleOutlineIcon from "@mui/icons-material/ChatBubbleOutline";
import SendIcon from "@mui/icons-material/Send";

import Like from "./Like/Like";
import Comments from "./Comment/Comment";
import Bookmark from "./Bookmark/Bookmark";
import Photo1 from "../../Assets/Post/Photo1.png";

const Post: React.FC = () => {
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const [showComments, setShowComments] = React.useState(false);

  const initialComments = [{ id: 1, text: "Супер фото!" }, { id: 2, text: "Круто!" }, { id: 3, text: "Чудово!" }, { id: 4, text: "Дуже гарно!" }];

  const handleMenuClick = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleCommentIconClick = () => {
    setShowComments((prev) => !prev);
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
      {/* Верхня частина */}
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
          <Avatar sx={{ bgcolor: "gray" }}>U</Avatar>
          <Typography fontWeight="bold">user_name</Typography>
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

      {/* Фото */}
      <Box
        component="img"
        src={Photo1}
        alt="Post"
        sx={{ width: "100%", height: "auto", objectFit: "cover" }}
      />

      {/* Кнопки */}
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          px: 2,
          py: 1.5,
          borderTop: '1px solid',
          borderColor: 'divider',
        }}
      >
        <Box sx={{ display: "flex", alignItems: "center", gap: 1.5 }}>
          <Like initialCount={10} />
          <IconButton aria-label="comment" onClick={handleCommentIconClick} size="small">
            <ChatBubbleOutlineIcon />
          </IconButton>
          <IconButton aria-label="share" size="small">
            <SendIcon />
          </IconButton>
        </Box>
        <Bookmark initiallySaved={false} />
      </Box>

      {/* Підпис */}
      <Box sx={{ px: 2, pb: 1 }}>
        <Typography component="span" fontWeight="bold">
          user_name{" "}
        </Typography>
        <Typography component="span">Це приклад підпису до фото...</Typography>
      </Box>

      {/* Коментарі з більшим паддінгом */}
      <Box sx={{ px: 3, pb: 2 }}>
        {showComments && (
          <Comments
            initialComments={initialComments}
            showCommentInput={showComments}
          />
        )}
      </Box>
    </Box>
  );
};

export default Post;
