import React from "react";
import { useTheme } from "../../Contexts/ThemeContext";

const InterfaceSettings: React.FC = () => {
  const { theme, toggleTheme } = useTheme();

  return (
    <div className="bg-white dark:bg-gray-100 p-6 rounded-lg shadow space-y-6">
      <h2 className="text-2xl font-semibold text-dark dark:text-gray-800">Interface Preferences</h2>

      <div className="flex items-center justify-between">
        <div>
          <h3 className="text-lg font-medium text-gray-700 dark:text-gray-800">Dark Mode</h3>
          <p className="text-sm text-gray-500 dark:text-gray-600">Switch between light and dark themes</p>
        </div>

        <button
          onClick={toggleTheme}
          className={`relative w-14 h-8 rounded-full transition ${
            theme === "dark" ? "bg-blue-600" : "bg-gray-300"
          }`}
        >
          <span
            className={`absolute top-1 left-1 w-6 h-6 rounded-full bg-white shadow transition ${
              theme === "dark" ? "translate-x-6" : ""
            }`}
          ></span>
        </button>
      </div>
    </div>
  );
};

export default InterfaceSettings;
