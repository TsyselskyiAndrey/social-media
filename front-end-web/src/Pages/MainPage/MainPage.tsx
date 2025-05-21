import React from 'react';
import { Box, CssBaseline } from "@mui/material";
import LeftSidebar from '../../Components/LeftSidebar/LeftSidebar';
import Feed from '../../Components/Feed/Feed';
import RightSidebar from '../../Components/RightSidebar/RightSidebar';
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
          // УБРАНО: height та overflowY
          // height: `calc(100vh - 64px)`,
          // overflowY: "auto",
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