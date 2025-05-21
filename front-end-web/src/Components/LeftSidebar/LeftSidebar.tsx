import React from "react";
import { Box } from "@mui/material";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import HomeIcon from "@mui/icons-material/Home";
import SearchIcon from "@mui/icons-material/Search";
import MessageIcon from "@mui/icons-material/Message";
import logoSrc from "../../Assets/logo.png";
import MenuItems from "./MenuItems/MenuItems";
import CreateButton from "./CreateButton/CreateButton";

const LeftSidebar: React.FC = () => {
  const items = [
    { text: "Профіль", icon: <AccountCircleIcon sx={{ fontSize: 28 }} /> },
    { text: "Головна", icon: <HomeIcon sx={{ fontSize: 28 }} /> },
    { text: "Пошук", icon: <SearchIcon sx={{ fontSize: 28 }} /> },
    { text: "Повідомлення", icon: <MessageIcon sx={{ fontSize: 28 }} /> },
  ];

  return (
    <Box
  sx={{
    bgcolor: "white",
    color: "black",
    height: "100vh",
    width: 300,
    borderRight: "1px solid #d1d5db",
    display: "flex",
    flexDirection: "column",
    position: "fixed",
    alignSelf: "flex-start",
    px: 2,  // горизонтальні падінги для сайдбару
  }}
>
  <Box
    sx={{
      display: "flex",
      alignItems: "center",
      justifyContent: "center",
      py: 3,  // вертикальні падінги
      // без maxWidth
    }}
  >
    <img src={logoSrc} alt="Logo" style={{ width: 200, height: "auto" }} />
  </Box>

  <MenuItems items={items} />
  <CreateButton />
</Box>
  );
};

export default LeftSidebar;