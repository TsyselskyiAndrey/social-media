import React from 'react';
import { Box, Paper, Typography, Chip, Stack } from '@mui/material';
import RecommendedUsers from './RecommendedUsers/RecommendedUsers';

const trends = ['#React', '#MUI', '#TypeScript', '#TwitterClone', '#Frontend'];

const RightSidebar: React.FC = () => {
  return (
    <Box
      sx={{
        p: 2,
        width: 380,
        position: 'fixed',
        top: 80,
        display: 'flex',
        flexDirection: 'column',
        gap: 3,
      }}
    >
      <Paper sx={{ p: 2 }}>
        <Typography
          variant="h6"
          fontWeight="bold"
          align="center"
          gutterBottom
        >
          Тренди
        </Typography>
        <Stack direction="row" flexWrap="wrap" gap={1}>
          {trends.map((trend) => (
            <Chip
              key={trend}
              label={trend}
              variant="outlined"
              sx={{
                cursor: 'pointer',
                borderColor: '#ffc107',
                color: '#555',
                '&:hover': {
                  bgcolor: '#fff9e6',
                  color: '#000',
                  borderColor: '#ffca28',
                },
              }}
            />
          ))}
        </Stack>
      </Paper>

      <RecommendedUsers />
    </Box>
  );
};

export default RightSidebar;
