import React, { useState } from "react";

type Comment = {
  id: number;
  text: string;
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

  const addComment = () => {
    if (commentInput.trim() === "") return;
    setComments((prev) => [
      ...prev,
      { id: prev.length + 1, text: commentInput.trim() },
    ]);
    setCommentInput("");
  };

  // Визначаємо які коментарі показувати
  const commentsToShow = showAll ? comments : comments.slice(0, 3);

  // Якщо немає коментарів і поле вводу не показувати — нічого не рендеримо
  if (!showCommentInput && comments.length === 0) {
    return null;
  }

  return (
    <div>
      <div>
        {commentsToShow.map((c) => (
          <p key={c.id} style={{ marginBottom: 4, paddingLeft: 8 }}>
            {c.text}
          </p>
        ))}
      </div>

      {/* Кнопка "Показати більше" */}
      {!showAll && comments.length > 3 && (
        <button
          onClick={() => setShowAll(true)}
          style={{
            background: "none",
            border: "none",
            color: "#1e90ff",
            cursor: "pointer",
            padding: 0,
            fontWeight: "bold",
            marginBottom: 8,
            paddingLeft: 8,
          }}
        >
          Показати більше
        </button>
      )}

      {/* Поле для додавання нового коментаря */}
      {showCommentInput && (
        <div className="mt-2 flex gap-2" style={{ paddingLeft: 8 }}>
          <input
            type="text"
            value={commentInput}
            onChange={(e) => setCommentInput(e.target.value)}
            placeholder="Напишіть коментар..."
            className="flex-grow border rounded px-3 py-1 focus:outline-none focus:ring-2 focus:ring-blue-500"
            onKeyDown={(e) => {
              if (e.key === "Enter") {
                addComment();
              }
            }}
          />
          <button
            onClick={addComment}
            className="bg-blue-600 text-white px-4 rounded disabled:opacity-50"
            disabled={commentInput.trim() === ""}
          >
            Відправити
          </button>
        </div>
      )}
    </div>
  );
};

export default Comments;
