import React, { ChangeEvent, useRef, useState } from 'react';

type PostMediaFile = File & { previewUrl: string };

interface PreviewStepProps {
  postMedias: PostMediaFile[];
  thumbnail: File | null;
  caption: string;
  tags: string[];
  addFiles: (files: FileList | File[]) => void;
  onRemoveMedia: (index: number) => void;
  onBack: () => void;
  onSubmit: () => void;
  onClose: () => void;
}

const PreviewStep: React.FC<PreviewStepProps> = ({
  postMedias,
  thumbnail,
  caption,
  tags,
  addFiles,
  onRemoveMedia,
  onBack,
  onSubmit,
}) => {
  const [currentIndex, setCurrentIndex] = useState(0);
  const currentMedia = postMedias[currentIndex];

  const handlePrev = () => {
    if (currentIndex > 0) setCurrentIndex((prev) => prev - 1);
  };

  const handleNext = () => {
    if (currentIndex < postMedias.length - 1) setCurrentIndex((prev) => prev + 1);
  };

  const onFileChange = (e: ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      addFiles(e.target.files);
    }
  };

  return (
    <div className="bg-white dark:bg-gray-900 rounded-lg shadow-lg p-6 w-full max-w-[1200px] mx-auto font-sans flex gap-6 transition-colors">
      <div className="w-2/3 relative flex items-center justify-center">
        {currentMedia && (
          <div className="relative w-full h-[550px] rounded-lg overflow-hidden border border-gray-300 dark:border-gray-600 flex items-center justify-center">
            {currentMedia.type.startsWith('image') ? (
              <img
                src={currentMedia.previewUrl}
                alt="preview"
                className="object-contain max-h-full max-w-full"
              />
            ) : (
              <video
                src={currentMedia.previewUrl}
                controls
                className="object-contain max-h-full max-w-full"
              />
            )}

            <button
              onClick={() => {
                onRemoveMedia(currentIndex);
                setCurrentIndex((prev) => (prev > 0 ? prev - 1 : 0));
              }}
              className="absolute top-2 right-2 bg-white dark:bg-gray-800 text-red-600 hover:bg-red-500 hover:text-white p-2 rounded-full transition"
              title="Видалити медіа"
            >
              ×
            </button>
          </div>
        )}

        {postMedias.length > 1 && (
          <>
            <button
              onClick={handlePrev}
              disabled={currentIndex === 0}
              className="absolute left-2 bg-blue-600 hover:bg-blue-700 text-white p-2 rounded-full disabled:opacity-50 disabled:cursor-not-allowed"
              title="Попереднє"
            >
              &#8592;
            </button>
            <button
              onClick={handleNext}
              disabled={currentIndex === postMedias.length - 1}
              className="absolute right-2 bg-blue-600 hover:bg-blue-700 text-white p-2 rounded-full disabled:opacity-50 disabled:cursor-not-allowed"
              title="Наступне"
            >
              &#8594;
            </button>
          </>
        )}
      </div>

      <div className="w-1/3 flex flex-col justify-between">
        <div className="mb-4 text-gray-700 dark:text-gray-200 break-words max-w-full space-y-2">
          <p>
            <span className="font-semibold">Підпис:</span>{' '}
            {caption || '—'}
          </p>
          <p>
            <span className="font-semibold">Теги:</span>{' '}
            {tags.length > 0 ? tags.join(', ') : '—'}
          </p>
        </div>

        {postMedias.length < 10 && (
          <div className="mb-6">
            <h3 className="text-gray-800 dark:text-gray-100 font-semibold mb-2">Додати ще медіа</h3>
            <input
              type="file"
              accept="image/*,video/*"
              multiple
              onChange={onFileChange}
              className="block w-full text-sm text-gray-700 dark:text-gray-300 file:hidden"
            />
          </div>
        )}

        <div className="flex justify-between gap-4">
          <button
            onClick={onBack}
            className="w-full py-2 px-4 bg-gray-200 hover:bg-gray-300 dark:bg-gray-700 dark:hover:bg-gray-600 text-gray-800 dark:text-white font-semibold rounded-md transition"
          >
            Назад
          </button>
          <button
            onClick={onSubmit}
            className="w-full py-2 px-4 bg-blue-600 hover:bg-blue-700 text-white font-semibold rounded-md transition"
          >
            Опублікувати
          </button>
        </div>
      </div>
    </div>
  );
};

export default PreviewStep;
