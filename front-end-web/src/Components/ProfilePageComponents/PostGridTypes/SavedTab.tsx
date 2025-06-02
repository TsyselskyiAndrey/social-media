import React, { useEffect, useState } from "react";
import Agent from "../../../API/agent";
import { Post } from "../../../API/agent";
import PostModal from "./PostModal";

interface SavedTabProps {
  viewMode?: "grid" | "list";
}

const SavedTab: React.FC<SavedTabProps> = ({ viewMode = "grid" }) => {
  const [hoveredVideoId, setHoveredVideoId] = useState<number | null>(null);
  const [savedPosts, setSavedPosts] = useState<Post[]>([]);
  const [modalOpen, setModalOpen] = useState(false);
  const [selectedPost, setSelectedPost] = useState<Post | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchSavedPosts = async () => {
      try {
        setLoading(true);
        const response = await Agent.Posts.getSavedPosts();
        setSavedPosts(response.data);
      } catch (error) {
        console.error("Помилка при завантаженні збережених постів:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchSavedPosts();
  }, []);

  const openModal = (post: Post) => {
    setSelectedPost(post);
    setModalOpen(true);
  };

  const closeModal = () => {
    setModalOpen(false);
    setSelectedPost(null);
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="flex space-x-3">
          <div className="w-4 h-4 bg-blue-600 rounded-full animate-bounce" style={{ animationDelay: '0ms' }}></div>
          <div className="w-4 h-4 bg-purple-600 rounded-full animate-bounce" style={{ animationDelay: '150ms' }}></div>
          <div className="w-4 h-4 bg-indigo-600 rounded-full animate-bounce" style={{ animationDelay: '300ms' }}></div>
        </div>
      </div>
    );
  }

  if (savedPosts.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center h-64 text-center bg-white dark:bg-gray-800 rounded-xl shadow-md p-8 border border-gray-200 dark:border-gray-700">
        <div className="text-6xl mb-6 opacity-75">📁</div>
        <h3 className="text-2xl font-semibold text-gray-800 dark:text-gray-200 mb-3">Немає збережених постів</h3>
        <p className="text-gray-600 dark:text-gray-400 text-lg">Пости, які ви збережете, з'являться тут</p>
      </div>
    );
  }

  const renderGridView = () => (
    <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-5 transition-all duration-500">
      {savedPosts.map((post, index) => {
        const media = post.postMedias[0];
        if (!media) return null;

        return (
          <div
            key={post.id}
            className="aspect-square overflow-hidden rounded-xl cursor-pointer bg-white dark:bg-gray-800 shadow-lg hover:shadow-xl transition-all duration-300 group relative border border-gray-200 dark:border-gray-700 transform hover:-translate-y-1"
            onClick={() => openModal(post)}
            style={{ animationDelay: `${index * 50}ms` }}
          >
            <div className="absolute inset-0 bg-gradient-to-br from-blue-600/20 to-purple-600/20 opacity-0 group-hover:opacity-100 transition-opacity duration-300 z-10"></div>
            {media.postMediaType.startsWith("Video") ? (
              <>
                <video
                  src={media.mediaUrl}
                  className="object-cover w-full h-full transition-transform duration-500 group-hover:scale-105"
                  muted={hoveredVideoId !== post.id}
                  loop
                  playsInline
                  autoPlay={hoveredVideoId === post.id}
                  onMouseEnter={() => setHoveredVideoId(post.id)}
                  onMouseLeave={() => setHoveredVideoId(null)}
                />
                <div className="absolute top-3 right-3 bg-black bg-opacity-70 rounded-full p-2 z-20">
                  <svg className="w-4 h-4 text-white" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M10 18a8 8 0 100-16 8 8 0 000 16zM9.555 7.168A1 1 0 008 8v4a1 1 0 001.555.832l3-2a1 1 0 000-1.664l-3-2z" />
                  </svg>
                </div>
              </>
            ) : (
              <img
                src={media.mediaUrl}
                alt={`saved-post-${post.id}`}
                className="object-cover w-full h-full transition-transform duration-500 group-hover:scale-105"
                loading="lazy"
              />
            )}
            <div className="absolute inset-0 bg-gradient-to-t from-black/80 via-black/40 to-transparent opacity-0 group-hover:opacity-100 transition-all duration-300 flex flex-col justify-end p-4 z-20">
              <div className="text-white text-base font-medium mb-2 line-clamp-2">
                {post.caption || "Без підпису"}
              </div>
              <div className="flex items-center justify-between text-white/90 text-sm">
                <span className="font-medium">{post.authorName}</span>
                <div className="flex items-center space-x-3">
                  <span className="flex items-center bg-black/30 px-2 py-1 rounded-full">
                    <svg className="w-3.5 h-3.5 mr-1" fill="currentColor" viewBox="0 0 20 20">
                      <path d="M10 12a2 2 0 100-4 2 2 0 000 4z" />
                      <path fillRule="evenodd" d="M.458 10C1.732 5.943 5.522 3 10 3s8.268 2.943 9.542 7c-1.274 4.057-5.064 7-9.542 7S1.732 14.057.458 10zM14 10a4 4 0 11-8 0 4 4 0 018 0z" clipRule="evenodd" />
                    </svg>
                    {post.views}
                  </span>
                  <span className="flex items-center bg-black/30 px-2 py-1 rounded-full">
                    <svg className="w-3.5 h-3.5 mr-1" fill="currentColor" viewBox="0 0 20 20">
                      <path d="M3.172 5.172a4 4 0 015.656 0L10 6.343l1.172-1.171a4 4 0 115.656 5.656L10 17.657l-6.828-6.829a4 4 0 010-5.656z" />
                    </svg>
                    {post.likes}
                  </span>
                </div>
              </div>
            </div>
          </div>
        );
      })}
    </div>
  );

  const renderListView = () => (
    <div className="space-y-6 transition-all duration-500">
      {savedPosts.map((post, index) => {
        const media = post.postMedias[0];
        if (!media) return null;

        return (
          <div
            key={post.id}
            className="flex bg-white dark:bg-gray-800 rounded-xl overflow-hidden shadow-lg hover:shadow-xl transition-all duration-300 cursor-pointer border border-gray-200 dark:border-gray-700 transform hover:-translate-y-1 group"
            onClick={() => openModal(post)}
            style={{ animationDelay: `${index * 50}ms` }}
          >
            <div className="relative w-40 h-40 sm:w-48 sm:h-48 flex-shrink-0 overflow-hidden">
              <div className="absolute inset-0 bg-gradient-to-br from-blue-600/20 to-purple-600/20 opacity-0 group-hover:opacity-100 transition-opacity duration-300 z-10"></div>
              {media.postMediaType.startsWith("Video") ? (
                <div className="relative w-full h-full">
                  <video
                    src={media.mediaUrl}
                    className="object-cover w-full h-full transition-transform duration-500 group-hover:scale-105"
                    muted
                  />
                  <div className="absolute inset-0 flex items-center justify-center bg-black bg-opacity-40 group-hover:bg-opacity-30 transition-all duration-300 z-20">
                    <svg className="w-12 h-12 text-white" fill="currentColor" viewBox="0 0 20 20">
                      <path d="M10 18a8 8 0 100-16 8 8 0 000 16zM9.555 7.168A1 1 0 008 8v4a1 1 0 001.555.832l3-2a1 1 0 000-1.664l-3-2z" />
                    </svg>
                  </div>
                </div>
              ) : (
                <img
                  src={media.mediaUrl}
                  alt={`saved-post-${post.id}`}
                  className="object-cover w-full h-full transition-transform duration-500 group-hover:scale-105"
                  loading="lazy"
                />
              )}
            </div>
            <div className="p-5 flex flex-col justify-between flex-grow">
              <div>
                <div className="flex items-center justify-between mb-3">
                  <h3 className="font-semibold text-lg text-gray-900 dark:text-white flex items-center">
                    {post.authorName}
                    <span className="ml-2 text-sm px-2 py-0.5 bg-blue-100 dark:bg-blue-900 text-blue-700 dark:text-blue-300 rounded-full">
                      {post.postType}
                    </span>
                  </h3>
                  <span className="text-sm text-gray-500 dark:text-gray-400">#{post.id}</span>
                </div>
                <p className="text-gray-700 dark:text-gray-300 text-base line-clamp-2 mb-4">
                  {post.caption || "Без підпису"}
                </p>
              </div>
              <div className="flex items-center justify-between">
                <div className="flex items-center space-x-4">
                  <span className="flex items-center text-gray-700 dark:text-gray-300 bg-gray-100 dark:bg-gray-700 px-3 py-1.5 rounded-full text-sm">
                    <svg className="w-4 h-4 mr-1.5" fill="currentColor" viewBox="0 0 20 20">
                      <path d="M10 12a2 2 0 100-4 2 2 0 000 4z" />
                      <path fillRule="evenodd" d="M.458 10C1.732 5.943 5.522 3 10 3s8.268 2.943 9.542 7c-1.274 4.057-5.064 7-9.542 7S1.732 14.057.458 10zM14 10a4 4 0 11-8 0 4 4 0 018 0z" clipRule="evenodd" />
                    </svg>
                    {post.views} переглядів
                  </span>
                  <span className="flex items-center text-gray-700 dark:text-gray-300 bg-gray-100 dark:bg-gray-700 px-3 py-1.5 rounded-full text-sm">
                    <svg className="w-4 h-4 mr-1.5" fill="currentColor" viewBox="0 0 20 20">
                      <path d="M3.172 5.172a4 4 0 015.656 0L10 6.343l1.172-1.171a4 4 0 115.656 5.656L10 17.657l-6.828-6.829a4 4 0 010-5.656z" />
                    </svg>
                    {post.likes} вподобань
                  </span>
                </div>
                <button className="text-blue-600 dark:text-blue-400 hover:text-blue-800 dark:hover:text-blue-300 transition-colors duration-300">
                  <svg className="w-5 h-5" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M6 10a2 2 0 11-4 0 2 2 0 014 0zM12 10a2 2 0 11-4 0 2 2 0 014 0zM16 12a2 2 0 100-4 2 2 0 000 4z" />
                  </svg>
                </button>
              </div>
            </div>
          </div>
        );
      })}
    </div>
  );

  return (
    <>
      <div className="transition-all duration-500">
        {viewMode === "grid" && renderGridView()}
        {viewMode === "list" && renderListView()}
      </div>

      {selectedPost && (
        <PostModal
          isOpen={modalOpen}
          onClose={closeModal}
          post={selectedPost}
        />
      )}
    </>
  );
};

export default SavedTab;
