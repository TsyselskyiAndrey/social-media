import React, { useState } from 'react';
import IconButton from '@mui/material/IconButton';
import BookmarkBorderIcon from '@mui/icons-material/BookmarkBorder';
import BookmarkIcon from '@mui/icons-material/Bookmark';

interface BookmarkProps {
  initiallySaved?: boolean;
  onSave?: () => Promise<boolean>;
}

const Bookmark: React.FC<BookmarkProps> = ({ initiallySaved = false, onSave }) => {
  const [saved, setSaved] = useState(initiallySaved);

  const handleToggle = async () => {
    if (onSave) {
      const result = await onSave();
      setSaved(result);
    } else {
      setSaved(prev => !prev);
    }
  };

  return (
    <IconButton aria-label="save" onClick={handleToggle} size="small">
      {saved ? <BookmarkIcon /> : <BookmarkBorderIcon />}
    </IconButton>
  );
};

export default Bookmark;