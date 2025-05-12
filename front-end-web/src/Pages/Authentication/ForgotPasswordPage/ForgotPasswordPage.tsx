import { ChangeEvent, FormEvent, useState } from "react";
import { FormControl } from "../../../Types/FormControl";
import "./ForgotPasswordPage.css";
import "../CommonStyles.css";
import logo from "../../../Assets/logo.png";
import loadanimation from "../../../Assets/loadanimation.gif";
import Input from "../../../Components/Input/Input";
import validateControl from "../../../Utils/GeneralValidation";
import Agent from "../../../API/agent";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";

interface ForgotPasswordControls {
  email: FormControl;
}

export default function ForgotPasswordPage() {
  const [formControls, setFormControls] = useState<ForgotPasswordControls>({
    email: {
      type: "text",
      name: "email",
      label: "Email:",
      errorMessage: "",
      value: "",
      valid: false,
      validation: {
        required: true,
        email: true,
        allowSpaces: false,
      },
      touched: false,
      shake: false,
    },
  });

  const [isSubmitLoading, setIsSubmitLoading] = useState(false);
  const navigate = useNavigate();

  function changeHandler(e: ChangeEvent<HTMLInputElement>, controlName: keyof ForgotPasswordControls) {
    const formControlsCopy = { ...formControls };
    const control = {
      ...formControlsCopy[controlName],
    };
    control.value = e.target.value;
    const [isValid, newErrorMessage] = validateControl(e.target.value, control.validation, control.name);
    control.valid = isValid;
    control.errorMessage = newErrorMessage;
    control.touched = true;

    formControlsCopy[controlName] = control;

    setFormControls(formControlsCopy);
  }

  function IsFormValid() {
    return Object.values(formControls).every((control) => control.valid);
  }

  function shakeInvalidElems() {
    if (!IsFormValid()) {
      const formControlsCopy = {
        ...formControls,
      };
      Object.keys(formControlsCopy).map((controlName) => {
        const control = formControlsCopy[controlName as keyof ForgotPasswordControls];
        if (!control.valid) {
          control.shake = true;
          if (!control.touched) {
            control.touched = true;
            const [isValid, newErrorMessage] = validateControl(control.value, control.validation, control.name);
            control.valid = isValid;
            control.errorMessage = newErrorMessage;
          }
        }
      });
      setFormControls(formControlsCopy);
      setTimeout(() => {
        const resetControls = { ...formControls };
        Object.values(resetControls).forEach((control) => {
          control.shake = false;
        });
        setFormControls(resetControls);
      }, 300);
    }
  }

  async function handleSubmit(e: FormEvent<HTMLFormElement>) {
    e.preventDefault();
    if (!IsFormValid()) {
      shakeInvalidElems();
    } else {
      setIsSubmitLoading(true);

      try {
        await Agent.Auth.forgotPassword({
          email: formControls.email.value,
          clientUri: "http://localhost:3000/resetpassword",
        });
        toast.success("Password reset link has been sent. Check your email.");
        navigate("/login");
      } catch (error: any) {
        if (!error?.response) {
          console.log("No Server Response");
        } else if (error.response.status === 404) {
          const formControlsCopy = { ...formControls };
          formControlsCopy.email.valid = false;
          formControlsCopy.email.errorMessage = "* The user was not found";
          formControlsCopy.email.touched = true;
          setFormControls(formControlsCopy);
        } else if (error.response.status === 403) {
          const formControlsCopy = { ...formControls };
          formControlsCopy.email.valid = false;
          formControlsCopy.email.errorMessage = "* Confirm your email first";
          formControlsCopy.email.touched = true;
          setFormControls(formControlsCopy);
        } else if (error.response?.status === 400) {
          const errorData = error.response.data;
          const formControlsCopy = {
            ...formControls,
          };
          if (errorData?.errors) {
            for (const field in errorData.errors) {
              const messages = errorData.errors[field];
              const key = field.toLowerCase() as keyof ForgotPasswordControls;

              if (formControlsCopy[key]) {
                formControlsCopy[key].valid = false;
                formControlsCopy[key].errorMessage = "* " + messages.join(", ");
                formControlsCopy[key].touched = true;
              }
            }
          }
          setFormControls(formControlsCopy);
        } else {
          toast.error("Something went wrong. Please try again.");
          console.error("Error during login:", error.response || error.message);
        }
        shakeInvalidElems();
      } finally {
        setIsSubmitLoading(false);
      }
    }
  }

  return (
    <div className="authWindow">
      <div className="authContainer forgotPasswordContainer">
        <div className="logo ">
          <img src={logo} alt="logo" />
          <div className="dash"></div>
        </div>
        <div className="infoText">
          <p>
            &nbsp;&nbsp;&nbsp;&nbsp;Forgot your password? No worries! Enter your registered email address below and we'll send you a link to reset
            your password. Make sure to check your spam or junk folder if you don't see the email within a few minutes.
          </p>
        </div>
        <form action="#" method="POST" onSubmit={handleSubmit} noValidate>
          {Object.keys(formControls).map((controlName, index) => {
            const key = controlName as keyof ForgotPasswordControls;
            const control = formControls[key];
            return (
              <Input
                key={`${index}_logInField`}
                type={control.type}
                name={control.name}
                label={control.label}
                value={control.value}
                valid={control.valid}
                errorMessage={control.errorMessage}
                onChange={(e) => changeHandler(e, key)}
                touched={control.touched}
                shake={control.shake}
              />
            );
          })}

          <div className="placeholder"></div>
          <button type="submit" className="submitBtn " disabled={isSubmitLoading ? true : false}>
            {isSubmitLoading ? <img src={loadanimation} alt="loading..."></img> : <p>Submit</p>}
          </button>
        </form>
      </div>
    </div>
  );
}
