import React, { useEffect, useState } from "react";
import Agent from "../../../API/agent";
import { Post } from "../../../API/agent";
import PostModal from "./PostModal";

const SavedTab: React.FC = () => {
  const [hoveredVideoId, setHoveredVideoId] = useState<number | null>(null);
  const [savedPosts, setSavedPosts] = useState<Post[]>([]);
  const [modalOpen, setModalOpen] = useState(false);
  const [selectedPost, setSelectedPost] = useState<Post | null>(null);

  useEffect(() => {
    const fetchSavedPosts = async () => {
      try {
        const response = await Agent.Posts.getSavedPosts();
        setSavedPosts(response.data);
      } catch (error) {
        console.error("Помилка при завантаженні збережених постів:", error);
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

  return (
    <div className="grid grid-cols-3 gap-2">
      {savedPosts.map((post) => {
        const media = post.postMedias[0];
        if (!media) return null;

        return (
          <div
            key={post.id}
            className="aspect-square overflow-hidden rounded-md cursor-pointer bg-gray-100"
            onClick={() => openModal(post)}
          >
            {media.postMediaType.startsWith("Video") ? (
              <video
                src={media.mediaUrl}
                className={`object-cover w-full h-full transition-transform duration-200 ${
                  hoveredVideoId === post.id ? "scale-105 z-10" : ""
                }`}
                muted={hoveredVideoId !== post.id}
                loop
                playsInline
                autoPlay={hoveredVideoId === post.id}
                onMouseEnter={() => setHoveredVideoId(post.id)}
                onMouseLeave={() => setHoveredVideoId(null)}
              />
            ): (
              <img
                src={media.mediaUrl}
                alt={`saved-post-${post.id}`}
                className="object-cover w-full h-full hover:scale-105 transition-transform duration-200"
                loading="lazy"
              />
            )}
          </div>
        );
      })}

      {selectedPost && (
        <PostModal
          isOpen={modalOpen}
          onClose={closeModal}
          post={selectedPost}
        />
      )}
    </div>
  );
};

export default SavedTab;
