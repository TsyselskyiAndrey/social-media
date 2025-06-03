import React, { useEffect, useState } from 'react';
import Agent, { Tag } from '../../../API/agent';
import { Autocomplete, TextField, Button, IconButton, Box, Typography, Chip, CircularProgress, Paper } from '@mui/material';
import { Post as PostType } from '../../../API/agent';
import CloseIcon from '@mui/icons-material/Close';
import SaveIcon from '@mui/icons-material/Save';
import CancelIcon from '@mui/icons-material/Cancel';
import ImageIcon from '@mui/icons-material/Image';
import MovieIcon from '@mui/icons-material/Movie';
import AddPhotoAlternateIcon from '@mui/icons-material/AddPhotoAlternate';
import DeleteIcon from '@mui/icons-material/Delete';
import UploadFileIcon from '@mui/icons-material/UploadFile';

interface EditPostFormProps {
  post: PostType;
  onCancel: () => void;
  onUpdated: () => void;
}

const EditPostForm: React.FC<EditPostFormProps> = ({ post, onCancel, onUpdated }) => {
  const [caption, setCaption] = useState(post.caption ?? '');
  const [tags, setTags] = useState<string[]>(post.tags || []);
  const [thumbnail, setThumbnail] = useState<File | null>(null);
  const [thumbnailPreview, setThumbnailPreview] = useState<string | null>(null);
  const [allTags, setAllTags] = useState<Tag[]>([]);
  const [existingMedia, setExistingMedia] = useState(post.postMedias ?? []);
  const [postMedias, setPostMedias] = useState<File[]>([]);
  const [mediaPreviewUrls, setMediaPreviewUrls] = useState<string[]>([]);
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

  useEffect(() => {
    if (thumbnail) {
      const reader = new FileReader();
      reader.onloadend = () => {
        setThumbnailPreview(reader.result as string);
      };
      reader.readAsDataURL(thumbnail);
    } else {
      setThumbnailPreview(null);
    }
  }, [thumbnail]);

  useEffect(() => {
    const previews: string[] = [];
    postMedias.forEach(file => {
      const reader = new FileReader();
      reader.onloadend = () => {
        previews.push(reader.result as string);
        if (previews.length === postMedias.length) {
          setMediaPreviewUrls(previews);
        }
      };
      reader.readAsDataURL(file);
    });
  }, [postMedias]);

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

  const handleRemoveMedia = (index: number) => {
    const newMedias = [...postMedias];
    newMedias.splice(index, 1);
    setPostMedias(newMedias);
    
    const newPreviews = [...mediaPreviewUrls];
    newPreviews.splice(index, 1);
    setMediaPreviewUrls(newPreviews);
  };

  return (
    <Paper elevation={3} className="p-6 max-w-xl mx-auto bg-white dark:bg-gray-800 shadow-lg rounded-lg transition-all duration-300">
      <div className="flex justify-between items-center mb-6">
        <Typography variant="h5" component="h2" className="font-semibold text-gray-800 dark:text-white flex items-center gap-2">
          <SaveIcon fontSize="small" /> Редагувати пост
        </Typography>
        <IconButton 
          onClick={onCancel} 
          color="error" 
          size="small"
          className="hover:bg-red-50 dark:hover:bg-red-900/30"
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
        className="mb-4"
        sx={{
          mb: 3,
          '& .MuiOutlinedInput-root': {
            borderRadius: '12px',
            '&:hover fieldset': {
              borderColor: 'primary.main',
              borderWidth: '2px',
            },
            '&.Mui-focused fieldset': {
              borderColor: 'primary.main',
              borderWidth: '2px',
            },
          },
          '& .MuiInputLabel-root': {
            '&.Mui-focused': {
              color: 'primary.main',
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
          <TextField 
            {...params} 
            label="Теги" 
            placeholder="Оберіть теги..." 
            variant="outlined"
            sx={{
              '& .MuiOutlinedInput-root': {
                borderRadius: '12px',
                '&:hover fieldset': {
                  borderColor: 'primary.main',
                },
                '&.Mui-focused fieldset': {
                  borderColor: 'primary.main',
                },
              },
            }}
          />
        )}
        renderTags={(value, getTagProps) =>
          value.map((option, index) => (
            <Chip
              label={option}
              {...getTagProps({ index })}
              color="primary"
              variant="outlined"
              size="small"
              className="m-0.5 transition-all duration-200 hover:scale-105"
            />
          ))
        }
        sx={{ mb: 4 }}
        slotProps={{ popper: { sx: { zIndex: 13010 } } }}
      />

      <Box className="mb-4 p-4 border border-dashed border-gray-300 dark:border-gray-600 rounded-lg hover:border-primary-500 transition-colors duration-300">
        <Typography variant="subtitle1" className="mb-3 font-medium flex items-center gap-2 text-gray-700 dark:text-gray-300">
          <ImageIcon fontSize="small" color="primary" /> Мініатюра (опціонально)
        </Typography>
        
        <div className="flex flex-col items-center">
          <label htmlFor="thumbnail-upload" className="cursor-pointer w-full">
            <div className="flex flex-col items-center justify-center p-4 border-2 border-dashed border-gray-300 dark:border-gray-600 rounded-lg hover:border-primary-500 hover:bg-gray-50 dark:hover:bg-gray-700/50 transition-all duration-300">
              {thumbnailPreview ? (
                <div className="relative">
                  <img 
                    src={thumbnailPreview} 
                    alt="Превью мініатюри" 
                    className="max-h-40 max-w-full rounded-lg shadow-md" 
                  />
                  <IconButton 
                    size="small" 
                    color="error" 
                    className="absolute -top-2 -right-2 bg-white shadow-md hover:bg-red-50"
                    onClick={(e) => {
                      e.preventDefault();
                      e.stopPropagation();
                      setThumbnail(null);
                    }}
                  >
                    <DeleteIcon fontSize="small" />
                  </IconButton>
                </div>
              ) : (
                <>
                  <AddPhotoAlternateIcon fontSize="large" color="primary" />
                  <Typography className="mt-2 text-sm text-gray-500 dark:text-gray-400">
                    Натисніть, щоб вибрати мініатюру
                  </Typography>
                </>
              )}
            </div>
          </label>
          <input
            id="thumbnail-upload"
            type="file"
            accept="image/*"
            onChange={e => setThumbnail(e.target.files?.[0] ?? null)}
            className="hidden"
          />
        </div>
      </Box>

      <Box className="mb-6 p-4 border border-dashed border-gray-300 dark:border-gray-600 rounded-lg hover:border-primary-500 transition-colors duration-300">
        <Typography variant="subtitle1" className="mb-3 font-medium flex items-center gap-2 text-gray-700 dark:text-gray-300">
          <MovieIcon fontSize="small" color="primary" /> Нові медіафайли
        </Typography>
        
        <div className="flex flex-col items-center">
          <label htmlFor="media-upload" className="cursor-pointer w-full">
            <div className="flex flex-col items-center justify-center p-4 border-2 border-dashed border-gray-300 dark:border-gray-600 rounded-lg hover:border-primary-500 hover:bg-gray-50 dark:hover:bg-gray-700/50 transition-all duration-300">
              <UploadFileIcon fontSize="large" color="primary" />
              <Typography className="mt-2 text-sm text-gray-500 dark:text-gray-400">
                Натисніть, щоб вибрати медіафайли (фото або відео)
              </Typography>
            </div>
          </label>
          <input
            id="media-upload"
            type="file"
            accept="image/*,video/*"
            multiple
            onChange={e => {
              const files = e.target.files;
              if (files) {
                setPostMedias(prev => [...prev, ...Array.from(files)]);
              }
            }}
            className="hidden"
          />
        </div>
        
        {mediaPreviewUrls.length > 0 && (
          <div className="mt-4">
            <Typography variant="subtitle2" className="mb-2 font-medium text-gray-700 dark:text-gray-300">
              Вибрані файли ({mediaPreviewUrls.length}):
            </Typography>
            <div className="grid grid-cols-3 gap-2">
              {mediaPreviewUrls.map((url, index) => (
                <div key={index} className="relative group">
                  <img 
                    src={url} 
                    alt={`Медіа ${index + 1}`} 
                    className="w-full h-24 object-cover rounded-lg shadow-sm" 
                  />
                  <div className="absolute inset-0 bg-black bg-opacity-0 group-hover:bg-opacity-30 transition-all duration-300 rounded-lg flex items-center justify-center">
                    <IconButton 
                      size="small" 
                      color="error" 
                      className="opacity-0 group-hover:opacity-100 transition-opacity duration-300 bg-white hover:bg-red-50"
                      onClick={() => handleRemoveMedia(index)}
                    >
                      <DeleteIcon fontSize="small" />
                    </IconButton>
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}
        
        {existingMedia.length > 0 && (
          <div className="mt-4">
            <Typography variant="subtitle2" className="mb-2 font-medium text-gray-700 dark:text-gray-300">
              Існуючі медіафайли ({existingMedia.length}):
            </Typography>
            <div className="grid grid-cols-3 gap-2">
              {existingMedia.map((media, index) => (
                <div key={index} className="relative">
                  <img 
                    src={media.mediaUrl} 
                    alt={`Існуючий медіа ${index + 1}`} 
                    className="w-full h-24 object-cover rounded-lg shadow-sm opacity-70" 
                  />
                </div>
              ))}
            </div>
          </div>
        )}
      </Box>

      <Box className="flex justify-between gap-4">
        <Button
          onClick={onCancel}
          variant="outlined"
          color="error"
          startIcon={<CancelIcon />}
          className="px-6 py-2 transition-all duration-300 hover:bg-red-50 dark:hover:bg-red-900/30"
          sx={{
            borderRadius: '10px',
            borderWidth: '2px',
            '&:hover': {
              borderWidth: '2px',
              transform: 'translateY(-2px)',
              boxShadow: '0 4px 8px rgba(0,0,0,0.1)',
            },
          }}
        >
          Скасувати
        </Button>
        <Button
          onClick={handleSubmit}
          variant="contained"
          color="primary"
          startIcon={loading ? <CircularProgress size={20} color="inherit" /> : <SaveIcon />}
          disabled={loading}
          className="px-6 py-2 transition-all duration-300"
          sx={{
            borderRadius: '10px',
            '&:hover': {
              transform: 'translateY(-2px)',
              boxShadow: '0 4px 12px rgba(0,0,0,0.15)',
            },
          }}
        >
          {loading ? "Збереження..." : "Зберегти зміни"}
        </Button>
      </Box>
    </Paper>
  );
};

export default EditPostForm;
