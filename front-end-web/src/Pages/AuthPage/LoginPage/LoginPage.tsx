import { useNavigate } from "react-router-dom";
import useAuth from "../../../Hooks/useAuth";
import { useState, ChangeEvent, FormEvent } from "react";
import FingerprintJS from "@fingerprintjs/fingerprintjs";
import { FormControl } from "../../../Types/FormControl";
import Input from "../../../Components/Input/Input";
import validateControl from "../../../Utils/GeneralValidation";
import Agent from "../../../API/agent";
import "./LoginPage.css";
import googleImage from "../../../Assets/google1.png";
import logo from "../../../Assets/logo.png";
import loadanimation from "../../../Assets/loadanimation.gif";
import "../CommonStyles.css";

interface LogInFormControls {
  login: FormControl;
  password: FormControl;
}

export default function LoginPage() {
  const [formControls, setFormControls] = useState<LogInFormControls>({
    login: {
      type: "login",
      name: "login",
      label: "Email or Username:",
      errorMessage: "",
      value: "",
      valid: false,
      validation: {
        required: true,
        login: true,
        allowSpaces: false,
      },
      touched: false,
      shake: false,
    },
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
        maxLength: 100,
      },
      touched: false,
      shake: false,
    },
  });
  const [isSubmitLoading, setIsSubmitLoading] = useState(false);
  const { setAuth } = useAuth();
  const navigate = useNavigate();

  function changeHandler(e: ChangeEvent<HTMLInputElement>, controlName: keyof LogInFormControls) {
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

    if (controlName === "login") {
      formControlsCopy.password = {
        ...formControlsCopy.password,
        touched: false,
        valid: true,
        errorMessage: "",
      };
    }

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
        const control = formControlsCopy[controlName as keyof LogInFormControls];
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

  const getFingerprint = async () => {
    const fp = await FingerprintJS.load();
    const result = await fp.get();
    return result.visitorId;
  };

  async function handleSubmit(e: FormEvent<HTMLFormElement>) {
    e.preventDefault();
    if (!IsFormValid()) {
      shakeInvalidElems();
    } else {
      setIsSubmitLoading(true);

      try {
        const response = await Agent.Auth.login({
          login: formControls.login.value,
          password: formControls.password.value,
          deviceId: await getFingerprint(),
        });
        if (response.data.token) {
          const { token, ...user } = response.data;
          localStorage.setItem("accessToken", token);
          setAuth(user);
          navigate("/");
        }
      } catch (error: any) {
        setAuth(null);
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
              const key = field.toLowerCase() as keyof LogInFormControls;

              if (formControlsCopy[key]) {
                formControlsCopy[key].valid = false;
                formControlsCopy[key].errorMessage = "* " + messages.join(", ");
                formControlsCopy[key].touched = true;
              }
            }
          }
          setFormControls(formControlsCopy);
        } else {
          console.error("Error during login:", error.response || error.message);
        }
        shakeInvalidElems();
      } finally {
        setIsSubmitLoading(false);
      }
    }
  }

  function handleLink(e: React.MouseEvent<HTMLAnchorElement>) {
    e.preventDefault();
    navigate("/signup");
  }

  return (
    <div className="authWindow">
      <div className="authContainer">
        <div className="logo ">
          <img src={logo} alt="logo" />
          <p>Log in to see photos and videos of your friends</p>
          <div className="dash"></div>
        </div>
        <form action="#" method="POST" onSubmit={handleSubmit} noValidate>
          {Object.keys(formControls).map((controlName, index) => {
            const key = controlName as keyof LogInFormControls;
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
            {isSubmitLoading ? <img src={loadanimation} alt="loading..."></img> : <p>Log In</p>}
          </button>

          <span className="linkText ">
            Don't have an account?{" "}
            <a className="link " href="" onClick={handleLink}>
              Sign Up
            </a>
          </span>

          <div className="divider ">
            <div className="line"></div>
            <p>&nbsp;&nbsp;OR&nbsp;&nbsp;</p>
            <div className="line"></div>
          </div>

          <button type="button" className="googleLogin " disabled={isSubmitLoading ? true : false}>
            <img src={googleImage} alt="" className="googleLogo" />
            <p>Continue with Google</p>
          </button>
        </form>
      </div>
    </div>
  );
}
