import React, { useState } from "react";
import { Heart } from "lucide-react";

type Reply = {
  id: number;
  text: string;
  username: string;
  avatarUrl: string;
  likes: number;
  replies: Reply[];
};

type Comment = {
  id: number;
  text: string;
  username: string;
  avatarUrl: string;
  likes: number;
  replies: Reply[];
};

type CommentsProps = {
  initialComments?: Comment[];
  showCommentInput: boolean;
};

const Comments: React.FC<CommentsProps> = ({
  initialComments = [],
  showCommentInput,
}) => {
  const [comments, setComments] = useState<Comment[]>(initialComments);
  const [commentInput, setCommentInput] = useState("");
  const [showAll, setShowAll] = useState(false);
  const [activeReplyTarget, setActiveReplyTarget] = useState<
    { type: "comment" | "reply"; id: number; parentId?: number } | null
  >(null);
  const [replyInput, setReplyInput] = useState("");

  const commentsToShow = showAll ? comments : comments.slice(0, 3);

  const addComment = () => {
    if (commentInput.trim() === "") return;
    setComments((prev) => [
      ...prev,
      {
        id: prev.length + 1,
        text: commentInput.trim(),
        username: "New User",
        avatarUrl: "https://via.placeholder.com/40",
        likes: 0,
        replies: [],
      },
    ]);
    setCommentInput("");
  };

  const addReply = () => {
    if (replyInput.trim() === "" || !activeReplyTarget) return;

    const newReply: Reply = {
      id: Date.now(),
      text: replyInput.trim(),
      username: "New User",
      avatarUrl: "https://via.placeholder.com/40",
      likes: 0,
      replies: [],
    };

    if (activeReplyTarget.type === "comment") {
      setComments((prev) =>
        prev.map((c) =>
          c.id === activeReplyTarget.id
            ? { ...c, replies: [...c.replies, newReply] }
            : c
        )
      );
    } else if (activeReplyTarget.type === "reply") {
      setComments((prev) =>
        prev.map((c) =>
          c.id === activeReplyTarget.parentId
            ? {
                ...c,
                replies: c.replies.map((r) =>
                  r.id === activeReplyTarget.id
                    ? { ...r, replies: [...r.replies, newReply] }
                    : r
                ),
              }
            : c
        )
      );
    }

    setReplyInput("");
    setActiveReplyTarget(null);
  };

  const toggleLikeComment = (commentId: number) => {
    setComments((prev) =>
      prev.map((c) =>
        c.id === commentId ? { ...c, likes: c.likes === 0 ? 1 : 0 } : c
      )
    );
  };

  const toggleLikeReply = (commentId: number, replyId: number) => {
    setComments((prev) =>
      prev.map((c) =>
        c.id === commentId
          ? {
              ...c,
              replies: c.replies.map((r) =>
                r.id === replyId ? { ...r, likes: r.likes === 0 ? 1 : 0 } : r
              ),
            }
          : c
      )
    );
  };

  const renderReplies = (replies: Reply[], parentCommentId: number, level = 1) =>
    replies.map((r) => (
      <div key={r.id} className={`ml-${level * 6} mt-3 space-y-1`}>
        <div className="flex items-center gap-3">
          <img
            src={r.avatarUrl}
            alt="avatar"
            className="w-8 h-8 rounded-full object-cover"
          />
          <span className="font-medium">{r.username}</span>
        </div>
        <p className="ml-11">{r.text}</p>
        <div className="flex items-center gap-4 ml-11 text-sm text-gray-500">
          <button
            onClick={() =>
              setActiveReplyTarget({
                type: "reply",
                id: r.id,
                parentId: parentCommentId,
              })
            }
            className="hover:text-blue-600 font-medium"
          >
            Reply
          </button>
          <button
            className="flex items-center gap-1 hover:text-red-500"
            onClick={() => toggleLikeReply(parentCommentId, r.id)}
          >
            <Heart size={16} />
            <span>{r.likes}</span>
          </button>
        </div>

        {/* Nested Replies */}
        {r.replies.length > 0 &&
          renderReplies(r.replies, parentCommentId, level + 1)}
      </div>
    ));

  if (!showCommentInput && comments.length === 0) {
    return null;
  }

  return (
    <div className="space-y-4">
      {commentsToShow.map((c) => (
        <div
          key={c.id}
          className="flex flex-col gap-1 p-2 border rounded-lg shadow-sm"
        >
          <div className="flex items-center gap-3">
            <img
              src={c.avatarUrl}
              alt="avatar"
              className="w-10 h-10 rounded-full object-cover"
            />
            <span className="font-semibold">{c.username}</span>
          </div>

          <p className="ml-12">{c.text}</p>

          <div className="flex items-center justify-between ml-12 mt-1 text-sm text-gray-500">
            <div className="flex gap-4">
              <button
                onClick={() =>
                  setActiveReplyTarget({ type: "comment", id: c.id })
                }
                className="hover:text-blue-600 font-medium"
              >
                Reply
              </button>
              <span className="text-gray-400">
                {c.replies.length} {c.replies.length === 1 ? "Reply" : "Replies"}
              </span>
            </div>
            <button
              className="flex items-center gap-1 hover:text-red-500"
              onClick={() => toggleLikeComment(c.id)}
            >
              <Heart size={16} />
              <span>{c.likes}</span>
            </button>
          </div>

          {/* Replies */}
          {c.replies.length > 0 && (
            <div className="ml-12 mt-2">{renderReplies(c.replies, c.id)}</div>
          )}
        </div>
      ))}

      {activeReplyTarget && (
        <div className="flex gap-2 items-start mt-2 ml-6">
          <input
            type="text"
            value={replyInput}
            onChange={(e) => setReplyInput(e.target.value)}
            placeholder="Write a reply..."
            className="flex-grow border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
            onKeyDown={(e) => {
              if (e.key === "Enter") addReply();
            }}
          />
          <button
            onClick={addReply}
            className="bg-blue-600 text-white px-4 py-2 rounded disabled:opacity-50"
            disabled={replyInput.trim() === ""}
          >
            Send
          </button>
        </div>
      )}

      {!showAll && comments.length > 3 && (
        <button
          onClick={() => setShowAll(true)}
          className="text-blue-600 font-medium hover:underline ml-2"
        >
          Show more
        </button>
      )}

      {showCommentInput && (
        <div className="flex gap-2 items-start mt-2">
          <input
            type="text"
            value={commentInput}
            onChange={(e) => setCommentInput(e.target.value)}
            placeholder="Write a comment..."
            className="flex-grow border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
            onKeyDown={(e) => {
              if (e.key === "Enter") addComment();
            }}
          />
          <button
            onClick={addComment}
            className="bg-blue-600 text-white px-4 py-2 rounded disabled:opacity-50"
            disabled={commentInput.trim() === ""}
          >
            Send
          </button>
        </div>
      )}
    </div>
  );
};

export default Comments;
