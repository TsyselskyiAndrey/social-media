import React from "react";
import { useTheme } from "../../Contexts/ThemeContext";

const InterfaceSettings: React.FC = () => {
  const { theme, toggleTheme } = useTheme();

  return (
    <div className="bg-white dark:bg-cyan-950 p-6 rounded-lg shadow space-y-6">
      <h2 className="text-2xl font-semibold text-gray-900 dark:text-white">
        Налаштування інтерфейсу
      </h2>

      <div className="flex items-center justify-between hover:bg-gray-100 dark:hover:bg-gray-800 p-3 rounded-lg transition-all duration-300 ease-in-out">
        <div>
          <h3 className="text-lg font-bold text-gray-800 dark:text-gray-100">
            Темний режим
          </h3>
          <p className="text-sm text-gray-600 dark:text-gray-400">
            Перемикання між світлою та темною темами
          </p>
        </div>

        <button
          onClick={toggleTheme}
          className={`relative w-14 h-8 rounded-full transition-all duration-300 ease-in-out transform hover:scale-105 ${
            theme === "dark" ? "bg-teal-600" : "bg-gray-300"
          }`}
        >
          <span
            className={`absolute top-1 left-1 w-6 h-6 rounded-full bg-white shadow transition-all duration-300 ease-in-out ${
              theme === "dark" ? "translate-x-6" : ""
            }`}
          ></span>
        </button>
      </div>
    </div>
  );
};

export default InterfaceSettings;
