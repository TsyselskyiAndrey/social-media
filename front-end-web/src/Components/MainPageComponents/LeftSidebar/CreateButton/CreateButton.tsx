import React from "react";
import { ListItemButton, ListItemIcon, ListItemText, Typography, Box } from "@mui/material";
import AddCircleIcon from "@mui/icons-material/AddCircle";

type CreateButtonProps = {
  onClick?: () => void;
};

const CreateButton: React.FC<CreateButtonProps> = ({ onClick }) => {
  return (
    <Box>
      <ListItemButton
        onClick={onClick}
        sx={{
          backgroundColor: "#1e90ff",
          color: "white",
          mt: 3,
          borderRadius: 2,
          mx: 0,
          "&:hover": {
            backgroundColor: "#1c86ee",
          },
        }}
      >
        <ListItemIcon sx={{ color: "white", minWidth: 40 }}>
          <AddCircleIcon sx={{ fontSize: 30 }} />
        </ListItemIcon>
        <ListItemText
          primary={
            <Typography fontWeight="bold" fontSize="1.05rem">
              Створити допис
            </Typography>
          }
        />
      </ListItemButton>
    </Box>
  );
};

export default CreateButton;