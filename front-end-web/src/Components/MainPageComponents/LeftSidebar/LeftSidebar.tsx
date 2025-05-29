import React from "react";
import { Box } from "@mui/material";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import HomeIcon from "@mui/icons-material/Home";
import SearchIcon from "@mui/icons-material/Search";
import MessageIcon from "@mui/icons-material/Message";
import logoSrc from "../../../Assets/logo.png";
import MenuItems from "./MenuItems/MenuItems";
import CreateButton from "./CreateButton/CreateButton";
import { useNavigate } from "react-router-dom";
import SettingsIcon from "@mui/icons-material/Settings";

type LeftSidebarProps = {
  onCreateClick: () => void;
  hideCreateButton?: boolean;
  customHeight?: string; // Новий пропс
};

const LeftSidebar: React.FC<LeftSidebarProps> = ({ 
  onCreateClick, 
  hideCreateButton = false, 
  customHeight 
}) => {
  const navigate = useNavigate();

  const items = [
    {
      text: "Профіль",
      icon: <AccountCircleIcon sx={{ fontSize: 28 }} />,
      onClick: () => navigate("/profile"),
    },
    {
      text: "Головна",
      icon: <HomeIcon sx={{ fontSize: 28 }} />,
      onClick: () => navigate("/mainpage"),
    },
    
    { text: "Пошук", icon: <SearchIcon sx={{ fontSize: 28 }} /> },
    { text: "Повідомлення", icon: <MessageIcon sx={{ fontSize: 28 }} /> },
    { text: "Налаштування", 
      icon: <SettingsIcon sx={{ fontSize: 28 }} />,
      onClick: () => navigate("/settings"), },
  ];

  return (
    <Box
      sx={{
        bgcolor: "white",
        color: "black",
        height: customHeight || "90vh", // Використовуємо customHeight якщо передано
        width: 300,
        borderRight: "1px solid #d1d5db",
        borderRadius: 3, // Додано border-radius: 3px
        display: "flex",
        flexDirection: "column",
        position: "fixed",
        alignSelf: "flex-start",
        px: 2,
      }}
    >
      <Box
        sx={{
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          py: 3,
        }}
      >
        <img src={logoSrc} alt="Logo" style={{ width: 200, height: "auto" }} />
      </Box>

      <MenuItems items={items} />

      {!hideCreateButton && <CreateButton onClick={onCreateClick} />}
    </Box>
  );
};

export default LeftSidebar;
