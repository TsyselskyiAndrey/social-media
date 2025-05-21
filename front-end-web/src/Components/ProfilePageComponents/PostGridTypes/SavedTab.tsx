import React, { useState } from "react";
import PostModal from "./PostModal";

const savedImages = Array.from({ length: 9 }, (_, i) =>
  `https://picsum.photos/seed/saved${i}/600/600`
);

const SavedTab: React.FC = () => {
  const [modalOpen, setModalOpen] = useState(false);
  const [selectedImage, setSelectedImage] = useState<string | null>(null);

  const openModal = (src: string) => {
    setSelectedImage(src);
    setModalOpen(true);
  };

  const closeModal = () => {
    setModalOpen(false);
    setSelectedImage(null);
  };

  return (
    <div className="grid grid-cols-3 gap-2">
      {savedImages.map((src, i) => (
        <div
          key={i}
          className="aspect-square overflow-hidden rounded-md cursor-pointer bg-gray-100"
          onClick={() => openModal(src)}
        >
          <img
            src={src}
            alt={`saved-post-${i}`}
            className="object-cover w-full h-full hover:scale-105 transition-transform duration-200"
            loading="lazy"
          />
        </div>
      ))}

      {selectedImage && (
        <PostModal isOpen={modalOpen} onClose={closeModal} imageSrc={selectedImage} />
      )}
    </div>
  );
};

export default SavedTab;