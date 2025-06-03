import React from "react";
import LeftSidebar from "../../Components/MainPageComponents/LeftSidebar/LeftSidebar";
import SettingsHeader from "../../Components/SettingsPageComponents/SettingsHeader";
import NotificationsSettings from "../../Components/SettingsPageComponents/Notifications/NotificationsSettings";

const NotificationsPage: React.FC = () => {
  return (
    <div className="max-w-7xl mx-auto px-4 flex gap-20 pt-16">
      <div
        style={{
          width: 300,
          marginLeft: 10,
          flexShrink: 0,
          height: "600px",
        }}
      >
        <LeftSidebar
          customHeight="500px"
          hideCreateButton={true}
          onCreateClick={function (): void {
            throw new Error("Function not implemented.");
          }}
        />
      </div>

      <main className="flex-1 space-y-6">
        <SettingsHeader />
        <NotificationsSettings />
      </main>
    </div>
  );
};

export default NotificationsPage;
