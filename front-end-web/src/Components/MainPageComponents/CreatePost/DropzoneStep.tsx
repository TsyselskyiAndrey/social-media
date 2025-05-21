import React, { ChangeEvent } from "react";

interface DropzoneStepProps {
  onNext: () => void;
  setMedia: (files: File[]) => void;
  existingMedia: File[];
}

const DropzoneStep: React.FC<DropzoneStepProps> = ({
  onNext,
  setMedia,
  existingMedia,
}) => {
  const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
    const newFiles = Array.from(e.target.files || []);
    const combined = [...existingMedia, ...newFiles].slice(0, 10);
    setMedia(combined);
  };

  return (
    <div className="text-center p-8 border-2 border-dashed border-gray-300 rounded-lg">
      <h2 className="text-xl font-semibold mb-4">Створити допис</h2>
      <p className="mb-4 text-gray-500">Перетягніть фото або відео сюди</p>
      <label className="cursor-pointer inline-block bg-blue-600 text-white px-4 py-2 rounded">
        Обрати файли
        <input
          type="file"
          accept="image/*,video/*"
          multiple
          onChange={handleFileChange}
          className="hidden"
        />
      </label>
    </div>
  );
};

export default DropzoneStep;