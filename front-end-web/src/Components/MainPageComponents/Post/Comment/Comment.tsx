import React, { useEffect, useState } from "react";
import { Heart, MoreVertical } from "lucide-react";
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
  const [activeReplyTarget, setActiveReplyTarget] = useState<{ type: "comment" | "reply"; id: number; parentId?: number } | null>(null);
  const [editingTarget, setEditingTarget] = useState<{ id: number; content: string } | null>(null);
  const [menuOpen, setMenuOpen] = useState<number | null>(null);

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
      const res = await Agent.Comments.createComment({ content: commentInput, postId });
      const newComment = mapCommentToReply(res);
      setComments((prev) => [...prev, newComment]);
      setCommentInput("");
    } catch (error) {
      console.error("Failed to add comment", error);
    }
  };

  const addReplyRecursive = (comments: Reply[], parentId: number, newReply: Reply): Reply[] =>
    comments.map((comment) =>
      comment.id === parentId
        ? { ...comment, replies: [...comment.replies, newReply] }
        : { ...comment, replies: addReplyRecursive(comment.replies, parentId, newReply) }
    );

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
      const updateLikesRecursive = (replies: Reply[]): Reply[] =>
        replies.map((r) =>
          r.id === id ? { ...r, likes: r.likes + (isLiked ? 1 : -1) } : { ...r, replies: updateLikesRecursive(r.replies) }
        );
      setComments((prev) =>
        prev.map((c) =>
          c.id === id ? { ...c, likes: c.likes + (isLiked ? 1 : -1) } : { ...c, replies: updateLikesRecursive(c.replies) }
        )
      );
    } catch (error) {
      console.error("Failed to like comment", error);
    }
  };

  const handleEditComment = async (id: number, content: string) => {
    try {
      await Agent.Comments.editComment({ commentId: id, content });
      const updateContentRecursive = (replies: Reply[]): Reply[] =>
        replies.map((r) =>
          r.id === id ? { ...r, content } : { ...r, replies: updateContentRecursive(r.replies) }
        );
      setComments((prev) =>
        prev.map((c) =>
          c.id === id ? { ...c, content } : { ...c, replies: updateContentRecursive(c.replies) }
        )
      );
      setEditingTarget(null);
    } catch (error) {
      console.error("Failed to edit comment", error);
    }
  };

  const handleDeleteComment = async (id: number) => {
    if (!window.confirm("Are you sure you want to delete this comment?")) return;
    try {
      await Agent.Comments.deleteComment({ commentId: id });
      const deleteRecursive = (items: Reply[]): Reply[] =>
        items.filter((item) => item.id !== id).map((item) => ({ ...item, replies: deleteRecursive(item.replies) }));
      setComments((prev) => deleteRecursive(prev));
    } catch (error) {
      console.error("Failed to delete comment", error);
    }
  };

  const renderReplies = (replies: Reply[], level = 1, parentId?: number) =>
    replies.map((r) => (
      <div key={r.id} className={`ml-${level * 6} mt-3 space-y-1 relative`}>
        <div className="flex items-center gap-3">
          <img src={r.avatarUrl} alt="avatar" className="w-8 h-8 rounded-full" />
          <span className="font-medium text-gray-800 dark:text-gray-200">{r.username}</span>
          <button onClick={() => setMenuOpen(menuOpen === r.id ? null : r.id)} className="ml-auto">
            <MoreVertical size={18} className="text-gray-500 dark:text-gray-400" />
          </button>
          {menuOpen === r.id && (
            <div className="absolute right-0 top-6 bg-white dark:bg-cyan-950 border border-gray-200 dark:border-cyan-900 rounded shadow w-28 z-10">
              <button onClick={() => { setEditingTarget({ id: r.id, content: r.content }); setMenuOpen(null); }} className="block w-full px-3 py-2 text-left hover:bg-gray-100 dark:hover:bg-cyan-900 text-gray-700 dark:text-gray-200">Edit</button>
              <button onClick={() => { handleDeleteComment(r.id); setMenuOpen(null); }} className="block w-full px-3 py-2 text-left hover:bg-gray-100 dark:hover:bg-cyan-900 text-red-600 dark:text-red-400">Delete</button>
            </div>
          )}
        </div>
        {editingTarget?.id === r.id ? (
          <div className="ml-11 flex gap-2 mt-2">
            <input
              value={editingTarget.content}
              onChange={(e) => setEditingTarget({ ...editingTarget, content: e.target.value })}
              onKeyDown={(e) => e.key === "Enter" && handleEditComment(r.id, editingTarget.content)}
              className="flex-grow border rounded px-2 py-1 dark:bg-cyan-950 dark:border-cyan-800 dark:text-gray-100"
            />
            <button onClick={() => handleEditComment(r.id, editingTarget.content)} className="bg-teal-600 text-white px-3 py-1 rounded">Save</button>
          </div>
        ) : (
          <p className="ml-11 text-gray-800 dark:text-gray-200">{r.content}</p>
        )}
        <div className="flex gap-4 ml-11 text-sm text-gray-500 dark:text-gray-400">
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
              onKeyDown={(e) => e.key === "Enter" && handleCreateReply()}
              placeholder="Write a reply..."
              className="flex-grow border rounded px-2 py-1 dark:bg-cyan-950 dark:border-cyan-800 dark:text-gray-100"
            />
            <button onClick={handleCreateReply} className="bg-teal-600 text-white px-3 py-1 rounded">Send</button>
          </div>
        )}
        {r.replies.length > 0 && renderReplies(r.replies, level + 1, r.id)}
      </div>
    ));

  const commentsToShow = showAll ? comments : comments.slice(0, 3);

  return (
    <div className="space-y-4">
      {commentsToShow.map((c) => (
        <div key={c.id} className="p-3 border rounded shadow-sm flex flex-col gap-1 relative bg-white dark:bg-cyan-950 border-gray-100 dark:border-cyan-900">
          <div className="flex items-center gap-3">
            <img src={c.avatarUrl} alt="avatar" className="w-10 h-10 rounded-full" />
            <span className="font-semibold text-gray-900 dark:text-gray-100">{c.username}</span>
            <button onClick={() => setMenuOpen(menuOpen === c.id ? null : c.id)} className="ml-auto">
              <MoreVertical size={20} className="text-gray-500 dark:text-gray-400" />
            </button>
            {menuOpen === c.id && (
              <div className="absolute right-2 top-12 bg-white dark:bg-cyan-950 border border-gray-200 dark:border-cyan-900 rounded shadow w-28 z-10">
                <button onClick={() => { setEditingTarget({ id: c.id, content: c.content }); setMenuOpen(null); }} className="block w-full px-3 py-2 text-left hover:bg-gray-100 dark:hover:bg-cyan-900 text-gray-700 dark:text-gray-200">Edit</button>
                <button onClick={() => { handleDeleteComment(c.id); setMenuOpen(null); }} className="block w-full px-3 py-2 text-left hover:bg-gray-100 dark:hover:bg-cyan-900 text-red-600 dark:text-red-400">Delete</button>
              </div>
            )}
          </div>
          {editingTarget?.id === c.id ? (
            <div className="ml-12 flex gap-2 mt-2">
              <input
                value={editingTarget.content}
                onChange={(e) => setEditingTarget({ ...editingTarget, content: e.target.value })}
                onKeyDown={(e) => e.key === "Enter" && handleEditComment(c.id, editingTarget.content)}
                className="flex-grow border rounded px-2 py-1 dark:bg-cyan-950 dark:border-cyan-800 dark:text-gray-100"
              />
              <button onClick={() => handleEditComment(c.id, editingTarget.content)} className="bg-teal-600 text-white px-3 py-1 rounded">Save</button>
            </div>
          ) : (
            <p className="ml-12 text-gray-800 dark:text-gray-200">{c.content}</p>
          )}
          <div className="flex justify-between ml-12 mt-1 text-sm text-gray-500 dark:text-gray-400">
            <div className="flex gap-4">
              <button onClick={() => setActiveReplyTarget({ type: "comment", id: c.id })} className="hover:text-teal-600 font-medium">Reply</button>
              <span>{c.replies.length} {c.replies.length === 1 ? "Reply" : "Replies"}</span>
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
                onKeyDown={(e) => e.key === "Enter" && handleCreateReply()}
                placeholder="Write a reply..."
                className="flex-grow border rounded px-2 py-1 dark:bg-cyan-950 dark:border-cyan-800 dark:text-gray-100"
              />
              <button onClick={handleCreateReply} className="bg-teal-600 text-white px-3 py-1 rounded">Send</button>
            </div>
          )}
          {c.replies.length > 0 && <div className="ml-12 mt-2">{renderReplies(c.replies, 1, c.id)}</div>}
        </div>
      ))}
      {!showAll && comments.length > 3 && (
        <button onClick={() => setShowAll(true)} className="text-teal-600 font-medium hover:underline ml-2">Show more</button>
      )}
      {showCommentInput && (
        <div className="flex gap-2 items-start mt-2">
          <input
            value={commentInput}
            onChange={(e) => setCommentInput(e.target.value)}
            onKeyDown={(e) => e.key === "Enter" && handleCreateComment()}
            placeholder="Write a comment..."
            className="flex-grow border rounded px-3 py-2 dark:bg-cyan-950 dark:border-cyan-800 dark:text-gray-100"
          />
          <button onClick={handleCreateComment} className="bg-teal-600 text-white px-4 py-2 rounded disabled:opacity-50" disabled={!commentInput.trim()}>Send</button>
        </div>
      )}
    </div>
  );
};

export default Comments;
