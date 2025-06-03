import SavedTab from "../../Components/ProfilePageComponents/PostGridTypes/SavedTab";
import { useState, useEffect } from "react";

export default function Saved() {
  const [viewMode, setViewMode] = useState<"grid" | "list">("grid");
  const [mounted, setMounted] = useState(false);

  useEffect(() => {
    setMounted(true);
    return () => setMounted(false);
  }, []);

  return (
    <div className={`p-6 max-w-7xl mx-auto ${mounted ? 'animate-fadeIn' : ''}`}>
      <div className="flex justify-between items-center mb-8">
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white relative inline-block">
          Збережені пости
          <span className="absolute -bottom-1 left-0 w-1/2 h-1 bg-gradient-to-r from-blue-600 to-purple-600"></span>
        </h1>
        <div className="bg-white dark:bg-gray-800 rounded-xl shadow-md p-1.5 border border-gray-200 dark:border-gray-700">
          <div className="flex space-x-1">
            <button
              onClick={() => setViewMode("grid")}
              className={`px-4 py-2 rounded-lg transition-all duration-300 flex items-center ${viewMode === "grid" 
                ? "bg-blue-600 text-white shadow-md" 
                : "text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700"}`}
            >
              <svg className="w-5 h-5 mr-2" fill="currentColor" viewBox="0 0 24 24">
                <path d="M4 4h7v7H4V4zm9 0h7v7h-7V4zm-9 9h7v7H4v-7zm9 0h7v7h-7v-7z"></path>
              </svg>
              Сітка
            </button>
            <button
              onClick={() => setViewMode("list")}
              className={`px-4 py-2 rounded-lg transition-all duration-300 flex items-center ${viewMode === "list" 
                ? "bg-blue-600 text-white shadow-md" 
                : "text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700"}`}
            >
              <svg className="w-5 h-5 mr-2" fill="currentColor" viewBox="0 0 24 24">
                <path d="M3 5h18v2H3V5zm0 6h18v2H3v-2zm0 6h18v2H3v-2z"></path>
              </svg>
              Список
            </button>
          </div>
        </div>
      </div>
      <SavedTab viewMode={viewMode} />
    </div>
  );
}
