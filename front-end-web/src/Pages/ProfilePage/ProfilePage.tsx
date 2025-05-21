import React, { useState } from "react";
import ProfileHeader from "../../Components/ProfilePageComponents/ProfileHeader";
import ProfileBio from "../../Components/ProfilePageComponents/ProfileBio";
import PostGrid from "../../Components/ProfilePageComponents/PostGrid";
import LeftSidebar from "../../Components/MainPageComponents/LeftSidebar/LeftSidebar";

const ProfilePage: React.FC = () => {
  const [isCreating, setIsCreating] = useState(false);

  return (
    <div className="max-w-7xl mx-auto px-4 flex gap-6 pt-16">
        
  <div style={{ width: 300, marginLeft: 10, flexShrink: 0 }}>
    <LeftSidebar onCreateClick={() => setIsCreating(true)} />
  </div>

  <main className="flex-1">
    <ProfileHeader />
    <ProfileBio />
    <PostGrid />
  </main>
</div>
  );
};

export default ProfilePage;