import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import LeftSidebar from "../../Components/MainPageComponents/LeftSidebar/LeftSidebar";
import EditProfileForm from "../../Components/EditPageComponents/EditProfileForm";
import Agent from "../../API/agent";
import { UserProfileInfo } from "../../API/agent";
import { useToast } from "../../Contexts/ToastContext";
import "./EditPage.css";

const EditPage: React.FC = () => {
  const [isCreating, setIsCreating] = useState(false);
  const [userProfile, setUserProfile] = useState<UserProfileInfo | null>(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();
  const { showSuccess, showError } = useToast();

  useEffect(() => {
    const fetchUserProfile = async () => {
      try {
        const response = await Agent.User.getUserProfileInfo();
        setUserProfile(response.data);
      } catch (error) {
        console.error("Помилка завантаження профілю:", error);
        showError("Не вдалося завантажити дані профілю. Спробуйте пізніше.");
      } finally {
        setLoading(false);
      }
    };

    fetchUserProfile();
  }, []);

  if (loading) {
    return (
      <div className="max-w-7xl mx-auto px-4 flex gap-6 pt-16">
        <div className="flex-1 flex justify-center items-center">
          <div className="text-lg">Завантаження...</div>
        </div>
      </div>
    );
  }

  return (
    <div className="max-w-7xl mx-auto px-4 flex gap-6 pt-16">
      <div style={{ width: 300, marginLeft: 10, flexShrink: 0 }}>
        <LeftSidebar 
          onCreateClick={() => setIsCreating(true)} 
          hideCreateButton={true} 
        />
      </div>

      <main className="flex-1">
        
        {userProfile && (
          <EditProfileForm 
            userProfile={userProfile} 
            onSave={(updatedProfile) => {
              setUserProfile(updatedProfile);
              showSuccess("Профіль успішно оновлено!");
              navigate("/profile");
            }}
            onCancel={() => navigate("/profile")}
          />
        )}
      </main>
    </div>
  );
};

export default EditPage;