import React from 'react';
import NotificationsSettings from '../../Components/SettingsPageComponents/Notifications/NotificationsSettings';
import SettingsHeader from '../../Components/SettingsPageComponents/SettingsHeader';

const NotificationsPage = () => {
  return (
    <div>
      <SettingsHeader/>
      <NotificationsSettings />
    </div>
  );
};

export default NotificationsPage;
