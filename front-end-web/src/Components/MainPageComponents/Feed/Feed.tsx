import React from "react";
import Post from "../Post/Post";
import { Box } from "@mui/material";

const Feed = () => {
  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      {[1, 2, 3].map((id) => (
        <Post key={id} />
      ))}
    </Box>
  );
};

export default Feed;