import React, { useState } from 'react';
import DropzoneStep from './DropzoneStep';
import ConfirmStep from './ConfirmStep';
import PreviewStep from './PreviewStep';
import SuccessStep from './SuccessStep';

type PostMediaFile = File & { previewUrl: string };

type CreatePostProps = {
  onClose: () => void;
};

const MAX_FILES = 10;

const CreatePost: React.FC<CreatePostProps> = ({ onClose }) => {
  const [step, setStep] = useState<'dropzone' | 'confirm' | 'preview' | 'success'>('dropzone');
  const [postMedias, setPostMedias] = useState<PostMediaFile[]>([]);
  const [caption, setCaption] = useState('');
  const [tags, setTags] = useState<string[]>([]);
  const [thumbnail, setThumbnail] = useState<File | null>(null);

  const addFiles = (files: FileList | File[]) => {
    const availableSlots = MAX_FILES - postMedias.length;
    const newFiles = Array.from(files)
      .slice(0, availableSlots)
      .map((file) =>
        Object.assign(file, {
          previewUrl: URL.createObjectURL(file),
        })
      );
    setPostMedias((prev) => [...prev, ...newFiles]);
  };

  const removeFile = (index: number) => {
    setPostMedias((prev) => {
      URL.revokeObjectURL(prev[index].previewUrl); // Очищення URL
      return prev.filter((_, i) => i !== index);
    });
  };

  const removeMedia = (index: number) => {
    setPostMedias((prev) => prev.filter((_, i) => i !== index));
  };

  const reset = () => {
    postMedias.forEach((file) => URL.revokeObjectURL(file.previewUrl));
    setPostMedias([]);
    setCaption('');
    setTags([]);
    setThumbnail(null);
    setStep('dropzone');
  };

  return (
    <div>
      {step === 'dropzone' && (
        <DropzoneStep
          postMedias={postMedias}
          addFiles={addFiles}
          removeFile={removeFile}
          onNext={() => (postMedias.length > 0 ? setStep('confirm') : alert('Додайте файли'))}
          maxFiles={MAX_FILES}
          onClose={onClose}
        />
      )}

      {step === 'confirm' && (
        <ConfirmStep
          caption={caption}
          setCaption={setCaption}
          tags={tags}
          setTags={setTags}
          thumbnail={thumbnail}
          setThumbnail={setThumbnail}
          onBack={() => setStep('dropzone')}
          onNext={() => setStep('preview')}
          onClose={onClose}
        />
      )}

      {step === 'preview' && (
        <div className="w-full flex justify-center">
          <div className="min-w-[900px] max-w-[1200px] w-full">
            <PreviewStep
              postMedias={postMedias}
              caption={caption}
              tags={tags}
              thumbnail={thumbnail}
              addFiles={addFiles}
              onRemoveMedia={removeMedia}
              onBack={() => setStep('confirm')}
              onSubmit={async () => {
                const formData = new FormData();
                formData.append('Caption', caption);
                tags.forEach((tag) => formData.append('Tags', tag));
                postMedias.forEach((file) => formData.append('PostMedias', file));
                if (thumbnail) formData.append('Thumbnail', thumbnail);

                try {
                  const res = await fetch('/api/post/createPost', {
                    method: 'POST',
                    headers: {
                      Authorization: `Bearer ${localStorage.getItem('token') || ''}`,
                    },
                    body: formData,
                  });
                  if (!res.ok) {
                    const errorText = await res.text();
                    alert('Помилка при створенні поста: ' + errorText);
                  } else {
                    setStep('success');
                  }
                } catch (error) {
                  alert('Помилка мережі');
                }
              }}
              onClose={onClose}
            />
          </div>
        </div>
      )}

      {step === 'success' && (
        <SuccessStep
          onReset={() => {
            reset();
            onClose();
          }}
        />
      )}
    </div>
  );
};

export default CreatePost;
