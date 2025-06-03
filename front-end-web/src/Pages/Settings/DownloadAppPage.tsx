import React from "react";
import DownloadAppButtons from "../../Components/SettingsPageComponents/DownloadAppButtons";

const DownloadAppPage: React.FC = () => {
  return (
    <div className="min-h-screen bg-gray-50 dark:bg-cyan-950 flex items-center justify-center p-6">
      <div className="max-w-xl w-full space-y-8">
        <h1 className="text-3xl font-extrabold text-gray-900 dark:text-white text-center">
          Get Our Mobile App
        </h1>
        <p className="text-center text-gray-600 dark:text-gray-300">
          Download the app for your device and stay connected wherever you are.
        </p>

        <DownloadAppButtons
          apkLink="https://example.com/app.apk"
          playStoreLink="https://play.google.com/store/apps/details?id=com.example"
          appStoreLink="https://apps.apple.com/app/id1234567890"
        />
      </div>
    </div>
  );
};

export default DownloadAppPage;
