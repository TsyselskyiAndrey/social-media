import React, { useEffect, useState } from 'react';
import Agent, { Tag } from '../../../API/agent';
import { Autocomplete, TextField, Button, IconButton, Box, Typography, Chip } from '@mui/material';
import { Post as PostType } from '../../../API/agent';
import CloseIcon from '@mui/icons-material/Close';
import SaveIcon from '@mui/icons-material/Save';
import CancelIcon from '@mui/icons-material/Cancel';
import ImageIcon from '@mui/icons-material/Image';
import MovieIcon from '@mui/icons-material/Movie';

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
        onUpdated();
    } catch (error) {
        console.error("Помилка при оновленні поста", error);
        alert("Не вдалося оновити пост.");
    } finally {
        setLoading(false);
    }
  };

  return (
    <div className="p-6 max-w-xl mx-auto bg-white dark:bg-gray-800 shadow-lg rounded-lg transition-all duration-300">
      <div className="flex justify-between items-center mb-6">
        <Typography variant="h5" component="h2" className="font-semibold text-gray-800 dark:text-white">
          Редагувати пост
        </Typography>
        <IconButton 
          onClick={onCancel} 
          color="error" 
          size="small"
          sx={{
            transition: 'all 0.2s',
            '&:hover': {
              transform: 'scale(1.1) rotate(90deg)',
            }
          }}
        >
          <CloseIcon />
        </IconButton>
      </div>

      <TextField
        label="Підпис"
        placeholder="Опишіть ваш пост..."
        value={caption}
        onChange={e => setCaption(e.target.value)}
        multiline
        rows={4}
        fullWidth
        variant="outlined"
        margin="normal"
        sx={{
          mb: 3,
          '& .MuiOutlinedInput-root': {
            '&:hover fieldset': {
              borderColor: 'primary.main',
            },
          },
        }}
      />

      <Typography variant="subtitle1" className="mb-2 font-medium text-gray-700 dark:text-gray-300">
        Теги:
      </Typography>
      <Autocomplete
        multiple
        options={allTags.map(tag => tag.name)}
        value={tags}
        onChange={(_, newValue) => setTags(newValue)}
        renderInput={(params) => (
          <TextField {...params} label="Теги" placeholder="Оберіть теги..." variant="outlined" />
        )}
        renderTags={(value, getTagProps) =>
          value.map((option, index) => (
            <Chip
              label={option}
              {...getTagProps({ index })}
              color="primary"
              variant="outlined"
              size="small"
            />
          ))
        }
        sx={{ mb: 4 }}
        slotProps={{ popper: { sx: { zIndex: 13010 } } }}
      />

      <Box className="mb-4 p-4 border border-dashed border-gray-300 dark:border-gray-600 rounded-lg">
        <Typography variant="subtitle1" className="mb-3 font-medium flex items-center gap-2 text-gray-700 dark:text-gray-300">
          <ImageIcon fontSize="small" /> Мініатюра (опціонально)
        </Typography>
        <input 
          type="file" 
          accept="image/*" 
          onChange={e => setThumbnail(e.target.files?.[0] ?? null)} 
          className="w-full p-2 border border-gray-200 dark:border-gray-700 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
      </Box>

      <Box className="mb-6 p-4 border border-dashed border-gray-300 dark:border-gray-600 rounded-lg">
        <Typography variant="subtitle1" className="mb-3 font-medium flex items-center gap-2 text-gray-700 dark:text-gray-300">
          <MovieIcon fontSize="small" /> Нові медіафайли (опціонально)
        </Typography>
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
          className="w-full p-2 border border-gray-200 dark:border-gray-700 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
      </Box>

      <Box className="flex justify-between gap-4">
        <Button
          onClick={onCancel}
          variant="outlined"
          color="error"
          startIcon={<CancelIcon />}
          className="px-6 py-2 transition-all duration-300 hover:bg-red-50"
        >
          Скасувати
        </Button>
        <Button
          onClick={handleSubmit}
          variant="contained"
          color="primary"
          startIcon={<SaveIcon />}
          disabled={loading}
          className="px-6 py-2 transition-all duration-300"
        >
          {loading ? "Збереження..." : "Зберегти"}
        </Button>
      </Box>
    </div>
  );
};

export default EditPostForm;
