import React from "react";
import RecommendedUsers from "./RecommendedUsers/RecommendedUsers";

const trends = ["#React", "#MUI", "#TypeScript", "#TwitterClone", "#Frontend"];

const RightSidebar: React.FC = () => {
  return (
    <div className="fixed top-20 w-[380px] p-4 flex flex-col gap-4">
      <div
        className="
          bg-white dark:bg-cyan-950
          text-black dark:text-gray-100
          rounded-lg shadow-md dark:shadow-none
          p-4
        "
      >
        <h2 className="text-lg font-semibold text-center mb-3">Тренди</h2>

        <div className="flex flex-wrap gap-2">
          {trends.map((trend) => (
            <button
              key={trend}
              className="
                border border-amber-400
                text-gray-700 dark:text-amber-400
                rounded-full px-3 py-1 text-sm
                hover:bg-amber-50 dark:hover:bg-amber-500/10
                hover:text-black dark:hover:text-white
                transition
              "
            >
              {trend}
            </button>
          ))}
        </div>
      </div>

      <RecommendedUsers />
    </div>
  );
};

export default RightSidebar;
