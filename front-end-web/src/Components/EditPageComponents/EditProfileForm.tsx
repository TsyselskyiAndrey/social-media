import React, { useState } from "react";
import { UserProfileInfo } from "../../API/agent";
import Input from "../Input/Input";
import { FormControl } from "../../Types/FormControl";
import validateControl from "../../Utils/GeneralValidation";

interface EditProfileFormProps {
  userProfile: UserProfileInfo;
  onSave: (updatedProfile: UserProfileInfo) => void;
  onCancel: () => void;
}

type EditFormControls = {
  [K in keyof Pick<UserProfileInfo, 'FirstName' | 'LastName' | 'UserName' | 'Email' | 'Biography' | 'BirthDate'>]: FormControl;
};

const EditProfileForm: React.FC<EditProfileFormProps> = ({ 
  userProfile, 
  onSave, 
  onCancel 
}) => {

  const [formControls, setFormControls] = useState<EditFormControls>({
    FirstName: {
      type: "text",
      name: "FirstName",
      label: "Ім'я:",
      errorMessage: "",
      value: userProfile.FirstName || "",
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
    LastName: {
      type: "text",
      name: "LastName",
      label: "Прізвище:",
      errorMessage: "",
      value: userProfile.LastName || "",
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
    UserName: {
      type: "text",
      name: "UserName",
      label: "Ім'я користувача:",
      errorMessage: "",
      value: userProfile.UserName || "",
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
    Email: {
      type: "email",
      name: "Email",
      label: "Email:",
      errorMessage: "",
      value: userProfile.Email || "",
      valid: true,
      validation: {
        required: true,
        email: true,
        allowSpaces: false,
      },
      touched: false,
      shake: false,
    },
    Biography: {
      type: "textarea",
      name: "Biography",
      label: "Біографія:",
      errorMessage: "",
      value: userProfile.Biography || "",
      valid: true,
      validation: {
        required: false,
        maxLength: 500,
        allowSpaces: true,
      },
      touched: false,
      shake: false,
    },
    BirthDate: {
      type: "date",
      name: "BirthDate",
      label: "Дата народження:",
      errorMessage: "",
      value: userProfile.BirthDate ? new Date(userProfile.BirthDate).toISOString().split('T')[0] : "",
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
    userProfile.ProfileImagePath || ""
  );
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleInputChange = (name: string, value: string) => {
    const updatedControl = { ...formControls[name as keyof EditFormControls] };
    updatedControl.value = value;
    updatedControl.touched = true;
    
    const [isValid, errorMessage] = validateControl(updatedControl.value, updatedControl.validation, updatedControl.name);
    updatedControl.valid = isValid;
    updatedControl.errorMessage = errorMessage;
    updatedControl.shake = !isValid;

    setFormControls(prev => ({
      ...prev,
      [name]: updatedControl
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

    const requiredFields = ['FirstName', 'LastName', 'UserName', 'Email', 'BirthDate'];
    
    return requiredFields.every(fieldName => {
      const control = formControls[fieldName as keyof EditFormControls];
      return control.valid && control.value.trim() !== '';
    });
  };

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    
    if (!isFormValid()) {
      return;
    }

    setIsSubmitting(true);
    
    try {
      const updatedProfile: UserProfileInfo = {
        FirstName: formControls.FirstName.value,
        LastName: formControls.LastName.value,
        UserName: formControls.UserName.value,
        Email: formControls.Email.value,
        Biography: formControls.Biography.value || null,
        BirthDate: formControls.BirthDate.value ? new Date(formControls.BirthDate.value) : null,
        ProfileImagePath: userProfile.ProfileImagePath,
      };
      
      onSave(updatedProfile);
    } catch (error) {
      console.error("Помилка збереження профілю:", error);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="edit-form-container">
      <div className="form-header mb-6">
        <h2 className="text-2xl font-bold text-gray-800 mb-2">📝 Форма редагування профілю</h2>
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
              value={formControls.FirstName.value}
              label="Введіть ваше ім'я"
              errorMessage={formControls.FirstName.errorMessage}
              valid={formControls.FirstName.valid}
              touched={formControls.FirstName.touched}
              shake={formControls.FirstName.shake}
              onChange={(e) => handleInputChange('FirstName', e.target.value)}
            />
            <Input
              type="text"
              name="LastName"
              value={formControls.LastName.value}
              label="Введіть ваше прізвище"
              errorMessage={formControls.LastName.errorMessage}
              valid={formControls.LastName.valid}
              touched={formControls.LastName.touched}
              shake={formControls.LastName.shake}
              onChange={(e) => handleInputChange('LastName', e.target.value)}
            />
            <Input
              type="text"
              name="UserName"
              value={formControls.UserName.value}
              label="Username"
              errorMessage={formControls.UserName.errorMessage}
              valid={formControls.UserName.valid}
              touched={formControls.UserName.touched}
              shake={formControls.UserName.shake}
              onChange={(e) => handleInputChange('UserName', e.target.value)}
            />
            <Input
              type="email"
              name="Email"
              value={formControls.Email.value}
              label="Email"
              errorMessage={formControls.Email.errorMessage}
              valid={formControls.Email.valid}
              touched={formControls.Email.touched}
              shake={formControls.Email.shake}
              onChange={(e) => handleInputChange('Email', e.target.value)}
            />
          </div>
          
          <div className="mt-2">
            <Input
              type={formControls.BirthDate.type}
              name={formControls.BirthDate.name}
              value={formControls.BirthDate.value}
              label={formControls.BirthDate.label}
              errorMessage={formControls.BirthDate.errorMessage}
              valid={formControls.BirthDate.valid}
              touched={formControls.BirthDate.touched}
              shake={formControls.BirthDate.shake}
              onChange={(e) => handleInputChange(formControls.BirthDate.name, e.target.value)}
            />
          </div>
        </div>

        <div className="form-section">
          <h3 className="section-title">📖 Біографія</h3>
          <textarea
            value={formControls.Biography.value}
            onChange={(e) => handleInputChange('Biography', e.target.value)}
            placeholder="✨ Розкажіть про себе щось цікаве... Ваші хобі, інтереси, досягнення або просто те, що робить вас унікальним! 🌟"
            className="w-full p-3 border border-gray-300 rounded-lg resize-none h-32 focus:ring-2 focus:ring-blue-500 focus:border-transparent placeholder:font-medium placeholder:text-gray-500"
            maxLength={500}
          />
          <div className="character-counter">
            {formControls.Biography.value.length}/500 символів
          </div>
          {formControls.Biography.errorMessage && (
            <div className="text-red-500 text-sm mt-1">
              {formControls.Biography.errorMessage}
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