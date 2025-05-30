import React from "react";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import HomeIcon from "@mui/icons-material/Home";
import SearchIcon from "@mui/icons-material/Search";
import MessageIcon from "@mui/icons-material/Message";
import SettingsIcon from "@mui/icons-material/Settings";
import logoSrc from "../../../Assets/logo.png";
import MenuItems from "./MenuItems/MenuItems";
import CreateButton from "./CreateButton/CreateButton";
import { useNavigate } from "react-router-dom";

type LeftSidebarProps = {
  onCreateClick: () => void;
  hideCreateButton?: boolean;
  customHeight?: string;
};

const LeftSidebar: React.FC<LeftSidebarProps> = ({
  onCreateClick,
  hideCreateButton = false,
  customHeight,
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
    { 
      text: "Пошук", 
      icon: <SearchIcon sx={{ fontSize: 28 }} />,
      onClick: () => navigate("/search"),
    },
    { text: "Повідомлення", icon: <MessageIcon sx={{ fontSize: 28 }} /> },
    { 
      text: "Налаштування", 
      icon: <SettingsIcon sx={{ fontSize: 28 }} />,
      onClick: () => navigate("/settings"),
    },
  ];

  return (
    <div
      className={`
        ${customHeight || "h-[90vh]"}
        w-[300px]
        border-r border-gray-300 dark:border-gray-700
        rounded-md
        flex flex-col fixed px-4
        bg-white text-black dark:bg-cyan-950 dark:text-gray-100
      `}
    >
      <div className="flex items-center justify-center py-6">
        <img src={logoSrc} alt="Logo" className="w-48 h-auto" />
      </div>

      <MenuItems items={items} />

      {!hideCreateButton && <CreateButton onClick={onCreateClick} />}
    </div>
  );
};

export default LeftSidebar;
