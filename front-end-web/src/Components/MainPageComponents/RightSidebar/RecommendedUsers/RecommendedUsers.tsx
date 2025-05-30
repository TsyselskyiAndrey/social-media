import React, { useState } from "react";

const recommendedUsers = [
  { id: 1, name: "Ірина Сидоренко", handle: "@iryna" },
  { id: 2, name: "Андрій Коваленко", handle: "@andrii" },
  { id: 3, name: "Марія Литвин", handle: "@maria" },
];

const RecommendedUsers: React.FC = () => {
  const [following, setFollowing] = useState<number[]>([]);

  const toggleFollow = (id: number) => {
    setFollowing((prev) =>
      prev.includes(id) ? prev.filter((uid) => uid !== id) : [...prev, id]
    );
  };

  return (
    <div className="bg-white dark:bg-cyan-950 rounded-lg shadow-md dark:shadow-none p-4">
      <h2 className="text-lg font-semibold mb-3 text-gray-900 dark:text-gray-100">
        Рекомендовані для вас
      </h2>

      <div className="flex flex-col divide-y divide-gray-200 dark:divide-gray-700">
        {recommendedUsers.map((user) => {
          const isFollowing = following.includes(user.id);
          return (
            <div
              key={user.id}
              className="flex items-center justify-between py-3"
            >
              <div className="flex items-center gap-3">
                <div className="w-10 h-10 rounded-full bg-amber-400 text-black font-bold flex items-center justify-center text-lg">
                  {user.name[0]}
                </div>
                <div>
                  <div className="font-medium text-gray-900 dark:text-gray-100">
                    {user.name}
                  </div>
                  <div className="text-sm text-gray-500">{user.handle}</div>
                </div>
              </div>

              <button
                className={`px-4 py-1.5 rounded-full text-sm font-semibold border transition 
                  ${
                    isFollowing
                      ? "bg-teal-500 text-white border-teal-500 hover:bg-teal-600"
                      : "border-teal-500 text-teal-500 hover:bg-teal-500/10"
                  }`}
                onClick={() => toggleFollow(user.id)}
              >
                {isFollowing ? "Підписано" : "Підписатися"}
              </button>
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default RecommendedUsers;
