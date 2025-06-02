import React, { useEffect, useState } from "react";
import { Heart } from "lucide-react";
import Agent, { Comment } from "../../../../API/agent";

type Reply = {
  id: number;
  content: string;
  username: string;
  avatarUrl: string;
  likes: number;
  replies: Reply[];
};

type CommentsProps = {
  postId: number;
  showCommentInput: boolean;
};

const mapCommentToReply = (comment: Comment): Reply => ({
  id: comment.id,
  content: comment.content,
  username: comment.author.userName,
  avatarUrl: comment.author.profileImageUrl ?? "",
  likes: comment.likes,
  replies: comment.childComments ? comment.childComments.map(mapCommentToReply) : [],
});

const Comments: React.FC<CommentsProps> = ({ postId, showCommentInput }) => {
  const [comments, setComments] = useState<Reply[]>([]);
  const [commentInput, setCommentInput] = useState("");
  const [replyInput, setReplyInput] = useState("");
  const [showAll, setShowAll] = useState(false);
  const [activeReplyTarget, setActiveReplyTarget] = useState<{
    type: "comment" | "reply";
    id: number;
    parentId?: number;
  } | null>(null);

  useEffect(() => {
    const loadComments = async () => {
      try {
        const res = await Agent.Comments.getComments(postId);
        const mapped = res.data.map(mapCommentToReply);
        setComments(mapped);
      } catch (error) {
        console.error("Failed to load comments", error);
      }
    };
    loadComments();
  }, [postId]);

  const handleCreateComment = async () => {
    if (!commentInput.trim()) return;
    try {
      const res = await Agent.Comments.createComment({
        content: commentInput,
        postId,
      });
      const newComment = mapCommentToReply(res);
      setComments((prev) => [...prev, newComment]);
      setCommentInput("");
    } catch (error) {
      console.error("Failed to add comment", error);
    }
  };

  const addReplyRecursive = (comments: Reply[], parentId: number, newReply: Reply): Reply[] => {
    return comments.map((comment) => {
      if (comment.id === parentId) {
        return {
          ...comment,
          replies: [...comment.replies, newReply],
        };
      }
      return {
        ...comment,
        replies: addReplyRecursive(comment.replies, parentId, newReply),
      };
    });
  };

  const handleCreateReply = async () => {
    if (!replyInput.trim() || !activeReplyTarget) return;

    try {
      const res = await Agent.Comments.createComment({
        content: replyInput,
        postId,
        parentCommentId: activeReplyTarget.type === "comment" ? activeReplyTarget.id : activeReplyTarget.parentId,
      });

      const newReply = mapCommentToReply(res);

      setComments((prev) =>
        addReplyRecursive(prev, activeReplyTarget.type === "comment" ? activeReplyTarget.id : activeReplyTarget.parentId!, newReply)
      );

      setReplyInput("");
      setActiveReplyTarget(null);
    } catch (error) {
      console.error("Failed to add reply", error);
    }
  };

  const toggleLike = async (id: number) => {
    try {
      const result = await Agent.Comments.likeComment({ commentId: id });
      if (typeof result.data !== "boolean") return;

      const isLiked = result.data;

      setComments((prev) =>
        prev.map((c) =>
          c.id === id
            ? { ...c, likes: c.likes + (isLiked ? 1 : -1) }
            : {
                ...c,
                replies: updateLikesRecursive(c.replies, id, isLiked),
              }
        )
      );
    } catch (error) {
      console.error("Failed to like comment", error);
    }
  };

  const updateLikesRecursive = (replies: Reply[], id: number, isLiked: boolean): Reply[] => {
    return replies.map((r) =>
      r.id === id ? { ...r, likes: r.likes + (isLiked ? 1 : -1) } : { ...r, replies: updateLikesRecursive(r.replies, id, isLiked) }
    );
  };

  const renderReplies = (replies: Reply[], level = 1, parentId?: number) =>
    replies.map((r) => (
      <div key={r.id} className={`ml-${level * 6} mt-3 space-y-1`}>
        <div className="flex items-center gap-3">
          <img src={r.avatarUrl} alt="avatar" className="w-8 h-8 rounded-full" />
          <span className="font-medium">{r.username}</span>
        </div>
        <p className="ml-11">{r.content}</p>
        <div className="flex gap-4 ml-11 text-sm text-gray-500">
          <button onClick={() => toggleLike(r.id)} className="flex items-center gap-1 hover:text-red-500">
            <Heart size={16} />
            <span>{r.likes}</span>
          </button>
        </div>

        {activeReplyTarget?.type === "reply" && activeReplyTarget?.id === r.id && activeReplyTarget?.parentId === parentId && (
          <div className="flex gap-2 items-start mt-2 ml-11">
            <input
              value={replyInput}
              onChange={(e) => setReplyInput(e.target.value)}
              placeholder="Write a reply..."
              onKeyDown={(e) => e.key === "Enter" && handleCreateReply()}
              className="flex-grow border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
            <button
              onClick={handleCreateReply}
              className="bg-blue-600 text-white px-4 py-2 rounded disabled:opacity-50"
              disabled={!replyInput.trim()}
            >
              Send
            </button>
          </div>
        )}

        {r.replies.length > 0 && renderReplies(r.replies, level + 1, r.id)}
      </div>
    ));

  const commentsToShow = showAll ? comments : comments.slice(0, 3);

  return (
    <div className="space-y-4">
      {commentsToShow.map((c) => (
        <div key={c.id} className="p-2 border rounded shadow-sm flex flex-col gap-1">
          <div className="flex items-center gap-3">
            <img src={c.avatarUrl} alt="avatar" className="w-10 h-10 rounded-full" />
            <span className="font-semibold">{c.username}</span>
          </div>
          <p className="ml-12">{c.content}</p>
          <div className="flex justify-between ml-12 mt-1 text-sm text-gray-500">
            <div className="flex gap-4">
              <button onClick={() => setActiveReplyTarget({ type: "comment", id: c.id })} className="hover:text-blue-600 font-medium">
                Reply
              </button>
              <span>
                {c.replies.length} {c.replies.length === 1 ? "Reply" : "Replies"}
              </span>
            </div>
            <button onClick={() => toggleLike(c.id)} className="flex items-center gap-1 hover:text-red-500">
              <Heart size={16} />
              <span>{c.likes}</span>
            </button>
          </div>

          {activeReplyTarget?.type === "comment" && activeReplyTarget?.id === c.id && (
            <div className="flex gap-2 items-start mt-2 ml-12">
              <input
                value={replyInput}
                onChange={(e) => setReplyInput(e.target.value)}
                placeholder="Write a reply..."
                onKeyDown={(e) => e.key === "Enter" && handleCreateReply()}
                className="flex-grow border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
              />
              <button
                onClick={handleCreateReply}
                className="bg-blue-600 text-white px-4 py-2 rounded disabled:opacity-50"
                disabled={!replyInput.trim()}
              >
                Send
              </button>
            </div>
          )}

          {c.replies.length > 0 && <div className="ml-12 mt-2">{renderReplies(c.replies, 1, c.id)}</div>}
        </div>
      ))}

      {!showAll && comments.length > 3 && (
        <button onClick={() => setShowAll(true)} className="text-blue-600 font-medium hover:underline ml-2">
          Show more
        </button>
      )}

      {showCommentInput && (
        <div className="flex gap-2 items-start mt-2">
          <input
            value={commentInput}
            onChange={(e) => setCommentInput(e.target.value)}
            placeholder="Write a comment..."
            onKeyDown={(e) => e.key === "Enter" && handleCreateComment()}
            className="flex-grow border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          <button
            onClick={handleCreateComment}
            className="bg-blue-600 text-white px-4 py-2 rounded disabled:opacity-50"
            disabled={!commentInput.trim()}
          >
            Send
          </button>
        </div>
      )}
    </div>
  );
};

export default Comments;
