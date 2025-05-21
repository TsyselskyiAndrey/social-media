import React, { useState } from 'react';
import {
  Box,
  Paper,
  Typography,
  Avatar,
  Button,
  Stack,
  Divider,
} from '@mui/material';

const recommendedUsers = [
  { id: 1, name: 'Ірина Сидоренко', handle: '@iryna' },
  { id: 2, name: 'Андрій Коваленко', handle: '@andrii' },
  { id: 3, name: 'Марія Литвин', handle: '@maria' },
];

const RecommendedUsers: React.FC = () => {
  const [following, setFollowing] = useState<number[]>([]);

  const toggleFollow = (id: number) => {
    setFollowing((prev) =>
      prev.includes(id) ? prev.filter((uid) => uid !== id) : [...prev, id]
    );
  };

  return (
    <Paper sx={{ p: 2 }}>
      <Typography variant="h6" fontWeight="bold" gutterBottom>
        Рекомендовані для вас
      </Typography>
      <Stack spacing={2} divider={<Divider flexItem />}>
        {recommendedUsers.map((user) => {
          const isFollowing = following.includes(user.id);
          return (
            <Box
              key={user.id}
              sx={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'space-between',
              }}
            >
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
                <Avatar>{user.name[0]}</Avatar>
                <Box>
                  <Typography fontWeight="bold">{user.name}</Typography>
                  <Typography variant="caption" color="text.secondary">
                    {user.handle}
                  </Typography>
                </Box>
              </Box>
              <Button
                variant={isFollowing ? 'contained' : 'outlined'}
                size="small"
                sx={{
                  bgcolor: isFollowing ? '#00bfff' : 'transparent',
                  color: isFollowing ? '#fff' : '#00bfff',
                  borderColor: '#00bfff',
                  textTransform: 'none',
                  fontWeight: 'bold',
                  '&:hover': {
                    bgcolor: isFollowing ? '#00a6d6' : 'rgba(0,191,255,0.1)',
                  },
                }}
                onClick={() => toggleFollow(user.id)}
              >
                {isFollowing ? 'Підписано' : 'Підписатися'}
              </Button>
            </Box>
          );
        })}
      </Stack>
    </Paper>
  );
};

export default RecommendedUsers;