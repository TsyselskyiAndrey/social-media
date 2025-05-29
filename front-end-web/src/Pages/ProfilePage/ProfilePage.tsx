import React, { useEffect, useState } from "react";
import ProfileHeader from "../../Components/ProfilePageComponents/ProfileHeader";
import ProfileBio from "../../Components/ProfilePageComponents/ProfileBio";
import PostGrid from "../../Components/ProfilePageComponents/PostGrid";
import LeftSidebar from "../../Components/MainPageComponents/LeftSidebar/LeftSidebar";
import { UserProfileInfo } from "../../API/agent"
import Agent from "../../API/agent";
import CreatePost from "../../Components/MainPageComponents/CreatePost/CreatePost";

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

  useEffect(() => {
    if (isCreating) {
      document.body.style.overflow = "hidden";
    } else {
      document.body.style.overflow = "auto";
    }
    return () => {
      document.body.style.overflow = "auto";
    };
  }, [isCreating]);

  useEffect(() => {
    const handleEsc = (event: KeyboardEvent) => {
      if (event.key === "Escape") {
        setIsCreating(false);
      }
    };
    window.addEventListener("keydown", handleEsc);
    return () => window.removeEventListener("keydown", handleEsc);
  }, []);

  if (!profileInfo) return <div>Завантаження...</div>;

  return (
    <div className="max-w-7xl mx-auto px-4 flex gap-6 pt-16">
      {/* Left Sidebar */}
      <div style={{ width: 300, marginLeft: 10, flexShrink: 0 }}>
        <LeftSidebar onCreateClick={() => setIsCreating(true)} />
      </div>

      <main className="flex-1">
        <ProfileHeader profile={profileInfo} />
        <ProfileBio profile={profileInfo} />
        <PostGrid />
      </main>

      {isCreating && (
        <div
          style={{
            position: "fixed",
            inset: 0,
            backgroundColor: "rgba(0, 0, 0, 0.4)",
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            zIndex: 9999,
            padding: "16px",
          }}
          onClick={() => setIsCreating(false)}
        >
          <div
            onClick={(e) => e.stopPropagation()}
            style={{
              width: "100%",
              maxWidth: 900,
              maxHeight: "90vh",
              backgroundColor: "white",
              borderRadius: 8,
              padding: 24,
              overflowY: "auto",
              boxShadow: "0 10px 20px rgba(0,0,0,0.3)",
            }}
          >
            <CreatePost onClose={() => setIsCreating(false)} />
          </div>
        </div>
      )}
    </div>
  );
};

export default ProfilePage;
