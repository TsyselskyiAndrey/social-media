import React, { useState } from "react";
import { useSwipeable } from "react-swipeable";
import { PostMedia } from "../../../../API/agent";
import AutoPlayVideo from "../Video/AutoplayVideo";
import ArrowBackIosNewIcon from "@mui/icons-material/ArrowBackIosNew";
import ArrowForwardIosIcon from "@mui/icons-material/ArrowForwardIos";

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
            className="absolute top-1/2 left-2 transform -translate-y-1/2 bg-black/60 hover:bg-black/80 text-white rounded-full p-1.5 z-10 select-none transition-all duration-300 ease-in-out"
            aria-label="Previous media"
          >
            <ArrowBackIosNewIcon fontSize="small" />
          </button>
          <button
            onClick={() =>
              setCurrentIndex((i) => (i + 1) % count)
            }
            className="absolute top-1/2 right-2 transform -translate-y-1/2 bg-black/60 hover:bg-black/80 text-white rounded-full p-1.5 z-10 select-none transition-all duration-300 ease-in-out"
            aria-label="Next media"
          >
            <ArrowForwardIosIcon fontSize="small" />
          </button>
        </>
      )}

      {count > 1 && (
        <div className="absolute bottom-2 left-1/2 transform -translate-x-1/2 flex gap-1.5 z-10">
          {medias.map((_, i) => (
            <div
              key={i}
              className={`w-2 h-2 rounded-full transition-all duration-300 ${i === currentIndex ? "bg-white scale-125" : "bg-gray-400 hover:bg-gray-300"}`}
              onClick={() => setCurrentIndex(i)}
            />
          ))}
        </div>
      )}
    </div>
  );
};

export default MediaCarousel;
