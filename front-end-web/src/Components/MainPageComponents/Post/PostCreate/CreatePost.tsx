import React, { useEffect, useState } from "react";
import { Box, Button, TextField, Typography, IconButton, Paper, Autocomplete } from "@mui/material";
import { useDropzone } from "react-dropzone";
import AddPhotoAlternateIcon from "@mui/icons-material/AddPhotoAlternate";
import DeleteIcon from "@mui/icons-material/Delete";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import agent from "../../../../API/agent";
import { Tag } from "../../../../API/agent";

export default function CreatePostForm() {
  const [caption, setCaption] = useState("");
  const [availableTags, setAvailableTags] = useState<Tag[]>([]);
  const [tags, setTags] = useState<string[]>([]);
  const [postMedias, setPostMedias] = useState<File[]>([]);
  const [thumbnail, setThumbnail] = useState<File | null>(null);

  const onDrop = (acceptedFiles: File[]) => {
    setPostMedias((prev) => [...prev, ...acceptedFiles]);
  };

  const hasVideo = postMedias.some((file) => file.type.startsWith("video/"));

  const { getRootProps, getInputProps, isDragActive } = useDropzone({
    onDrop,
    accept: { "image/*": [], "video/*": [] },
    multiple: true,
  });

  useEffect(() => {
    // Имитация запроса тегов с бэка
    const fetchTags = async () => {
      try {
        const response = await agent.Tags.getAllTags();
        setAvailableTags(response.data);
      } catch (err) {
        console.error("Ошибка при загрузке тегов", err);
      }
    };

    fetchTags();
  }, []);

  const handleSubmit = async () => {
    try {
      await agent.Posts.createPost({ caption, tags, postMedias, thumbnail });
      alert("Post created successfully!");
    } catch (err) {
      console.error(err);
      alert("Upload failed");
    }
  };

  return (
    <Paper
      elevation={4}
      sx={{
        width: "100vw",
        maxWidth: 1200,
        height: "80vh",
        borderRadius: 4,
        display: "flex",
        overflow: "hidden",
        boxShadow: 3,
        bgcolor: "#fff",
      }}
    >
      <Box
        {...getRootProps()}
        sx={{
          width: "70%",
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          justifyContent: "center",
          border: "2px dashed #aaa",
          borderRadius: 2,
          bgcolor: "#fff",
          p: 3,
          cursor: "pointer",
          m: 2,
        }}
      >
        <input {...getInputProps()} />
        <AddPhotoAlternateIcon fontSize="large" color="primary" />
        <Typography variant="h6" sx={{ mt: 1 }}>
          {isDragActive ? "Drop files here..." : "Click or drag to upload"}
        </Typography>

        <Box
          sx={{
            mt: 3,
            display: "flex",
            gap: 2,
            overflowX: "auto",
            width: "100%",
            justifyContent: "center",
            p: 1,
          }}
        >
          {postMedias.map((file, index) => (
            <Box key={index} sx={{ position: "relative", minWidth: 120 }}>
              <Paper
                sx={{
                  width: 120,
                  height: 120,
                  backgroundSize: "cover",
                  backgroundPosition: "center",
                  backgroundImage: `url(${URL.createObjectURL(file)})`,
                }}
              />
              <IconButton
                onClick={(e) => {
                  e.stopPropagation();
                  setPostMedias(postMedias.filter((_, i) => i !== index));
                }}
                sx={{
                  position: "absolute",
                  top: 0,
                  right: 0,
                  bgcolor: "#fff",
                  boxShadow: 1,
                }}
              >
                <DeleteIcon fontSize="small" />
              </IconButton>
            </Box>
          ))}
        </Box>
      </Box>

      <Box
        sx={{
          width: "30%",
          display: "flex",
          flexDirection: "column",
          p: 3,
          gap: 2,
          bgcolor: "#fafafa",
        }}
      >
        <TextField label="Caption" value={caption} onChange={(e) => setCaption(e.target.value)} multiline rows={3} />

        <Autocomplete
          multiple
          options={availableTags.map((tag) => tag.name)}
          value={tags}
          onChange={(_, newValue) => setTags(newValue)}
          renderInput={(params) => <TextField {...params} label="Tags" placeholder="Chouse tags..." />}
        />

        {hasVideo && (
          <>
            <Button variant="outlined" component="label">
              Upload Thumbnail
              <input hidden type="file" accept="image/*" onChange={(e) => setThumbnail(e.target.files?.[0] || null)} />
            </Button>

            {thumbnail && <Typography variant="caption">Thumbnail: {thumbnail.name}</Typography>}
          </>
        )}

        <Box sx={{ flexGrow: 1 }} />

        <Button variant="contained" startIcon={<CloudUploadIcon />} onClick={handleSubmit} fullWidth>
          Share
        </Button>
      </Box>
    </Paper>
  );
}
