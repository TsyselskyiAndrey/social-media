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

  return (
    <div className="bg-gray-50 p-6 rounded-lg shadow-md max-w-5xl mx-auto mb-8">
      <div className="flex items-center gap-10">
        <img
          src={profile.profileImagePath ?? "https://via.placeholder.com/150"}
          alt="Avatar"
          className="rounded-full w-28 h-28 object-cover shadow-md border border-gray-300"
        />
        <div className="flex-1 flex flex-col gap-4">
          <h2 className="text-3xl font-bold tracking-tight text-gray-900">{profile.userName}</h2>
          <div>
            <p className="text-base font-bold tracking-tight text-gray-500">{profile.firstName} {profile.lastName}</p>
          </div>
          <div className="flex gap-6 text-gray-700 font-semibold text-sm">
            <div className="flex flex-col items-start bg-white rounded-md px-6 py-3 shadow-sm w-28">
              <div className="text-lg text-gray-900 font-bold">{profile.postsAmount}</div>
              <div className="text-gray-600">дописи</div>
            </div>
            <div className="flex flex-col items-start bg-white rounded-md px-6 py-3 shadow-sm w-28">
              <div className="text-lg text-gray-900 font-bold">{profile.followers}</div>
              <div className="text-gray-600">підписників</div>
            </div>
            <div className="flex flex-col items-start bg-white rounded-md px-6 py-3 shadow-sm w-28">
              <div className="text-lg text-gray-900 font-bold">{profile.followed}</div>
              <div className="text-gray-600">підписок</div>
            </div>
          </div>
        </div>
        <div className="flex flex-col items-end gap-3">
          <button className="px-5 py-1.5 border border-blue-600 bg-blue-600 text-white rounded-lg text-sm font-semibold hover:bg-blue-700 transition">
            Редагувати профіль
          </button>
          <button
            aria-label="Налаштування профілю"
            className="p-2 rounded-full hover:bg-gray-200 transition"
            title="Налаштування"
            onClick={goToSettings}
          >
            <SettingsIcon className="text-gray-700" />
          </button>
        </div>
      </div>
    </div>
  );
};

export default ProfileHeader;
