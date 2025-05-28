import React, { useEffect, useState } from 'react';
import agent, { Tag } from '../../../API/agent';
import { Autocomplete, TextField } from '@mui/material';

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
    const fetchTags = async () => {
      setLoading(true);
      try {
        const response = await agent.Tags.getAllTags();
        setAllTags(response.data);
      } catch (err) {
        setAllTags([]);
        setError('Failed to load tags');
      } finally {
        setLoading(false);
      }
    };
    fetchTags();
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
        <Autocomplete
          multiple
          options={allTags.map(tag => tag.name)}
          value={tags}
          onChange={(_, newValue) => setTags(newValue)}
          renderInput={(params) => (
            <TextField {...params} label="Теги" placeholder="Оберіть теги..." />
          )}
          sx={{ mb: 3 }}
          slotProps={{ popper: { sx: { zIndex: 13010 } } }}
        />
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