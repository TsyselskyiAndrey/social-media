import React, { useEffect, useState } from 'react';
import { Tag } from '../../../API/agent';

interface ConfirmStepProps {
    caption: string;
    setCaption: (text: string) => void;
    tags: string[];
    setTags: (tags: string[]) => void;
    thumbnail: File | null;
    setThumbnail: (file: File | null) => void;
    onBack: () => void;
    onNext: () => void;
    onClose: () => void;
}

const ConfirmStep: React.FC<ConfirmStepProps> = ({
  caption,
  setCaption,
  tags,
  setTags,
  onBack,
  onNext,
  onClose,
}) => {
  const [allTags, setAllTags] = useState<Tag[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    setLoading(true);
    fetch('/api/post/getAllTags', {
      headers: { Authorization: `Bearer ${localStorage.getItem('token') || ''}` },
    })
      .then(res => res.json())
      .then(data => {
        setAllTags(data);
        setLoading(false);
      })
      .catch(() => {
        setAllTags([]);
        setError('Не вдалося завантажити теги');
        setLoading(false);
      });
  }, []);

  const onTagToggle = (tagName: string) => {
    if (tags.includes(tagName)) {
      setTags(tags.filter(t => t !== tagName));
    } else {
      setTags([...tags, tagName]);
    }
  };

  return (
    <div className="p-4 max-w-xl mx-auto">
      <div className="flex justify-between items-center mb-4">
        <h2 className="text-xl font-semibold">Додайте підпис і теги</h2>
        <button onClick={onClose} className="text-gray-500 hover:text-red-500 text-2xl font-bold">×</button>
      </div>

      <textarea
        placeholder="Підпис..."
        value={caption}
        onChange={e => setCaption(e.target.value)}
        rows={4}
        className="w-full p-2 border border-gray-300 rounded mb-6 resize-none focus:outline-none focus:ring-2 focus:ring-blue-500"
      />

      <h3 className="mb-2 font-medium">Виберіть теги:</h3>
      {loading ? (
        <div className="mb-4 text-gray-500">Завантаження тегів...</div>
      ) : error ? (
        <div className="mb-4 text-red-500">{error}</div>
      ) : (
        <div className="flex flex-wrap gap-2 mb-6">
          {allTags.map(tag => (
            <button
              key={tag.id}
              onClick={() => onTagToggle(tag.name)}
              className={`px-3 py-1 rounded cursor-pointer border transition-colors
                ${tags.includes(tag.name)
                  ? 'bg-blue-600 text-white border-blue-600'
                  : 'bg-gray-200 text-gray-800 border-gray-300 hover:bg-gray-300'}`}
            >
              {tag.name}
            </button>
          ))}
        </div>
      )}

      <div className="flex justify-between">
        <button
          onClick={onBack}
          className="px-4 py-2 bg-gray-300 rounded hover:bg-gray-400 transition"
        >
          Назад
        </button>
        <button
          onClick={onNext}
          className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition"
        >
          Далі
        </button>
      </div>
    </div>
  );
};

export default ConfirmStep;