import React from "react";
import CakeIcon from "@mui/icons-material/Cake";
import EmailIcon from "@mui/icons-material/Email";
import { UserProfileInfo } from "../../API/agent";

interface Props {
  profile: UserProfileInfo;
}

const ProfileBio: React.FC<Props> = ({ profile }) => {
  return (
    <div
      className="max-w-[1280px] mx-auto px-6 py-6 bg-white dark:bg-cyan-950 rounded-lg shadow-lg border border-gray-200 dark:border-gray-600"
      style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "24px" }}
    >
      <div className="space-y-4">
        {profile.biography && (
          <p className="text-lg text-gray-700 dark:text-gray-300 leading-relaxed">
            {profile.biography}
          </p>
        )}
      </div>

      <div className="flex flex-col justify-center gap-4 text-gray-700 dark:text-gray-300 text-base md:text-lg">
        {profile.birthDate && (
          <div className="flex items-center gap-2 hover:text-blue-600 dark:hover:text-blue-400 transition-colors cursor-default">
            <CakeIcon className="text-pink-400" />
            <span className="font-semibold">
              Дата народження: {new Date(profile.birthDate).toLocaleDateString("uk-UA")}
            </span>
          </div>
        )}
        <div className="flex items-center gap-2 hover:text-blue-600 dark:hover:text-blue-400 transition-colors cursor-pointer">
          <EmailIcon className="text-green-500" />
          <a
            href={`mailto:${profile.email}`}
            className="font-semibold underline hover:text-green-700 dark:hover:text-green-400"
          >
            {profile.email}
          </a>
        </div>
      </div>
    </div>
  );
};

export default ProfileBio;
