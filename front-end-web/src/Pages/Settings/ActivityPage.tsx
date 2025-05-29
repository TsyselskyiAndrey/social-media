import React from "react";
import ActivitySettings from "../../Components/SettingsPageComponents/Activity/ActivitySettings";

const ActivityPage: React.FC = () => {
  return (
    <div className="p-4">
      <h2 className="text-xl font-semibold mb-4">Активність</h2>
      <ActivitySettings />
    </div>
  );
};

export default ActivityPage;
