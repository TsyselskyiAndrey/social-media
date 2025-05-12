import "./ResetPasswordPage.css";
import "../CommonStyles.css";
import { FormControl } from "../../../Types/FormControl";
import { ChangeEvent, FormEvent, useState } from "react";
import validateControl from "../../../Utils/GeneralValidation";
import Agent from "../../../API/agent";
import Input from "../../../Components/Input/Input";
import logo from "../../../Assets/logo.png";
import loadanimation from "../../../Assets/loadanimation.gif";
import { Navigate, useNavigate, useSearchParams } from "react-router-dom";
import { toast } from "react-toastify";

interface ResetPasswordControls {
  password: FormControl;
  confirmpassword: FormControl;
}

export default function ResetPasswordPage() {
  const [formControls, setFormControls] = useState<ResetPasswordControls>({
    password: {
      type: "password",
      name: "password",
      label: "Password:",
      errorMessage: "",
      value: "",
      valid: false,
      validation: {
        required: true,
        requireNums: true,
        requireBothCases: true,
        allowSpaces: false,
        allowSymbols: false,
        minLength: 12,
        maxLength: 30,
      },
      touched: false,
      shake: false,
    },
    confirmpassword: {
      type: "password",
      name: "re-password",
      label: "Re-Password:",
      errorMessage: "",
      value: "",
      valid: false,
      validation: {
        required: true,
        confirmpassword: true,
      },
      touched: false,
      shake: false,
    },
  });

  const [isSubmitLoading, setIsSubmitLoading] = useState(false);
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  const token = searchParams.get("token");
  const email = searchParams.get("email");

  if (!token || !email) {
    return <Navigate to="/notfound" />;
  }

  function changeHandler(e: ChangeEvent<HTMLInputElement>, controlName: keyof ResetPasswordControls) {
    const formControlsCopy = { ...formControls };
    const control = {
      ...formControlsCopy[controlName],
    };
    control.value = e.target.value;

    const passwordValue = formControls?.password?.value;

    const [isValid, newErrorMessage] = validateControl(
      e.target.value,
      control.validation,
      control.name,
      controlName === "confirmpassword" ? passwordValue : ""
    );
    control.valid = isValid;
    control.errorMessage = newErrorMessage;
    control.touched = true;

    if (controlName === "password") {
      const repasswordControl = { ...formControlsCopy["confirmpassword"] };
      const [isRepasswordValid, rePasswordErrorMessage] = validateControl(
        repasswordControl.value,
        repasswordControl.validation,
        repasswordControl.name,
        control.value
      );
      repasswordControl.valid = isRepasswordValid;
      repasswordControl.errorMessage = rePasswordErrorMessage;
      repasswordControl.touched = true;
      formControlsCopy["confirmpassword"] = repasswordControl;
    }

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
        const control = formControlsCopy[controlName as keyof ResetPasswordControls];
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
        await Agent.Auth.resetPassword({
          password: formControls.password.value,
          confirmpassword: formControls.password.value,
          email: email,
          token: token,
        });
        toast.success("Password reset successful!");
        navigate("/login");
      } catch (error: any) {
        if (!error?.response) {
          console.log("No Server Response");
        } else if (error.response?.status === 400) {
          const errorData = error.response.data;
          const formControlsCopy = {
            ...formControls,
          };
          if (errorData?.errors) {
            for (const field in errorData.errors) {
              const messages = errorData.errors[field];
              const key = field.toLowerCase() as keyof ResetPasswordControls;

              if (formControlsCopy[key]) {
                formControlsCopy[key].valid = false;
                formControlsCopy[key].errorMessage = "* " + messages.join(", ");
                formControlsCopy[key].touched = true;
              }
            }
          }
          setFormControls(formControlsCopy);
        } else {
          toast.error("Reset failed. Please try again.");
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
      <div className="authContainer resetPasswordContainer">
        <div className="logo ">
          <img src={logo} alt="logo" />
          <div className="dash"></div>
        </div>
        <div className="infoText">
          <p>
            &nbsp;&nbsp;&nbsp;&nbsp;Please enter your new password below and confirm it. Make sure your password is strong and secure. After
            submitting, your password will be updated and you can log in with the new credentials.
          </p>
        </div>
        <form action="#" method="POST" onSubmit={handleSubmit} noValidate>
          {Object.keys(formControls).map((controlName, index) => {
            const key = controlName as keyof ResetPasswordControls;
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
            {isSubmitLoading ? <img src={loadanimation} alt="loading..."></img> : <p>Reset</p>}
          </button>
        </form>
      </div>
    </div>
  );
}
