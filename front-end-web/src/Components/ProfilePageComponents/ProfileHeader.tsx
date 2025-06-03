import React from "react";
import SettingsIcon from "@mui/icons-material/Settings";
import { useNavigate } from "react-router-dom";
import { UserProfileInfo } from "../../API/agent";

interface Props {
  profile: UserProfileInfo;
}

const ProfileHeader: React.FC<Props> = ({ profile }) => {
  const navigate = useNavigate();

  const goToSettings = () => {
    navigate("/settings");
  };

  const goToEditProfile = () => {
    navigate("/editpage");
  };

  return (
    <div className="bg-gray-50 dark:bg-cyan-950 p-6 rounded-lg shadow-md dark:shadow-lg max-w-5xl mx-auto mb-8">
      <div className="flex items-center gap-10">
        <img
          src={profile.profileImagePath ?? "https://via.placeholder.com/150"}
          alt="Avatar"
          className="rounded-full w-28 h-28 object-cover shadow-md border border-gray-300 dark:border-gray-600"
        />
        <div className="flex-1 flex flex-col gap-4">
          <h2 className="text-3xl font-bold tracking-tight text-gray-900 dark:text-white">{profile.userName}</h2>
          <div>
            <p className="text-base font-bold tracking-tight text-gray-500 dark:text-gray-300">
              {profile.firstName} {profile.lastName}
            </p>
          </div>
          <div className="flex gap-6 text-gray-700 dark:text-gray-300 font-semibold text-sm">
            <div className="flex flex-col items-start bg-white dark:bg-cyan-900 rounded-md px-6 py-3 shadow-sm w-28">
              <div className="text-lg text-gray-900 dark:text-white font-bold">{profile.postsAmount}</div>
              <div className="text-gray-600 dark:text-gray-400">дописи</div>
            </div>
            <div className="flex flex-col items-start bg-white dark:bg-cyan-900 rounded-md px-6 py-3 shadow-sm w-28">
              <div className="text-lg text-gray-900 dark:text-white font-bold">{profile.followers}</div>
              <div className="text-gray-600 dark:text-gray-400">підписників</div>
            </div>
            <div className="flex flex-col items-start bg-white dark:bg-cyan-900 rounded-md px-6 py-3 shadow-sm w-28">
              <div className="text-lg text-gray-900 dark:text-white font-bold">{profile.followed}</div>
              <div className="text-gray-600 dark:text-gray-400">підписок</div>
            </div>
          </div>
        </div>
        <div className="flex flex-col items-end gap-3">
          <button
            onClick={goToEditProfile}
            className="px-5 py-1.5 border border-teal-600 bg-teal-600 text-white rounded-lg text-sm font-semibold 
            hover:bg-blue-700 hover:scale-105 hover:shadow-lg active:scale-95 
            transition-all duration-300 ease-in-out relative overflow-hidden 
            before:absolute before:inset-0 before:bg-white before:opacity-0 before:scale-0 
            hover:before:opacity-20 hover:before:scale-100 before:transition-all before:duration-300 before:rounded-lg"
          >
            Редагувати профіль
          </button>
          <button
            aria-label="Налаштування профілю"
            className="p-2 rounded-full hover:bg-gray-200 dark:hover:bg-gray-600 transition-all duration-300 hover:rotate-12 hover:scale-110 active:scale-95"
            title="Налаштування"
            onClick={goToSettings}
          >
            <SettingsIcon className="text-gray-700 dark:text-gray-200" />
          </button>
        </div>
      </div>
    </div>
  );
};

export default ProfileHeader;
