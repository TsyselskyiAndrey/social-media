import React, { useState } from "react";
import { Box, IconButton } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import DropzoneStep from "./DropzoneStep";
import PreviewStep from "./PreviewStep";
import SuccessStep from "./SuccessStep";

type CreatePostProps = {
  onClose: () => void;
};

const CreatePost: React.FC<CreatePostProps> = ({ onClose }) => {
  const [step, setStep] = useState(1);
  const [media, setMedia] = useState<File[]>([]);

  const goToNextStep = () => setStep((prev) => prev + 1);
  const goToPreviousStep = () => setStep((prev) => prev - 1);

  return (
    <Box
      sx={{
        position: "relative",
        maxWidth: 500,
        mx: "auto",
        p: 4,
        bgcolor: "background.paper",
        borderRadius: 2,
        boxShadow: 3,
        minHeight: "300px",
      }}
    >
      <IconButton
        onClick={onClose}
        sx={{
          position: "absolute",
          top: 8,
          right: 8,
          color: "grey.600",
          "&:hover": {
            color: "black",
          },
        }}
      >
        <CloseIcon />
      </IconButton>

      {step === 1 && (
         <DropzoneStep
            onNext={goToNextStep}
            setMedia={(files: File[]) => {
              setMedia(files);
              goToNextStep(); 
           }}
        existingMedia={media}
         />
        )}

      {step === 2 && media.length > 0 && (
        <PreviewStep
          media={media}
          setMedia={setMedia}
          onBack={goToPreviousStep}
          onPublish={() => setStep(3)}
        />
      )}

      {step === 3 && <SuccessStep />}
    </Box>
  );
};

export default CreatePost;