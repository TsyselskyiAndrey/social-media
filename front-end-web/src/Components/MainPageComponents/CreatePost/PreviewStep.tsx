import React, { useState, useMemo, ChangeEvent } from "react";

interface PreviewStepProps {
  media: File[];
  setMedia: (files: File[]) => void;
  onBack: () => void;
  onPublish: () => void;
}

const PreviewStep: React.FC<PreviewStepProps> = ({
  media,
  setMedia,
  onBack,
  onPublish,
}) => {
  const [caption, setCaption] = useState("");
  const [currentIndex, setCurrentIndex] = useState(0);

  const currentFile = media[currentIndex];
  const mediaURL = useMemo(() => URL.createObjectURL(currentFile), [currentFile]);
  const isImage = currentFile.type.startsWith("image");

  const handleAddFiles = (e: ChangeEvent<HTMLInputElement>) => {
    const newFiles = Array.from(e.target.files || []);
    const combined = [...media, ...newFiles].slice(0, 10); // максимум 10
    setMedia(combined);
  };

  const goPrev = () => setCurrentIndex((prev) => Math.max(prev - 1, 0));
  const goNext = () => setCurrentIndex((prev) => Math.min(prev + 1, media.length - 1));

  return (
    <div>
      <div className="mb-4 relative">
        {isImage ? (
          <img src={mediaURL} alt="preview" className="rounded w-full" />
        ) : (
          <video src={mediaURL} controls className="rounded w-full" />
        )}
        <div className="absolute top-2 right-2 bg-black text-white px-2 py-1 text-sm rounded">
          {currentIndex + 1} з {media.length}
        </div>
      </div>

      <div className="flex justify-between mb-4">
        <button
          onClick={goPrev}
          disabled={currentIndex === 0}
          className="px-4 py-2 rounded bg-gray-300 hover:bg-gray-400 disabled:opacity-50"
        >
          ←
        </button>
        <button
          onClick={goNext}
          disabled={currentIndex === media.length - 1}
          className="px-4 py-2 rounded bg-gray-300 hover:bg-gray-400 disabled:opacity-50"
        >
          →
        </button>
      </div>

      <div className="mb-4">
        <label className="cursor-pointer inline-block bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700">
          Додати ще
          <input
            type="file"
            accept="image/*,video/*"
            multiple
            onChange={handleAddFiles}
            className="hidden"
          />
        </label>
      </div>

      <textarea
        placeholder="Введіть опис..."
        value={caption}
        onChange={(e) => setCaption(e.target.value)}
        className="w-full border rounded p-2 mb-4 resize-none overflow-auto"
        rows={4}
        style={{ height: "100px" }}
      />

      <div className="flex justify-between">
        <button
          onClick={onBack}
          className="px-4 py-2 rounded bg-gray-300 hover:bg-gray-400"
        >
          Назад
        </button>
        <button
          onClick={onPublish}
          className="px-4 py-2 rounded bg-blue-600 text-white hover:bg-blue-700"
        >
          Опублікувати
        </button>
      </div>
    </div>
  );
};

export default PreviewStep;
