import React, { useState } from "react";
import GridViewIcon from "@mui/icons-material/GridView";
import BookmarkIcon from "@mui/icons-material/Bookmark";
import PersonIcon from "@mui/icons-material/Person";
import PostsTab from "./PostGridTypes/PostsTab";
import SavedTab from "./PostGridTypes/SavedTab";
import TaggedTab from "./PostGridTypes/TaggedTab";

const tabs = [
  { id: "posts", label: "Дописи", icon: <GridViewIcon fontSize="small" /> },
  { id: "saved", label: "Збережено", icon: <BookmarkIcon fontSize="small" /> },
  { id: "tagged", label: "Позначення", icon: <PersonIcon fontSize="small" /> },
];

const PostGrid: React.FC = () => {
  const [activeTab, setActiveTab] = useState("posts");

  return (
    <div className="max-w-[1280px] mx-auto px-6 pt-6">
      <div className="flex justify-center gap-16 border-t border-gray-300 bg-gray-50 rounded-t-md px-6">
        {tabs.map(({ id, label, icon }) => {
          const isActive = id === activeTab;
          return (
            <button
              key={id}
              onClick={() => setActiveTab(id)}
              className={`relative flex items-center gap-2 pt-4 pb-3 rounded-t-md transition-colors duration-300 bg-gray-50 ${
                isActive
                  ? "text-gray-900 font-semibold"
                  : "text-gray-500 hover:text-gray-700"
              }`}
            >
              {isActive && (
                <span className="absolute bottom-0 left-0 right-0 h-1.5 bg-blue-600 rounded-t-md" />
              )}
              <span className={`${isActive ? "text-blue-700" : "text-gray-400"}`}>
                {icon}
              </span>
              {label}
            </button>
          );
        })}
      </div>

      <div className="mt-4">
        {activeTab === "posts" && <PostsTab />}
        {activeTab === "saved" && <SavedTab />}
        {activeTab === "tagged" && <TaggedTab />}
      </div>
    </div>
  );
};

export default PostGrid;