import React, { useState } from "react";
import { UserProfileInfo } from "../../API/agent";
import Input from "../Input/Input";
import { FormControl } from "../../Types/FormControl";
import validateControl from "../../Utils/GeneralValidation";
import Agent from "../../API/agent";

interface EditProfileFormProps {
  userProfile: UserProfileInfo;
  onSave: (updatedProfile: UserProfileInfo, profileImage?: File | null) => void;
  onCancel: () => void;
}

type EditFormControls = {
  [K in keyof Pick<UserProfileInfo, 'firstName' | 'lastName' | 'userName' |  'biography' | 'birthDate'>]: FormControl;
};

const EditProfileForm: React.FC<EditProfileFormProps> = ({ 
  userProfile, 
  onSave, 
  onCancel 
}) => {

  const [formControls, setFormControls] = useState<EditFormControls>({
    firstName: {
      type: "text",
      name: "FirstName",
      label: "Ім'я:",
      errorMessage: "",
      value: userProfile.firstName || "",
      valid: true,
      validation: {
        required: true,
        minLength: 2,
        maxLength: 50,
        allowSpaces: true,
      },
      touched: false,
      shake: false,
    },
    lastName: {
      type: "text",
      name: "LastName",
      label: "Прізвище:",
      errorMessage: "",
      value: userProfile.lastName || "",
      valid: true,
      validation: {
        required: true,
        minLength: 2,
        maxLength: 50,
        allowSpaces: true,
      },
      touched: false,
      shake: false,
    },
    userName: {
      type: "text",
      name: "UserName",
      label: "Ім'я користувача:",
      errorMessage: "",
      value: userProfile.userName || "",
      valid: true,
      validation: {
        required: true,
        username: true,
        minLength: 3,
        maxLength: 30,
        allowSpaces: false,
      },
      touched: false,
      shake: false,
    },
    biography: {
      type: "textarea",
      name: "Biography",
      label: "Біографія:",
      errorMessage: "",
      value: userProfile.biography || "",
      valid: true,
      validation: {
        required: false,
        maxLength: 500,
        allowSpaces: true,
      },
      touched: false,
      shake: false,
    },
    birthDate: {
      type: "date",
      name: "BirthDate",
      label: "Дата народження:",
      errorMessage: "",
      value: userProfile.birthDate ? new Date(userProfile.birthDate).toISOString().split('T')[0] : "",
      valid: false,
      validation: {
        required: true,
        date: true,
      },
      touched: false,
      shake: false,
    },
  });

  const [profileImage, setProfileImage] = useState<File | null>(null);
  const [profileImagePreview, setProfileImagePreview] = useState<string>(
    userProfile.profileImagePath || ""
  );
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleInputChange = (name: string, value: string) => {
    const camelCaseName = name.charAt(0).toLowerCase() + name.slice(1) as keyof EditFormControls;
    
    const updatedControl = { ...formControls[camelCaseName] };
    updatedControl.value = value;
    updatedControl.touched = true;
    
    const [isValid, errorMessage] = validateControl(updatedControl.value, updatedControl.validation, updatedControl.name);
    updatedControl.valid = isValid;
    updatedControl.errorMessage = errorMessage;
    updatedControl.shake = !isValid;
  
    setFormControls(prev => ({
      ...prev,
      [camelCaseName]: updatedControl
    }));
  };

  const handleImageChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) {
      setProfileImage(file);
      const reader = new FileReader();
      reader.onload = (e) => {
        setProfileImagePreview(e.target?.result as string);
      };
      reader.readAsDataURL(file);
    }
  };

  const isFormValid = () => {
    const isPhotoChanged = profileImage !== null;
    
    const hasChanges = isPhotoChanged || Object.keys(formControls).some(key => {
      const fieldName = key as keyof EditFormControls;
      const control = formControls[fieldName];
      const originalValue = userProfile[fieldName] || "";
      const currentValue = control.value;

      if (fieldName === 'birthDate' && userProfile.birthDate) {
        const originalDateStr = new Date(userProfile.birthDate).toISOString().split('T')[0];
        return originalDateStr !== currentValue;
      }
      
      return String(originalValue) !== String(currentValue);
    });

    const allChangedFieldsValid = Object.keys(formControls).every(key => {
      const fieldName = key as keyof EditFormControls;
      const control = formControls[fieldName];
      const originalValue = userProfile[fieldName] || "";
      const currentValue = control.value;

      let isChanged = false;

      if (fieldName === 'birthDate' && userProfile.birthDate) {
        const originalDateStr = new Date(userProfile.birthDate).toISOString().split('T')[0];
        isChanged = originalDateStr !== currentValue;
      } else {
        isChanged = String(originalValue) !== String(currentValue);
      }
      
      if (!isChanged) {
        return true;
      }

      return control.valid;
    });

    return hasChanges && allChangedFieldsValid;
  };

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();

    if (!isFormValid()) {
      return;
    }

    setIsSubmitting(true);

    try {
      await Agent.User.updateUserProfileInfo({
        firstName: formControls.firstName.value,
        lastName: formControls.lastName.value,
        birthday: new Date(formControls.birthDate.value),
        username: formControls.userName.value,
        biography: formControls.biography.value || null,
        profilePhoto: profileImage,
      });

      onCancel();
    } catch (error) {
      console.error("Помилка при оновленні профілю:", error);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="edit-form-container">
      <div className="form-header mb-6">
        <h2 className="text-2xl font-bold text-gray-800 dark:text-gray-100 mb-2">📝 Форма редагування профілю</h2>
      </div>
      
      <form onSubmit={handleSubmit}>
        <div className="form-section">
          <h3 className="section-title">Фото профілю</h3>
          <div className="profile-image-section">
            <div className="relative">
              <img
                src={profileImagePreview || "https://via.placeholder.com/96x96?text=No+Image"}
                alt="Profile"
                className="profile-image-preview"
              />
            </div>
            <div>
              <input
                type="file"
                id="profileImage"
                accept="image/*"
                onChange={handleImageChange}
                className="hidden"
              />
              <label
                htmlFor="profileImage"
                className="image-upload-btn"
              >
                📷 Змінити фото
              </label>
              {profileImage && (
                <p className="text-sm text-gray-600 mt-2">
                  Вибрано: {profileImage.name}
                </p>
              )}
            </div>
          </div>
        </div>

        <div className="form-section">
          <h3 className="section-title">👤 Особиста інформація</h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <Input
              type="text"
              name="FirstName"
              value={formControls.firstName.value}
              label="Введіть ваше ім'я"
              errorMessage={formControls.firstName.errorMessage}
              valid={formControls.firstName.valid}
              touched={formControls.firstName.touched}
              shake={formControls.firstName.shake}
              onChange={(e) => handleInputChange('FirstName', e.target.value)}
            />
            <Input
              type="text"
              name="LastName"
              value={formControls.lastName.value}
              label="Введіть ваше прізвище"
              errorMessage={formControls.lastName.errorMessage}
              valid={formControls.lastName.valid}
              touched={formControls.lastName.touched}
              shake={formControls.lastName.shake}
              onChange={(e) => handleInputChange('LastName', e.target.value)}
            />
            <Input
              type="text"
              name="UserName"
              value={formControls.userName.value}
              label="Username"
              errorMessage={formControls.userName.errorMessage}
              valid={formControls.userName.valid}
              touched={formControls.userName.touched}
              shake={formControls.userName.shake}
              onChange={(e) => handleInputChange('UserName', e.target.value)}
            />
          </div>
          
          <div className="mt-2">
            <Input
              type={formControls.birthDate.type}
              name={formControls.birthDate.name}
              value={formControls.birthDate.value}
              label={formControls.birthDate.label}
              errorMessage={formControls.birthDate.errorMessage}
              valid={formControls.birthDate.valid}
              touched={formControls.birthDate.touched}
              shake={formControls.birthDate.shake}
              onChange={(e) => handleInputChange(formControls.birthDate.name, e.target.value)}
            />
          </div>
        </div>

        <div className="form-section">
          <h3 className="section-title">📖 Біографія</h3>
          <textarea
            value={formControls.biography.value}
            onChange={(e) => handleInputChange('Biography', e.target.value)}
            placeholder="✨ Розкажіть про себе щось цікаве... Ваші хобі, інтереси, досягнення або просто те, що робить вас унікальним! 🌟"
            className="w-full p-3 border border-gray-300 dark:border-teal-900 rounded-lg resize-none h-32 focus:ring-2 focus:ring-blue-500 focus:border-transparent placeholder:font-medium placeholder:text-gray-500"
            maxLength={500}
          />
          <div className="character-counter">
            {formControls.biography.value.length}/500 символів
          </div>
          {formControls.biography.errorMessage && (
            <div className="text-red-500 text-sm mt-1">
              {formControls.biography.errorMessage}
            </div>
          )}
        </div>

        <div className="form-actions">
          <button
            type="button"
            onClick={onCancel}
            className="btn-secondary"
            disabled={isSubmitting}
          >
            ❌ Скасувати
          </button>
          <button
            type="submit"
            className="btn-primary"
            disabled={!isFormValid() || isSubmitting}
          >
            {isSubmitting ? "💾 Збереження..." : "✅ Зберегти зміни"}
          </button>
        </div>
      </form>
    </div>
  );
};

export default EditProfileForm;