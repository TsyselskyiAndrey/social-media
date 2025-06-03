import React, { useState } from "react";
import { useSwipeable } from "react-swipeable";
import { PostMedia } from "../../../../API/agent";
import AutoPlayVideo from "../Video/AutoplayVideo";

interface MediaCarouselProps {
  medias: PostMedia[];
}

const MediaCarousel: React.FC<MediaCarouselProps> = ({ medias }) => {
  const [currentIndex, setCurrentIndex] = useState(0);

  const count = medias.length;

  const handlers = useSwipeable({
    onSwipedLeft: () =>
      setCurrentIndex((prev) => (prev + 1) % count),
    onSwipedRight: () =>
      setCurrentIndex((prev) => (prev - 1 + count) % count),
    trackMouse: true,
  });

  const currentMedia = medias[currentIndex];

  return (
    <div {...handlers} className="relative w-full overflow-hidden select-none">
      <div className="absolute top-2 left-2 bg-black bg-opacity-50 text-white text-xs px-2 py-1 rounded z-10">
        {currentIndex + 1} / {count}
      </div>

      {currentMedia.postMediaType === "Photo" ? (
        <img
          src={currentMedia.mediaUrl}
          alt="Post media"
          className="w-full h-auto object-cover"
          draggable={false}
        />
      ) : (
        <AutoPlayVideo
          src={currentMedia.mediaUrl}
          poster={currentMedia.thumbnailUrl ?? undefined}
          controls
          className="w-full h-auto object-cover"
        />
      )}

      {count > 1 && (
        <>
          <button
            onClick={() =>
              setCurrentIndex((i) => (i - 1 + count) % count)
            }
            className="absolute top-1/2 left-2 transform -translate-y-1/2 bg-black/50 text-white rounded-full p-2 z-10 select-none"
            aria-label="Previous media"
          >
            ←
          </button>
          <button
            onClick={() =>
              setCurrentIndex((i) => (i + 1) % count)
            }
            className="absolute top-1/2 right-2 transform -translate-y-1/2 bg-black/50 text-white rounded-full p-2 z-10 select-none"
            aria-label="Next media"
          >
            →
          </button>
        </>
      )}

      {count > 1 && (
        <div className="absolute bottom-2 left-1/2 transform -translate-x-1/2 flex gap-1 z-10">
          {medias.map((_, i) => (
            <div
              key={i}
              className={`w-2 h-2 rounded-full ${
                i === currentIndex ? "bg-white" : "bg-gray-400"
              }`}
            />
          ))}
        </div>
      )}
    </div>
  );
};

export default MediaCarousel;
