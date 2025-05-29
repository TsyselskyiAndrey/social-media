import React from "react";
import { useNavigate } from "react-router-dom";
import AccessTimeIcon from "@mui/icons-material/AccessTime";
import NotificationsIcon from "@mui/icons-material/Notifications";
import BookmarkBorderIcon from "@mui/icons-material/BookmarkBorder";
import PaymentIcon from "@mui/icons-material/Payment";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import HelpOutlineIcon from "@mui/icons-material/HelpOutline";
import AccessibilityNewIcon from "@mui/icons-material/AccessibilityNew";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";

const SettingsList: React.FC = () => {
  const navigate = useNavigate();

  const options = [
    { label: "Активність", icon: <AccessTimeIcon /> },
    { label: "Сповіщення", icon: <NotificationsIcon /> },
    { label: "Збережені", icon: <BookmarkBorderIcon /> },
    { label: "Підписки", icon: <PaymentIcon /> },
    { label: "Акаунт", icon: <AccountCircleIcon /> },
    { label: "Допомога", icon: <HelpOutlineIcon /> },
    { label: "Інтерфейс", icon: <AccessibilityNewIcon /> },
  ];

  const handleClick = (label: string) => {
    if (label === "Інтерфейс") {
      navigate("/settings/interface");
    }
  };

  return (
    <div className="space-y-4">
      {options.map((item, idx) => (
        <div
          key={idx}
          onClick={() => handleClick(item.label)}
          className="flex items-center justify-between bg-white p-5 rounded-xl shadow-md hover:shadow-lg hover:bg-gradient-to-r hover:from-blue-50 hover:to-indigo-50 transition-all duration-300 ease-in-out cursor-pointer transform hover:scale-[1.02] hover:-translate-y-1 border border-gray-100 hover:border-blue-200"
        >
          <div className="flex items-center gap-5">
            <div className="text-gray-700 hover:text-blue-600 transition-colors duration-300 transform hover:scale-110">
              {item.icon}
            </div>
            <span className="text-xl font-bold text-gray-900 hover:text-blue-700 transition-colors duration-300">
              {item.label}
            </span>
          </div>
          <ChevronRightIcon className="text-gray-500 hover:text-blue-600 transition-all duration-300 transform hover:translate-x-1" />
        </div>
      ))}

      <div className="flex items-center justify-between bg-white p-5 rounded-xl shadow-md hover:shadow-lg hover:bg-gradient-to-r hover:from-red-50 hover:to-pink-50 transition-all duration-300 ease-in-out cursor-pointer transform hover:scale-[1.02] hover:-translate-y-1 border border-gray-100 hover:border-red-200">
        <span className="text-xl font-bold text-red-600 hover:text-red-700 transition-colors duration-300">
          Log out
        </span>
        <ChevronRightIcon className="text-red-500 hover:text-red-600 transition-all duration-300 transform hover:translate-x-1" />
      </div>
    </div>
  );
};

export default SettingsList;
