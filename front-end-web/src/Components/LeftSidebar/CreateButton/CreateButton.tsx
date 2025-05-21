import React from "react";
import { ListItemButton, ListItemIcon, ListItemText, Typography, Box } from "@mui/material";
import AddCircleIcon from "@mui/icons-material/AddCircle";

const CreateButton: React.FC = () => {
  return (
    <Box>
      <ListItemButton
        sx={{
          backgroundColor: "#1e90ff",
          color: "white",
          mt: 3, // трохи більший відступ зверху
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
            <Typography fontWeight="bold" fontSize="1rem">
              Створити
            </Typography>
          }
        />
      </ListItemButton>
    </Box>
  );
};
export default CreateButton;