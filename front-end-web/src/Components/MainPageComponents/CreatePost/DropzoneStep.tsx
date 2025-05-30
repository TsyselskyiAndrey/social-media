import React, { ChangeEvent, DragEvent, useRef, useState } from 'react';

type PostMediaFile = File & { previewUrl: string };

interface DropzoneStepProps {
  postMedias: PostMediaFile[];
  addFiles: (files: FileList | File[]) => void;
  removeFile: (index: number) => void;
  onNext: () => void;
  maxFiles: number;
  onClose: () => void;
}

const DropzoneStep: React.FC<DropzoneStepProps> = ({
  postMedias,
  addFiles,
  removeFile,
  onNext,
  maxFiles,
}) => {
  const inputRef = useRef<HTMLInputElement | null>(null);
  const [isDragging, setIsDragging] = useState(false);

  const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      addFiles(e.target.files);
    }
  };

  const handleDrop = (e: DragEvent<HTMLDivElement>) => {
    e.preventDefault();
    setIsDragging(false);
    if (e.dataTransfer.files) {
      addFiles(e.dataTransfer.files);
    }
  };

  return (
    <div className="bg-white dark:bg-gray-900 rounded-lg shadow-lg p-6 max-w-3xl mx-auto font-sans transition-colors">
      <h2 className="text-xl font-semibold text-gray-800 dark:text-gray-100 mb-4">
        Завантажте фото чи відео (до {maxFiles})
      </h2>

      <div
        onDragOver={(e) => {
          e.preventDefault();
          setIsDragging(true);
        }}
        onDragLeave={() => setIsDragging(false)}
        onDrop={handleDrop}
        onClick={() => inputRef.current?.click()}
        className={`border-2 border-dashed ${
          isDragging
            ? 'border-blue-500 bg-blue-50 dark:bg-blue-950'
            : 'border-gray-300 dark:border-gray-600'
        } rounded-xl p-6 text-center cursor-pointer transition-colors mb-6`}
      >
        <p className="text-gray-600 dark:text-gray-300">
          Перетягніть файли сюди або натисніть для вибору
        </p>
        <input
          ref={inputRef}
          type="file"
          accept="image/*,video/*"
          multiple
          onChange={handleFileChange}
          disabled={postMedias.length >= maxFiles}
          className="hidden"
        />
      </div>

      <div className="flex flex-wrap gap-4 mb-6">
        {postMedias.map((file, i) => (
          <div
            key={i}
            className="relative w-24 aspect-square rounded-md overflow-hidden border border-gray-300 dark:border-gray-600"
          >
            {file.type.startsWith('image') ? (
              <img
                src={file.previewUrl}
                alt="preview"
                className="object-cover w-full h-full"
              />
            ) : (
              <video
                src={file.previewUrl}
                controls
                className="object-cover w-full h-full"
              />
            )}
            <button
              onClick={() => removeFile(i)}
              className="absolute top-1 right-1 bg-red-600 text-white rounded-full w-6 h-6 flex items-center justify-center text-xs hover:bg-red-700"
              aria-label="Видалити файл"
            >
              ×
            </button>
          </div>
        ))}
      </div>

      <button
        onClick={onNext}
        disabled={postMedias.length === 0}
        className="w-full py-2 px-4 bg-blue-600 hover:bg-blue-700 text-white font-semibold rounded-md disabled:bg-gray-400 disabled:cursor-not-allowed transition"
      >
        Далі
      </button>
    </div>
  );
};

export default DropzoneStep;
