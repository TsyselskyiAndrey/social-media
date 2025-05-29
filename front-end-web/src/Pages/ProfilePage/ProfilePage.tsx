import React, { useEffect, useState } from "react";
import ProfileHeader from "../../Components/ProfilePageComponents/ProfileHeader";
import ProfileBio from "../../Components/ProfilePageComponents/ProfileBio";
import PostGrid from "../../Components/ProfilePageComponents/PostGrid";
import LeftSidebar from "../../Components/MainPageComponents/LeftSidebar/LeftSidebar";
import { UserProfileInfo } from "../../API/agent"
import Agent from "../../API/agent";

const ProfilePage: React.FC = () => {
  const [isCreating, setIsCreating] = useState(false);
  const [profileInfo, setProfileInfo] = useState<UserProfileInfo | null>(null);

  useEffect(() => {
    const fetchProfile = async () => {
      const response = await Agent.User.getUserProfileInfo();
      setProfileInfo(response.data);
    };
    fetchProfile();
  }, []);

  if (!profileInfo) return <div>Завантаження...</div>;

  return (
    <div className="max-w-7xl mx-auto px-4 flex gap-6 pt-16">
      <div style={{ width: 300, marginLeft: 10, flexShrink: 0 }}>
        <LeftSidebar onCreateClick={() => setIsCreating(true)} />
      </div>

      <main className="flex-1">
        <ProfileHeader profile={profileInfo} />
        <ProfileBio profile={profileInfo} />
        <PostGrid />
      </main>
    </div>
  );
};

export default ProfilePage;
