import { useNavigate } from "react-router-dom";
import useAuth from "../../../Hooks/useAuth";
import { useState, ChangeEvent, FormEvent } from "react";
import FingerprintJS from "@fingerprintjs/fingerprintjs";
import { FormControl } from "../../../Types/FormControl";
import Input from "../../../Components/Input/Input";
import validateControl from "../../../Utils/GeneralValidation";
import Agent from "../../../API/agent";
import { useGoogleLogin } from "@react-oauth/google";
import "./LoginPage.css";
import googleImage from "../../../Assets/google.png";
import facebookImage from "../../../Assets/facebook.png";
import logo from "../../../Assets/logo.png";
import loadanimation from "../../../Assets/loadanimation.gif";
import "../CommonStyles.css";

declare global {
  interface Window {
    FB: any;
  }
}

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
        } else if (error.response.status === 404) {
          const formControlsCopy = { ...formControls };
          formControlsCopy.login.valid = false;
          formControlsCopy.login.errorMessage = "* The user was not found";
          formControlsCopy.login.touched = true;
          setFormControls(formControlsCopy);
        } else if (error.response.status === 403) {
          const formControlsCopy = { ...formControls };
          formControlsCopy.login.valid = false;
          formControlsCopy.login.errorMessage = "* Confirm your email first";
          formControlsCopy.login.touched = true;
          setFormControls(formControlsCopy);
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

  const handleGoogleLogin = useGoogleLogin({
    flow: "auth-code",
    onSuccess: async (codeResponse) => {
      const code = codeResponse.code;
      const response = await Agent.Auth.googleLogin({
        codeOrIdToken: code,
        deviceId: await getFingerprint(),
        isMobile: false,
      });

      if (response.data.token) {
        const { token, ...user } = response.data;
        localStorage.setItem("accessToken", token);
        setAuth(user);
        navigate("/");
      }
    },
    onError: (errorResponse) => {
      console.error("Google login error:", errorResponse);
    },
  });

  async function handleFacebookLogin() {
    setIsSubmitLoading(true);
    try {
      window.FB.login(
        (response: any) => {
          if (response.authResponse) {
            processFacebookLogin(response.authResponse);
          } else {
            console.error("Facebook login failed");
            setIsSubmitLoading(false);
          }
        },
        { scope: "email,public_profile" }
      );
    } catch (err) {
      console.error("Facebook login error:", err);
      setIsSubmitLoading(false);
    }
  }

  async function processFacebookLogin(authResponse: any) {
    try {
      const accessToken = authResponse.accessToken;

      const res = await Agent.Auth.facebookLogin({
        accessToken,
        deviceId: await getFingerprint(),
      });

      if (res.data.token) {
        const { token, ...user } = res.data;
        localStorage.setItem("accessToken", token);
        setAuth(user);
        navigate("/");
      }
    } catch (error) {
      console.error("Error during Facebook login:", error);
    } finally {
      setIsSubmitLoading(false);
    }
  }

  function handleLinkSignUp(e: React.MouseEvent<HTMLAnchorElement>) {
    e.preventDefault();
    navigate("/signup");
  }

  function handleLinkForgotPassword(e: React.MouseEvent<HTMLAnchorElement>) {
    e.preventDefault();
    navigate("/forgotpassword");
  }

  return (
    <div className="authWindow">
      <div className="authContainer">
        <div className="logo ">
          <img src={logo} alt="logo" />
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
          <span className="linkText ">
            Forgot your password?{" "}
            <a className="link " href="" onClick={handleLinkForgotPassword}>
              Reset Password
            </a>
          </span>
          <div className="placeholder"></div>
          <button type="submit" className="submitBtn " disabled={isSubmitLoading ? true : false}>
            {isSubmitLoading ? <img src={loadanimation} alt="loading..."></img> : <p>Log In</p>}
          </button>

          <span className="linkText ">
            Don't have an account?{" "}
            <a className="link " href="" onClick={handleLinkSignUp}>
              Sign Up
            </a>
          </span>

          <div className="divider ">
            <div className="line"></div>
            <p>&nbsp;&nbsp;OR&nbsp;&nbsp;</p>
            <div className="line"></div>
          </div>

          <button type="button" className="externalServiceLogin" disabled={isSubmitLoading} onClick={() => handleGoogleLogin()}>
            <img src={googleImage} alt="" className="externalServiceLogo" />
            <p>Continue with Google</p>
          </button>

          <button type="button" className="externalServiceLogin" disabled={isSubmitLoading} onClick={handleFacebookLogin}>
            <img src={facebookImage} alt="" className="externalServiceLogo" />
            <p>Continue with Facebook</p>
          </button>
        </form>
      </div>
    </div>
  );
}
