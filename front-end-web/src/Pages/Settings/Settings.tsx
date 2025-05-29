import React from "react";
import LeftSidebar from "../../Components/MainPageComponents/LeftSidebar/LeftSidebar";
import SettingsHeader from "../../Components/SettingsPageComponents/SettingsHeader";
import SettingsList from "../../Components/SettingsPageComponents/SettingsList";

const SettingsPage: React.FC = () => {
  return (
    <div className="max-w-7xl mx-auto px-4 flex gap-20 pt-16">
      <div style={{ width: 300, marginLeft: 10, flexShrink: 0 }}>
        <LeftSidebar onCreateClick={() => {}} hideCreateButton={true} customHeight="500px"/>
      </div>
      
      <main className="flex-1">
        <SettingsHeader />
        <SettingsList />
      </main>
    </div>
  );
};

export default SettingsPage;
