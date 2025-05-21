import React, { useState } from 'react';
import IconButton from '@mui/material/IconButton';
import BookmarkBorderIcon from '@mui/icons-material/BookmarkBorder';
import BookmarkIcon from '@mui/icons-material/Bookmark';

interface BookmarkProps {
  initiallySaved?: boolean;
}

const Bookmark: React.FC<BookmarkProps> = ({ initiallySaved = false }) => {
  const [saved, setSaved] = useState(initiallySaved);

  const handleToggle = () => {
    setSaved(prev => !prev);
  };

  return (
    <IconButton aria-label="save" onClick={handleToggle} size="small">
      {saved ? <BookmarkIcon /> : <BookmarkBorderIcon />}
    </IconButton>
  );
};

export default Bookmark;