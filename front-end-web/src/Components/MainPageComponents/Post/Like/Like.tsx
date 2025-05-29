import React, { useState } from "react";
import UnlikeIcon from "../../../../Assets/Post/likeAsStar.png";
import LikeIcon from "../../../../Assets/Post/likeAsStarActive.png";

type LikeProps = {
  initialCount?: number;
  initiallyLiked?: boolean;
  onLike?: () => Promise<boolean>;
};

const Like: React.FC<LikeProps> = ({ initialCount = 0, initiallyLiked = false, onLike }) => {
  const [liked, setLiked] = useState(initiallyLiked);
  const [likeCount, setLikeCount] = useState(initialCount);

  const toggleLike = async () => {
    if (onLike) {
      const result = await onLike();
      setLiked(result);
      setLikeCount(prev => prev + (result ? 1 : -1));
    } else {
      setLiked(prev => !prev);
      setLikeCount(prev => prev + (liked ? -1 : 1));
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

