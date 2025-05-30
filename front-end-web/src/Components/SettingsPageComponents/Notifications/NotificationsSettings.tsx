import React, { useState } from "react";

interface NotificationSettingsProps {}

const NotificationsSettings: React.FC<NotificationSettingsProps> = () => {
  const [settings, setSettings] = useState({
    NotifyComments: true,
    NotifyFollows: true,
    NotifyMentions: true,
    NotifyMessages: true,
    NotifyReplies: true,
    NotifyPostLikes: true,
  });

  const toggleSetting = (key: keyof typeof settings) => {
    setSettings((prev) => ({
      ...prev,
      [key]: !prev[key],
    }));
  };

  const notificationLabels: { [key in keyof typeof settings]: string } = {
    NotifyComments: "Коментарі до ваших постів",
    NotifyFollows: "Нові підписники",
    NotifyMentions: "Згадки у постах та коментарях",
    NotifyMessages: "Особисті повідомлення",
    NotifyReplies: "Відповіді на ваші коментарі",
    NotifyPostLikes: "Лайки ваших постів",
  };

  return (
    <div className="bg-white dark:bg-cyan-950 p-6 rounded-lg shadow space-y-6">
      <h2 className="text-2xl font-semibold text-gray-900 dark:text-white">
        Налаштування сповіщень
      </h2>

      {Object.entries(notificationLabels).map(([key, label]) => (
        <div
          key={key}
          className="flex items-center justify-between hover:bg-gray-100 dark:hover:bg-gray-800 p-3 rounded-lg transition-all duration-300 ease-in-out"
        >
          <div>
            <h3 className="text-lg font-bold text-gray-800 dark:text-gray-100">
              {label}
            </h3>
            <p className="text-sm text-gray-600 dark:text-gray-400">
              Увімкнути чи вимкнути сповіщення
            </p>
          </div>

          <button
            onClick={() => toggleSetting(key as keyof typeof settings)}
            className={`relative w-14 h-8 rounded-full transition-all duration-300 ease-in-out transform hover:scale-105 ${
              settings[key as keyof typeof settings]
                ? "bg-teal-600"
                : "bg-gray-300"
            }`}
          >
            <span
              className={`absolute top-1 left-1 w-6 h-6 rounded-full bg-white shadow transition-all duration-300 ease-in-out ${
                settings[key as keyof typeof settings] ? "translate-x-6" : ""
              }`}
            ></span>
          </button>
        </div>
      ))}
    </div>
  );
};

export default NotificationsSettings;
