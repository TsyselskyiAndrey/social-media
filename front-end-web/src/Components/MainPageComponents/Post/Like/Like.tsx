import React, { useState } from "react";
import UnlikeIcon from "../../..//../Assets/Post/likeAsStar.png";      // твоя іконка лайка
import LikeIcon from "../../../../Assets/Post/likeAsStarActive.png";  // іконка "не лайк" або пусте серце

type LikeProps = {
  initialCount?: number;
};

const Like: React.FC<LikeProps> = ({ initialCount = 0 }) => {
  const [liked, setLiked] = useState(false);
  const [likeCount, setLikeCount] = useState(initialCount);

  const toggleLike = () => {
    if (liked) {
      setLiked(false);
      setLikeCount((prev) => prev - 1);
    } else {
      setLiked(true);
      setLikeCount((prev) => prev + 1);
    }
  };

  return (
    <button onClick={toggleLike} className="flex items-center gap-2">
      <img
        src={liked ? LikeIcon : UnlikeIcon}
        alt={liked ? "Liked" : "Not liked"}
        className="w-6 h-6"
      />
      <span>{likeCount}</span>
    </button>
  );
};

export default Like;