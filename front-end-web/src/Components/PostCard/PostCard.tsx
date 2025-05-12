import React, { useState } from "react";
import likeImage from "../../Assets/Post/likeAsStar.png";
import likeImageActive from "../../Assets/Post/likeAsStarActive.png";
import commentImage from "../../Assets/Post/comment.png";
import shareImage from "../../Assets/Post/share.png";
import saveImage from "../../Assets/Post/save.png";
import saveImageActive from "../../Assets/Post/saveActive.png";
import "./PostCard.css";

interface PostCardProps {
  username: string;
  userImage: string;
  postImages: string | string[];
  caption: string;
}

export default function PostCard({
  username,
  userImage,
  postImages,
  caption,
}: PostCardProps) {
  const images = Array.isArray(postImages) ? postImages : [postImages];
  const [currentIndex, setCurrentIndex] = useState(0);
  const [likes, setLikes] = useState(0);
  const [liked, setLiked] = useState(false);
  const [saved, setSaved] = useState(false);
  const [showComments, setShowComments] = useState(false);
  const [comments, setComments] = useState<string[]>([]);
  const [commentText, setCommentText] = useState("");
  const [showOptionsMenu, setShowOptionsMenu] = useState(false);

  const prevImage = () => {
    setCurrentIndex((prev) => (prev === 0 ? images.length - 1 : prev - 1));
  };

  const nextImage = () => {
    setCurrentIndex((prev) => (prev === images.length - 1 ? 0 : prev + 1));
  };

  const toggleLike = () => {
    setLiked(!liked);
    setLikes(liked ? likes - 1 : likes + 1);
  };

  const toggleSave = () => setSaved(!saved);

  const handleCommentSubmit = () => {
    if (commentText.trim()) {
      setComments([...comments, commentText]);
      setCommentText("");
    }
  };

  const toggleOptionsMenu = () => {
    setShowOptionsMenu(!showOptionsMenu);
  };

  const handleReport = () => {
    alert("Поскаржитись");
    setShowOptionsMenu(false);
  };

  const handleCancel = () => {
    setShowOptionsMenu(false);
  };

  return (
    <div className="postCard">
      <div className="postHeader">
        <div className="userInfo">
          <img src={userImage} alt="user" className="userImage" />
          <span className="username">{username}</span>
        </div>
        <div style={{ position: "relative" }}>
          <button className="optionsButton" onClick={toggleOptionsMenu}>
            ...
          </button>
          {showOptionsMenu && (
            <div className="optionsMenu">
              <button onClick={handleReport} className="report">
                Поскаржитись
              </button>
              <button onClick={handleCancel}>Скасувати</button>
            </div>
          )}
        </div>
      </div>

      <div className="postImageContainer">
        <img src={images[currentIndex]} alt="post" className="postImage" />
        {images.length > 1 && (
          <div className="carouselNav">
            <button className="navButton left" onClick={prevImage}>
              ‹
            </button>
            <button className="navButton right" onClick={nextImage}>
              ›
            </button>
          </div>
        )}
      </div>

      {images.length > 1 && (
        <div className="carouselDotsWrapper">
          <div className="carouselIndicators">
            {images.map((_, index) => (
              <span
                key={index}
                className={`carouselDot ${
                  index === currentIndex ? "active" : ""
                }`}
                onClick={() => setCurrentIndex(index)}
              ></span>
            ))}
          </div>
        </div>
      )}

      <div className="postFooter">
        <div className="postIcons">
          <button onClick={toggleLike}>
            <img
              src={liked ? likeImageActive : likeImage}
              alt="like"
              className="postIcon"
            />
          </button>
          <button onClick={() => setShowComments(!showComments)}>
            <img src={commentImage} alt="comment" className="postIcon" />
          </button>
          <button>
            <img src={shareImage} alt="share" className="postIcon" />
          </button>
          <button onClick={toggleSave} style={{ marginLeft: "auto" }}>
            <img
              src={saved ? saveImageActive : saveImage}
              alt="save"
              className="postIcon"
            />
          </button>
        </div>

        <div className="likesCount">
          <strong>{likes} likes</strong>
        </div>

        <div className="postCaption">
          <strong>{username}</strong> {caption}
        </div>

        {showComments && (
          <div className="commentsSection">
            <div className="commentsList">
              {comments.map((comment, index) => (
                <div key={index} className="comment">
                  <strong>{username}</strong> {comment}
                </div>
              ))}
            </div>
            <div className="commentInput">
              <input
                type="text"
                value={commentText}
                onChange={(e) => setCommentText(e.target.value)}
                placeholder="Add a comment..."
              />
              <button
                onClick={handleCommentSubmit}
                disabled={!commentText.trim()}
              >
                Post
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}