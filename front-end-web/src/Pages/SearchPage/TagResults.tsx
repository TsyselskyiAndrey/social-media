import React from "react";
import { Box, Chip, Typography, CircularProgress, Fade } from "@mui/material";
import { Tag } from "../../API/agent";

interface TagResultsProps {
  tags: Tag[];
  isLoading: boolean;
  onTagClick?: (tag: Tag) => void;
}

const TagResults: React.FC<TagResultsProps> = ({ tags, isLoading, onTagClick }) => {
  if (isLoading) {
    return (
      <Box sx={{ display: "flex", justifyContent: "center", my: 4 }}>
        <CircularProgress color="warning" thickness={4} />
      </Box>
    );
  }

  if (tags.length === 0) {
    return (
      <Fade in={true} timeout={800}>
        <Box 
          sx={{ 
            textAlign: "center", 
            my: 4, 
            p: 3, 
            borderRadius: 2,
            bgcolor: "rgba(255, 193, 7, 0.05)",
            border: "2px dashed rgba(255, 193, 7, 0.3)"
          }}
        >
          <Typography variant="h6" color="text.secondary" fontWeight="500">
            Теги не знайдено
          </Typography>
        </Box>
      </Fade>
    );
  }

  return (
    <Fade in={true} timeout={500}>
      <Box sx={{ my: 3 }}>
        <Box 
          sx={{ 
            display: "flex", 
            flexWrap: "wrap", 
            gap: 1.5,
            justifyContent: "center"
          }}
        >
          {tags.map((tag) => (
            <Chip
              key={tag.id}
              label={`#${tag.name}`}
              variant="outlined"
              sx={{
                cursor: "pointer",
                borderColor: "#ffc107",
                color: "#555",
                fontSize: "0.95rem",
                fontWeight: 500,
                py: 0.5,
                px: 1,
                borderRadius: "16px",
                transition: "all 0.2s ease-in-out",
                boxShadow: "0 1px 3px rgba(0,0,0,0.05)",
                "&:hover": {
                  bgcolor: "#fff9e6",
                  color: "#000",
                  borderColor: "#ffca28",
                  transform: "translateY(-2px)",
                  boxShadow: "0 4px 8px rgba(0,0,0,0.1)",
                },
                "&:active": {
                  transform: "translateY(0)",
                  boxShadow: "0 1px 2px rgba(0,0,0,0.1)",
                }
              }}
              onClick={() => onTagClick ? onTagClick(tag) : console.log(`Clicked tag: ${tag.name}`)}
            />
          ))}
        </Box>
      </Box>
    </Fade>
  );
};

export default TagResults;