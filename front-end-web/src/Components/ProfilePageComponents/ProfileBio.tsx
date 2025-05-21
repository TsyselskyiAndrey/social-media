import React from "react";
import LocationOnIcon from "@mui/icons-material/LocationOn";
import CakeIcon from "@mui/icons-material/Cake";
import EmailIcon from "@mui/icons-material/Email";

const ProfileBio: React.FC = () => {
  return (
    <div
      className="max-w-[1280px] mx-auto px-6 py-6 bg-white rounded-lg shadow-lg border border-gray-200"
      style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "24px" }}
    >
      <div className="space-y-4">
        <p className="text-3xl font-extrabold text-gray-900 leading-tight">
          Проект
        </p>
        <p className="text-lg text-gray-700 leading-relaxed">
          📸 Фанат фотографії, подорожей та кави ☕
        </p>
        <a
          href="https://example.com"
          target="_blank"
          rel="noopener noreferrer"
          className="inline-block text-blue-600 font-semibold hover:underline hover:text-blue-800 transition-colors duration-300"
        >
          https://example.com
        </a>
      </div>

      <div className="flex flex-col justify-center gap-4 text-gray-700 text-base md:text-lg">
        <div className="flex items-center gap-2 hover:text-blue-600 transition-colors cursor-default">
          <LocationOnIcon className="text-blue-500" />
          <span className="font-semibold">Київ, Україна</span>
        </div>
        <div className="flex items-center gap-2 hover:text-blue-600 transition-colors cursor-default">
          <CakeIcon className="text-pink-400" />
          <span className="font-semibold">Дата народження: 12 січня 1990</span>
        </div>
        <div className="flex items-center gap-2 hover:text-blue-600 transition-colors cursor-pointer">
          <EmailIcon className="text-green-500" />
          <a
            href="mailto:user@example.com"
            className="font-semibold underline hover:text-green-700"
          >
            user@example.com
          </a>
        </div>
      </div>
    </div>
  );
};

export default ProfileBio;