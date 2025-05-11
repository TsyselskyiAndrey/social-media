import { FormControl } from "../../../Types/FormControl";
import "./SignupPage.css";
import logo from "../../../Assets/logo.png";
import loadanimation from "../../../Assets/loadanimation.gif";
import defaultProfilePicture from "../../../Assets/default_profile_picture.jpg";
import { useState, ChangeEvent, FormEvent } from "react";
import { useNavigate } from "react-router-dom";
import validateControl from "../../../Utils/GeneralValidation";
import Input from "../../../Components/Input/Input";
import Agent from "../../../API/agent";

type FormStep = Record<string, FormControl>;
type SignUpFormControls = FormStep[];

export default function SignupPage() {
  const [formControls, setFormControls] = useState<SignUpFormControls>([
    {
      username: {
        type: "username",
        name: "username",
        label: "Username:",
        errorMessage: "",
        value: "",
        valid: false,
        validation: {
          required: true,
          username: true,
          allowSpaces: false,
        },
        touched: false,
        shake: false,
      },
      email: {
        type: "email",
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
    },
    {
      firstname: {
        type: "text",
        name: "name",
        label: "First Name:",
        errorMessage: "",
        value: "",
        valid: false,
        validation: {
          required: true,
          allowSpaces: false,
          allowSymbols: false,
          allowNums: false,
          minLength: 2,
          maxLength: 20,
        },
        touched: false,
        shake: false,
      },
      lastname: {
        type: "text",
        name: "surname",
        label: "Last Name:",
        errorMessage: "",
        value: "",
        valid: false,
        validation: {
          required: true,
          allowSpaces: false,
          allowSymbols: false,
          allowNums: false,
          minLength: 2,
          maxLength: 20,
        },
        touched: false,
        shake: false,
      },
      birthdate: {
        type: "date",
        name: "birthdate",
        label: "Birth date (MM/DD/YYYY):",
        errorMessage: "",
        value: "",
        valid: false,
        validation: {
          required: true,
          date: true,
        },
        touched: false,
        shake: false,
      },
    },
    {
      code: {
        type: "code",
        name: "code",
        label: "",
        errorMessage: "",
        value: "",
        valid: false,
        validation: {
          code: true,
        },
        touched: false,
        shake: false,
      },
    },
  ]);
  const [profilePictureUrl, setProfilePictureUrl] = useState<string>(defaultProfilePicture);
  const [isSubmitLoading, setIsSubmitLoading] = useState(false);
  const [selected, setSelected] = useState(0);
  const [isUploading, setIsUploading] = useState(false);
  const navigate = useNavigate();

  function IsFormValid(): boolean {
    return Object.values(formControls[selected]).every((control) => control.valid);
  }

  function shakeInvalidElems() {
    if (!IsFormValid()) {
      const formControlsCopy = formControls.map((group) => {
        return {
          ...group,
        };
      });
      Object.keys(formControlsCopy[selected]).map((controlName) => {
        const control = formControlsCopy[selected][controlName];
        if (!control.valid) {
          control.shake = true;
          if (!control.touched) {
            control.touched = true;
            const passwordValue = formControls[selected]?.password?.value;
            const [isValid, newErrorMessage] = validateControl(
              control.value,
              control.validation,
              control.name,
              controlName === "confirmpassword" ? passwordValue : ""
            );
            control.valid = isValid;
            control.errorMessage = newErrorMessage;
          }
        }
      });
      setFormControls(formControlsCopy);
      setTimeout(() => {
        const resetControls = formControls.map((group) => {
          return {
            ...group,
          };
        });
        Object.keys(resetControls[selected]).forEach((controlName) => {
          resetControls[selected][controlName].shake = false;
        });
        setFormControls(resetControls);
      }, 300);
    }
  }

  function changeHandler(e: ChangeEvent<HTMLInputElement>, controlName: string) {
    const formControlsCopy = formControls.map((group) => {
      return {
        ...group,
      };
    });
    const control = { ...formControlsCopy[selected][controlName] };

    const passwordValue = formControls[selected]?.password?.value;

    control.value = e.target.value;
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
      const repasswordControl = { ...formControlsCopy[selected]["confirmpassword"] };
      const [isRepasswordValid, rePasswordErrorMessage] = validateControl(
        repasswordControl.value,
        repasswordControl.validation,
        repasswordControl.name,
        control.value
      );
      repasswordControl.valid = isRepasswordValid;
      repasswordControl.errorMessage = rePasswordErrorMessage;
      repasswordControl.touched = true;
      formControlsCopy[selected]["confirmpassword"] = repasswordControl;
    }

    formControlsCopy[selected][controlName] = control;
    setFormControls(formControlsCopy);
  }

  async function continueHandler(e: FormEvent) {
    e.preventDefault();
    if (!IsFormValid()) {
      shakeInvalidElems();
    } else {
      setIsSubmitLoading(true);

      const handleServerErrors = (errorData: any, stepIndex: number) => {
        const formControlsCopy = { ...formControls };

        if (errorData?.errors) {
          for (const field in errorData.errors) {
            const messages = errorData.errors[field];
            const fieldName = field.toLowerCase();

            if (formControlsCopy[stepIndex]?.[fieldName]) {
              formControlsCopy[stepIndex][fieldName].valid = false;
              formControlsCopy[stepIndex][fieldName].errorMessage = "* " + messages.join(", ");
              formControlsCopy[stepIndex][fieldName].touched = true;
            }
          }
        }

        setFormControls(formControlsCopy);
      };

      try {
        if (selected === 0) {
          const signUpData = {
            email: formControls[0].email.value,
            username: formControls[0].username.value,
            password: formControls[0].password.value,
            confirmpassword: formControls[0].confirmpassword.value,
          };

          await Agent.Auth.registerStep1(signUpData);
          setSelected((prev) => prev + 1);
        } else if (selected === 1) {
          const signUpData = {
            firstname: formControls[1].firstname.value,
            lastname: formControls[1].lastname.value,
            birthdate: formControls[1].birthdate.value.replaceAll(" ", ""),
          };

          await Agent.Auth.registerStep2(signUpData);
          setSelected((prev) => prev + 1);
        } else {
          const signUpData = {
            code: formControls[2].code.value,
          };

          await Agent.Auth.registerStep3(signUpData);
          navigate("/login");
        }
      } catch (error: any) {
        if (!error?.response) {
          console.log("No Server Response");
        } else {
          const status = error.response.status;
          const errorData = error.response.data;

          if (status === 400 || status === 409) {
            handleServerErrors(errorData, selected);
          } else if (status === 404) {
            alert(error.message);
          } else {
            console.error("Error during registration:", error.response || error.message);
          }
        }
      } finally {
        setIsSubmitLoading(false);
        shakeInvalidElems();
      }
    }
  }

  function backHandler() {
    if (selected > 0) {
      setSelected((prev) => prev - 1);
    }
  }
  function handleLink(e: React.MouseEvent<HTMLAnchorElement>) {
    e.preventDefault();
    navigate("/login");
  }

  function handleFileUpload(e: ChangeEvent<HTMLInputElement>): void {
    if (isUploading) {
      return;
    }
    const file = e.target.files?.[0];
    if (file) {
      uploadAvatar(file);
    }
  }

  async function uploadAvatar(file: File): Promise<void> {
    if (!file) {
      alert("Please select a file!");
      return;
    }
    setIsUploading(true);
    const formData = new FormData();
    formData.append("file", file);
    try {
      const response = await Agent.Auth.uploadAvatar(file);

      if (response.status === 200) {
        const result = response.data;
        setProfilePictureUrl(result.profilePictureUrl);
      } else {
        alert("Error uploading file.");
      }
    } catch (error) {
      console.error("Error:", error);
      alert("An error occurred.");
    } finally {
      setIsUploading(false);
    }
  }

  return (
    <div className="authWindow">
      <div className="authContainer">
        <div className="logo ">
          <img src={logo} alt="logo" />
          <div className="dash"></div>
        </div>

        <form action="#" noValidate>
          <div className="pageSelector ">
            {Object.keys(formControls).map((cn, index) => {
              return <div key={`${index}_selector`} className={"selector " + (selected === index ? "active" : "")}></div>;
            })}
          </div>

          {selected === 1 ? (
            <div className="imageForm">
              <label htmlFor="fileToUpload">
                <div
                  className={"profile-pic " + (isUploading ? "profile-pic-dark" : "")}
                  style={{
                    backgroundImage: `url(${profilePictureUrl})`,
                  }}
                >
                  {isUploading ? (
                    <img src={loadanimation} alt="loading..."></img>
                  ) : (
                    <>
                      <span>📷</span>
                      <span>Change Image</span>
                    </>
                  )}
                </div>
              </label>
              <input type="file" name="fileToUpload" id="fileToUpload" onChange={handleFileUpload} />
            </div>
          ) : (
            <></>
          )}

          {selected === 2 ? (
            <div className={"verifyInfo "}>
              <h2 style={{ textAlign: "center" }}>Verify your email!</h2>
              <p style={{ textAlign: "center" }}>
                We're sending a letter to your email with a code. You may need to check your spam or junk folder. Please enter the code below.
              </p>
            </div>
          ) : (
            <></>
          )}
          {Object.keys(formControls[selected]).map((controlName, index) => {
            const control = formControls[selected][controlName];
            return (
              <Input
                key={`${index}_signUpField`}
                type={control.type}
                name={control.name}
                label={control.label}
                value={control.value}
                valid={control.valid}
                errorMessage={control.errorMessage}
                onChange={(e) => changeHandler(e, controlName)}
                touched={control.touched}
                shake={control.shake}
              />
            );
          })}
          <div className="placeholder"></div>
          <div className="btnContainer ">
            <button type="button" className={"submitBtn btnBack "} onClick={backHandler} disabled={selected === 0 ? true : false}>
              🡐
            </button>
            <button type="button" className={"submitBtn btnContinue "} onClick={continueHandler} disabled={isSubmitLoading ? true : false}>
              {isSubmitLoading ? <img src={loadanimation}></img> : <p>{selected === formControls.length - 1 ? "Sign Up" : "Continue"}</p>}
            </button>
          </div>

          <span className="linkText ">
            Already have an account?{" "}
            <a className="link " href="" onClick={handleLink}>
              Log In
            </a>
          </span>
        </form>
      </div>
    </div>
  );
}
