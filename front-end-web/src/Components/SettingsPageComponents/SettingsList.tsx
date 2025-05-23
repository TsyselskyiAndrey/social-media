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
    { label: "Activity", icon: <AccessTimeIcon /> },
    { label: "Notifications", icon: <NotificationsIcon /> },
    { label: "Saved", icon: <BookmarkBorderIcon /> },
    { label: "Payment", icon: <PaymentIcon /> },
    { label: "Account", icon: <AccountCircleIcon /> },
    { label: "Help", icon: <HelpOutlineIcon /> },
    { label: "Interface", icon: <AccessibilityNewIcon /> },
  ];

  const handleClick = (label: string) => {
    if (label === "Interface") {
      navigate("/settings/interface");
    }
  };

  return (
    <div className="space-y-4">
      {options.map((item, idx) => (
        <div
          key={idx}
          onClick={() => handleClick(item.label)}
          className="flex items-center justify-between bg-white p-4 rounded-lg shadow-sm hover:bg-gray-50 transition cursor-pointer"
        >
          <div className="flex items-center gap-4">
            <div className="text-gray-700">{item.icon}</div>
            <span className="text-lg font-medium text-gray-800">{item.label}</span>
          </div>
          <ChevronRightIcon className="text-gray-500" />
        </div>
      ))}

      <div
        className="flex items-center justify-between bg-white p-4 rounded-lg shadow-sm hover:bg-gray-50 transition cursor-pointer"
      >
        <span className="text-lg font-medium text-red-600">Log out</span>
        <ChevronRightIcon className="text-red-500" />
      </div>
    </div>
  );
};

export default SettingsList;
