import React, { useEffect, useState } from "react";
import Agent from "../../../API/agent";
import { Post as PostType } from "../../../API/agent";
import PostModal from "./PostModal";

const PostsTab: React.FC = () => {
  const [userPosts, setUserPosts] = useState<PostType[]>([]);
  const [loading, setLoading] = useState(true);
  const [hoveredVideoId, setHoveredVideoId] = useState<number | null>(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [selectedPost, setSelectedPost] = useState<PostType | null>(null);

  useEffect(() => {
    const fetchUserPosts = async () => {
      try {
        setLoading(true);

        const response = await Agent.Posts.getPosts(null, 50, null, null, null);
        setUserPosts(response.data);
      } catch (error) {
        console.error("Помилка при завантаженні постів користувача:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchUserPosts();
  }, []);

  const openModal = (post: PostType) => {
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

  if (userPosts.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center h-64 text-center bg-white dark:bg-gray-800 rounded-xl shadow-md p-8 border border-gray-200 dark:border-gray-700">
        <div className="text-6xl mb-6 opacity-75">📷</div>
        <h3 className="text-2xl font-semibold text-gray-800 dark:text-gray-200 mb-3">Немає постів</h3>
        <p className="text-gray-600 dark:text-gray-400 text-lg">Створіть свій перший пост, щоб він з'явився тут</p>
      </div>
    );
  }

  // Відображення сітки постів
  const renderGrid = () => (
    <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-1 md:gap-2">
      {userPosts.map((post) => {
        const media = post.postMedias[0];
        if (!media) return null;

        return (
          <div
            key={post.id}
            className="aspect-square overflow-hidden bg-gray-100 dark:bg-gray-800 rounded-md cursor-pointer relative group"
            onClick={() => openModal(post)}
          >
            {media.postMediaType.startsWith("Video") ? (
              <>
                <video
                  src={media.mediaUrl}
                  className="object-cover w-full h-full hover:scale-105 transition-transform duration-200"
                  muted={hoveredVideoId !== post.id}
                  loop
                  playsInline
                  autoPlay={hoveredVideoId === post.id}
                  onMouseEnter={() => setHoveredVideoId(post.id)}
                  onMouseLeave={() => setHoveredVideoId(null)}
                />
                <div className="absolute top-2 right-2 bg-black bg-opacity-70 rounded-full p-1.5">
                  <svg className="w-3.5 h-3.5 text-white" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M10 18a8 8 0 100-16 8 8 0 000 16zM9.555 7.168A1 1 0 008 8v4a1 1 0 001.555.832l3-2a1 1 0 000-1.664l-3-2z" />
                  </svg>
                </div>
              </>
            ) : (
              <img
                src={media.mediaUrl}
                alt={`post-${post.id}`}
                className="object-cover w-full h-full hover:scale-105 transition-transform duration-200"
                loading="lazy"
              />
            )}
            <div className="absolute inset-0 bg-gradient-to-t from-black/70 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300 flex flex-col justify-end p-3">
              <div className="text-white text-sm font-medium line-clamp-2">
                {post.caption || "Без підпису"}
              </div>
            </div>
          </div>
        );
      })}
    </div>
  );

  return (
    <>
      {renderGrid()}
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

export default PostsTab;