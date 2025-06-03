import React from "react";
import {
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Typography,
  Box,
  useTheme,
} from "@mui/material";

type MenuItem = {
  text: string;
  icon: React.ReactNode;
  onClick?: () => void;
};

type MenuItemsProps = {
  items: MenuItem[];
  maxWidth?: number;
};

const MenuItems: React.FC<MenuItemsProps> = ({ items }) => {
  const theme = useTheme();

  return (
    <Box>
      {items.map(({ text, icon, onClick }) => (
        <ListItemButton
          key={text}
          onClick={onClick}
          sx={{
            color: theme.palette.mode === "dark" ? "#f9fafb" : "black",
            mx: 0,
            mb: 1.5,
            borderRadius: 2,
            fontWeight: "bold",
            transition: "background-color 0.4s ease",
            "&:hover": {
              backgroundColor:
                theme.palette.mode === "dark"
                  ? "rgba(17,140,140, 0.3)" // light blue hover in dark
                  : "rgba(30,144,255, 0.4)", // DodgerBlue in light
            },
          }}
        >
          <ListItemIcon
            sx={{
              color: theme.palette.mode === "dark" ? "#f9fafb" : "black",
              minWidth: 40,
            }}
          >
            {icon}
          </ListItemIcon>
          <ListItemText
            primary={
              <Typography
                fontWeight="bold"
                fontSize="1.1rem"
                sx={{
                  color: theme.palette.mode === "dark" ? "#f9fafb" : "black",
                }}
              >
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
