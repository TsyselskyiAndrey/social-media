import React, { useState, useEffect } from "react";
import { Box, CssBaseline } from "@mui/material";
import LeftSidebar from '../../Components/MainPageComponents/LeftSidebar/LeftSidebar';
import Feed from '../../Components/MainPageComponents/Feed/Feed';
import RightSidebar from '../../Components/MainPageComponents/RightSidebar/RightSidebar';
import CreatePost from '../../Components/MainPageComponents/CreatePost/CreatePost';
import './MainPage.css';
import { useTheme } from "../../Contexts/ThemeContext";

const MainPage = () => {
  const [showCreatePost, setShowCreatePost] = useState(false);
  const { theme } = useTheme(); // <- just used to re-render on theme change

  useEffect(() => {
    if (showCreatePost) {
      document.body.style.overflow = 'hidden';
    } else {
      document.body.style.overflow = 'auto';
    }
    return () => {
      document.body.style.overflow = 'auto';
    };
  }, [showCreatePost]);

  useEffect(() => {
    const handleEsc = (event: KeyboardEvent) => {
      if (event.key === 'Escape') {
        setShowCreatePost(false);
      }
    };
    window.addEventListener('keydown', handleEsc);
    return () => window.removeEventListener('keydown', handleEsc);
  }, []);

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
          position: "relative",
        }}
      >
        <Box
          sx={{
            display: { xs: "none", md: "block" },
            flexBasis: { md: "25%", lg: "22%" },
            minWidth: 250,
          }}
        >
          <LeftSidebar onCreateClick={() => setShowCreatePost(true)} />
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

        {showCreatePost && (
          <Box
            sx={{
              position: "fixed",
              inset: 0,
              bgcolor: "rgba(0, 0, 0, 0.4)",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              zIndex: 9999,
              px: 2,
              py: 4,
            }}
            onClick={() => setShowCreatePost(false)}
          >
            <Box
              onClick={(e) => e.stopPropagation()}
              sx={{
                width: { xs: "100%", sm: "80%", md: 900 },
                maxHeight: "90vh",
                bgcolor: "background.paper",
                borderRadius: 2,
                p: 3,
                overflowY: "auto",
                boxShadow: 24,
              }}
            >
              <CreatePost onClose={() => setShowCreatePost(false)} />
            </Box>
          </Box>
        )}
      </Box>
    </>
  );
};

export default MainPage;
