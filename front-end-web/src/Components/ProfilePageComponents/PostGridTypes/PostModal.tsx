import React, { useState } from "react";
import { Dialog, DialogPanel } from "@headlessui/react";
import { Post } from "../../../API/agent";

interface PostModalProps {
  isOpen: boolean;
  onClose: () => void;
  post: Post;
}

const PostModal: React.FC<PostModalProps> = ({ isOpen, onClose, post }) => {
  const [likes, setLikes] = useState(0);
  const [comments, setComments] = useState<string[]>([]);
  const [commentText, setCommentText] = useState("");

  const handleLike = () => setLikes((prev) => prev + 1);

  const handleAddComment = () => {
    if (commentText.trim() === "") return;
    setComments((prev) => [...prev, commentText.trim()]);
    setCommentText("");
  };

  const media = post.postMedias[0];
  const isVideo = media?.format.startsWith("video");

  return (
    <Dialog
      open={isOpen}
      onClose={onClose}
      className="fixed inset-0 z-50 flex items-center justify-center p-4"
    >
      <div className="fixed inset-0 bg-black/60" aria-hidden="true" />

      <DialogPanel className="relative bg-white rounded-lg shadow-lg max-w-md w-full max-h-[80vh] flex flex-col overflow-hidden">
        <button
          onClick={onClose}
          className="absolute top-3 right-3 text-gray-600 hover:text-gray-900 text-3xl font-bold leading-none"
          aria-label="Close modal"
        >
          &times;
        </button>

        {isVideo ? (
          <video
            src={media.mediaUrl}
            controls
            className="w-full max-h-72 object-contain bg-black"
          />
        ) : (
          <img
            src={media.mediaUrl}
            alt="Post"
            className="w-full h-auto object-cover max-h-72"
          />
        )}

        <div className="p-4 flex flex-col gap-3 flex-grow overflow-auto">
          <h3 className="font-semibold">{post.authorName}</h3>
          {post.caption && (
            <p className="text-gray-700 text-sm">{post.caption}</p>
          )}

          <button
            onClick={handleLike}
            className="self-start text-red-600 hover:text-red-700 font-semibold"
            aria-label="Like post"
          >
            ❤️ {likes > 0 ? `Лайки: ${likes}` : "Поставити лайк"}
          </button>

          <div className="flex flex-col gap-2">
            <div className="flex gap-2">
              <input
                type="text"
                placeholder="Напиши коментар..."
                value={commentText}
                onChange={(e) => setCommentText(e.target.value)}
                className="flex-grow border rounded px-2 py-1 focus:outline-blue-500"
                onKeyDown={(e) => {
                  if (e.key === "Enter") {
                    e.preventDefault();
                    handleAddComment();
                  }
                }}
              />
              <button
                onClick={handleAddComment}
                className="bg-blue-600 text-white px-3 rounded hover:bg-blue-700 transition"
              >
                Надіслати
              </button>
            </div>

            <div className="max-h-36 overflow-auto text-gray-800 text-sm">
              {comments.length === 0 ? (
                <p className="italic text-gray-400">Немає коментарів</p>
              ) : (
                comments.map((c, i) => (
                  <p
                    key={i}
                    className="border-b border-gray-200 pb-1 last:border-0"
                  >
                    {c}
                  </p>
                ))
              )}
            </div>
          </div>
        </div>
      </DialogPanel>
    </Dialog>
  );
};

export default PostModal;
