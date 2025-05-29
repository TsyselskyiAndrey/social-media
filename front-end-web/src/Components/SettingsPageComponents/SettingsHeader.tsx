import React from "react";
import SettingsIcon from "@mui/icons-material/Settings";
import TuneIcon from "@mui/icons-material/Tune";

const SettingsHeader: React.FC = () => {
  return (
    <div className="mb-8 relative overflow-hidden">
      {/* Animated background gradient */}
      <div className="absolute inset-0 bg-gradient-to-r from-blue-50 via-indigo-50 to-purple-50 rounded-2xl opacity-60 animate-pulse"></div>
      
      {/* Floating decorative elements */}
      <div className="absolute top-2 right-4 w-16 h-16 bg-gradient-to-br from-blue-200 to-indigo-300 rounded-full opacity-20 animate-bounce" style={{animationDelay: '0.5s'}}></div>
      <div className="absolute top-8 right-12 w-8 h-8 bg-gradient-to-br from-purple-200 to-pink-300 rounded-full opacity-30 animate-bounce" style={{animationDelay: '1s'}}></div>
      <div className="absolute bottom-2 left-8 w-12 h-12 bg-gradient-to-br from-green-200 to-blue-300 rounded-full opacity-25 animate-bounce" style={{animationDelay: '1.5s'}}></div>
      
      {/* Main content */}
      <div className="relative z-10 p-6 bg-white/80 backdrop-blur-sm rounded-2xl border border-gray-100 shadow-lg hover:shadow-xl transition-all duration-500 ease-in-out transform hover:scale-[1.02]">
        <div className="flex items-center gap-4 mb-4">
          {/* Animated icon */}
          <div className="relative">
            <div className="absolute inset-0 bg-gradient-to-r from-blue-500 to-indigo-600 rounded-full animate-ping opacity-20"></div>
            <div className="relative bg-gradient-to-r from-blue-500 to-indigo-600 p-3 rounded-full shadow-lg transform hover:rotate-180 transition-transform duration-700 ease-in-out">
              <SettingsIcon className="text-white text-2xl" />
            </div>
          </div>
          
          {/* Animated title */}
          <div className="flex-1">
            <h1 className="text-4xl font-black bg-gradient-to-r from-gray-800 via-gray-900 to-black bg-clip-text text-transparent animate-pulse hover:from-blue-600 hover:via-indigo-700 hover:to-purple-800 transition-all duration-500">
              Налаштування
            </h1>
          </div>
          
          {/* Secondary animated icon */}
          <div className="transform hover:scale-110 transition-transform duration-300 hover:rotate-12">
            <TuneIcon className="text-gray-400 hover:text-blue-500 transition-colors duration-300 text-3xl" />
          </div>
        </div>
        
        {/* Animated description */}
        <div className="relative">
          <p className="text-lg text-gray-600 hover:text-gray-800 transition-colors duration-300 leading-relaxed font-medium">
            Керуйте налаштуваннями свого облікового запису
          </p>
          
          {/* Animated underline */}
          <div className="mt-2 h-1 bg-gradient-to-r from-blue-500 via-indigo-500 to-purple-500 rounded-full transform scale-x-0 hover:scale-x-100 transition-transform duration-500 ease-out origin-left"></div>
        </div>
        
        {/* Animated breadcrumb-style indicators */}
        <div className="flex gap-2 mt-4">
          <div className="w-2 h-2 bg-blue-500 rounded-full animate-pulse"></div>
          <div className="w-2 h-2 bg-indigo-500 rounded-full animate-pulse" style={{animationDelay: '0.2s'}}></div>
          <div className="w-2 h-2 bg-purple-500 rounded-full animate-pulse" style={{animationDelay: '0.4s'}}></div>
        </div>
      </div>
      
      {/* Animated border glow effect */}
      <div className="absolute inset-0 rounded-2xl bg-gradient-to-r from-blue-400 via-indigo-500 to-purple-600 opacity-0 hover:opacity-20 transition-opacity duration-500 blur-sm -z-10"></div>
    </div>
  );
};

export default SettingsHeader;
