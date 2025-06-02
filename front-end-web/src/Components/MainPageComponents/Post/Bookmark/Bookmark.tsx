import React, { useState, useEffect } from 'react';
import IconButton from '@mui/material/IconButton';
import BookmarkBorderIcon from '@mui/icons-material/BookmarkBorder';
import BookmarkIcon from '@mui/icons-material/Bookmark';

interface BookmarkProps {
  initiallySaved?: boolean;
  onSave?: () => Promise<boolean>;
}

const Bookmark: React.FC<BookmarkProps> = ({ initiallySaved = false, onSave }) => {
  const [saved, setSaved] = useState(initiallySaved);
  
  // Оновлюємо стан, коли змінюється initiallySaved
  useEffect(() => {
    setSaved(initiallySaved);
  }, [initiallySaved]);

  const handleToggle = async () => {
    if (onSave) {
      const result = await onSave();
      setSaved(result);
    } else {
      setSaved(prev => !prev);
    }
  };

  return (
    <IconButton 
      aria-label="save" 
      onClick={handleToggle} 
      size="small"
      sx={{
        color: saved ? 'primary.main' : 'inherit',
        backgroundColor: saved ? 'rgba(25, 118, 210, 0.08)' : 'transparent',
        '&:hover': {
          backgroundColor: saved ? 'rgba(25, 118, 210, 0.15)' : 'rgba(0, 0, 0, 0.04)',
        },
        transition: 'all 0.2s'
      }}
    >
      {saved ? <BookmarkIcon /> : <BookmarkBorderIcon />}
    </IconButton>
  );
};

export default Bookmark;