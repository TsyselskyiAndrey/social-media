import React, { useState } from "react";
import {
  Box,
  Typography,
  Avatar,
  Button,
  Divider,
  List,
  ListItem,
} from "@mui/material";

interface User {
  id: number;
  name: string;
  handle: string;
  avatar: string;
}

interface UserResultsProps {
  users: User[];
}

const UserResults: React.FC<UserResultsProps> = ({ users }) => {
  const [following, setFollowing] = useState<number[]>([]);

  const toggleFollow = (id: number) => {
    setFollowing((prev) =>
      prev.includes(id) ? prev.filter((uid) => uid !== id) : [...prev, id]
    );
  };

  if (users.length === 0) {
    return (
      <Box sx={{ textAlign: "center", my: 4 }}>
        <Typography variant="body1" color="text.secondary">
          Користувачів не знайдено
        </Typography>
      </Box>
    );
  }

  return (
    <List sx={{ width: "100%" }}>
      {users.map((user) => {
        const isFollowing = following.includes(user.id);
        return (
          <React.Fragment key={user.id}>
            <ListItem
              sx={{
                py: 2,
                display: "flex",
                alignItems: "center",
                justifyContent: "space-between",
              }}
            >
              <Box sx={{ display: "flex", alignItems: "center", gap: 2 }}>
                <Avatar sx={{ width: 50, height: 50 }}>{user.avatar}</Avatar>
                <Box>
                  <Typography fontWeight="bold">{user.name}</Typography>
                  <Typography variant="body2" color="text.secondary">
                    {user.handle}
                  </Typography>
                </Box>
              </Box>
              <Button
                variant={isFollowing ? "contained" : "outlined"}
                size="small"
                sx={{
                  bgcolor: isFollowing ? "#00bfff" : "transparent",
                  color: isFollowing ? "#fff" : "#00bfff",
                  borderColor: "#00bfff",
                  textTransform: "none",
                  fontWeight: "bold",
                  "&:hover": {
                    bgcolor: isFollowing ? "#00a6d6" : "rgba(0,191,255,0.1)",
                  },
                }}
                onClick={() => toggleFollow(user.id)}
              >
                {isFollowing ? "Підписано" : "Підписатися"}
              </Button>
            </ListItem>
            <Divider />
          </React.Fragment>
        );
      })}
    </List>
  );
};

export default UserResults;