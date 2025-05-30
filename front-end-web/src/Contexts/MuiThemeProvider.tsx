import React from "react";
import { ThemeProvider, createTheme, CssBaseline } from "@mui/material";
import { useTheme as useAppTheme } from "./ThemeContext";

interface Props {
  children: React.ReactNode;
}

const MuiThemeProvider: React.FC<Props> = ({ children }) => {
  const { theme } = useAppTheme();

  const muiTheme = createTheme({
    palette: {
      mode: theme === "dark" ? "dark" : "light",
    },
  });

  return (
    <ThemeProvider theme={muiTheme}>
      <CssBaseline />
      {children}
    </ThemeProvider>
  );
};

export default MuiThemeProvider;
