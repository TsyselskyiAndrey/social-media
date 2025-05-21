import React from "react";
import { ListItemButton, ListItemIcon, ListItemText, Typography, Box } from "@mui/material";

type MenuItem = {
  text: string;
  icon: React.ReactNode;
};

type MenuItemsProps = {
  items: MenuItem[];
  maxWidth?: number;
};

const MenuItems: React.FC<MenuItemsProps> = ({ items }) => {
  return (
    <Box>
      {items.map(({ text, icon }) => (
        <ListItemButton
          key={text}
          sx={{
            color: "black",
            mx: 0,
            mb: 1.5,
            borderRadius: 2,
            fontWeight: "bold",
            transition: "background-color 0.4s ease",
            "&:hover": {
              backgroundColor: "rgba(30,144,255, 0.4)",
            },
          }}
        >
          <ListItemIcon sx={{ color: "black", minWidth: 40 }}>
            {icon}
          </ListItemIcon>
          <ListItemText
            primary={
              <Typography fontWeight="bold" fontSize="1rem">
                {text}
              </Typography>
            }
          />
        </ListItemButton>
      ))}
    </Box>
  );
};

export default MenuItems;