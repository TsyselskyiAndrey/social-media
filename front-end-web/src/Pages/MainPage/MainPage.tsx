import React from 'react';
import { Box, CssBaseline } from "@mui/material";
import LeftSidebar from '../../Components/MainPageComponents/LeftSidebar/LeftSidebar';
import Feed from '../../Components/MainPageComponents/Feed/Feed';
import RightSidebar from '../../Components/MainPageComponents/RightSidebar/RightSidebar';
import './MainPage.css';

const MainPage = () => {
  return (
    <>
      <CssBaseline />
      <Box
        sx={{
          display: "flex",
          maxWidth: 1800,
          mx: "auto",
          pt: 8,
          px: 2,
          gap: 5,
        }}
      >
        <Box
          sx={{
            display: { xs: "none", md: "block" },
            flexBasis: { md: "25%", lg: "22%" },
            minWidth: 250,
          }}
        >
          <LeftSidebar />
        </Box>
        <Box
          sx={{
            flex: 1,
            flexBasis: { xs: "100%", md: "55%", lg: "56%" },
            minWidth: 700,
          }}
        >
          <Feed />
        </Box>
        <Box
          sx={{
            display: { xs: "none", lg: "block" },
            flexBasis: { lg: "22%" },
            minWidth: 280,
          }}
        >
          <RightSidebar />
        </Box>
      </Box>
    </>
  );
};

export default MainPage;