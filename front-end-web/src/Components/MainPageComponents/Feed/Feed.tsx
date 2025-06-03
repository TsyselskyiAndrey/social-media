import React, { useEffect, useState } from "react";
import { Box } from "@mui/material";
import Agent from "../../../API/agent";
import { Post as PostType } from "../../../API/agent";
import Post from "../Post/Post";

const Feed = () => {
  const [posts, setPosts] = useState<PostType[]>([]);

  useEffect(() => {
    const fetchPosts = async () => {
      try {
        const response = await Agent.Posts.getPosts(null, 1000, null, null, null);
        setPosts(response.data);
      } catch (error) {
        console.error("Помилка при завантаженні постів", error);
      }
    };

    fetchPosts();
  }, []);

  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      {posts.map((post) => (
        <Post key={post.id} post={post} />
      ))}
    </Box>
  );
};

export default Feed;