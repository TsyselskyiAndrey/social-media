import React, { useEffect, useState } from 'react';
import Agent, { Tag } from '../../../API/agent';
import { Autocomplete, TextField } from '@mui/material';
import { Post as PostType } from '../../../API/agent';

interface EditPostFormProps {
  post: PostType;
  onCancel: () => void;
  onUpdated: () => void;
}

const EditPostForm: React.FC<EditPostFormProps> = ({ post, onCancel, onUpdated }) => {
  const [caption, setCaption] = useState(post.caption ?? '');
  const [tags, setTags] = useState<string[]>(post.tags || []);
  const [thumbnail, setThumbnail] = useState<File | null>(null);
  const [allTags, setAllTags] = useState<Tag[]>([]);
  const [existingMedia, setExistingMedia] = useState(post.postMedias ?? []);
  const [postMedias, setPostMedias] = useState<File[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchTags = async () => {
      try {
        const response = await Agent.Tags.getAllTags();
        setAllTags(response.data);
      } catch (err) {
        console.error('Не вдалося завантажити теги');
      }
    };
    fetchTags();
  }, []);

  const handleSubmit = async () => {
    const updateRequest = {
        id: post.id,
        caption: caption ?? "",
        tags: tags,
        thumbnail: thumbnail,
        postMedias: postMedias
    };

    try {
        setLoading(true);
        await Agent.Posts.updatePost(updateRequest);
        alert("Пост оновлено!");
        onUpdated();
    } catch (error) {
        console.error("Помилка при оновленні поста", error);
        alert("Не вдалося оновити пост.");
    } finally {
        setLoading(false);
    }
    };


  return (
    <div className="p-4 max-w-xl mx-auto bg-white shadow rounded">
      <div className="flex justify-between items-center mb-4">
        <h2 className="text-xl font-semibold">Редагувати пост</h2>
        <button onClick={onCancel} className="text-gray-500 hover:text-red-500 text-2xl font-bold">×</button>
      </div>

      <textarea
        placeholder="Підпис..."
        value={caption}
        onChange={e => setCaption(e.target.value)}
        rows={4}
        className="w-full p-2 border border-gray-300 rounded mb-6 resize-none focus:outline-none focus:ring-2 focus:ring-blue-500"
      />

      <h3 className="mb-2 font-medium">Теги:</h3>
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

      <div className="mb-4">
        <label className="block mb-2 font-medium">Мініатюра (опціонально)</label>
        <input type="file" accept="image/*" onChange={e => setThumbnail(e.target.files?.[0] ?? null)} />
      </div>

      <div className="mb-4">
        <label className="block mb-2 font-medium">Нові медіафайли (опціонально)</label>
        <input
            type="file"
            accept="image/*,video/*"
            multiple
            onChange={e => {
            const files = e.target.files;
            if (files) {
                setPostMedias(Array.from(files));
            }
            }}
        />
        </div>


      <div className="flex justify-between">
        <button
          onClick={onCancel}
          className="px-4 py-2 bg-gray-300 rounded hover:bg-gray-400 transition"
        >
          Скасувати
        </button>
        <button
          onClick={handleSubmit}
          className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition"
        >
          Зберегти
        </button>
      </div>
    </div>
  );
};

export default EditPostForm;
