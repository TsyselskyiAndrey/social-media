import { useId, useState, useRef } from "react";
import "./Input.css";
import showPassIcon from "../../Assets/showPass.png";
import hidePassIcon from "../../Assets/hidePass.png";
import { KeyboardEvent, ClipboardEvent, ChangeEvent } from "react";

interface InputProps {
  type: string;
  name: string;
  value: string;
  label?: string;
  errorMessage?: string;
  valid?: boolean;
  touched?: boolean;
  shake?: boolean;
  onChange?: (e: ChangeEvent<HTMLInputElement>) => void;
}

export default function Input(props: InputProps) {
  const [showPassword, setShowPassword] = useState(false);
  const [code, setCode] = useState([...Array(6)].map(() => ""));
  const inputs = useRef<Array<HTMLInputElement | null>>([]);
  const id = props.type + "_" + useId();

  function mouseDownHandler(event: React.MouseEvent<HTMLImageElement>) {
    event.preventDefault();
    setShowPassword(!showPassword);
  }

  function updateCode(index: number, value: string) {
    const newCode = [...code];
    newCode[index] = value;
    setCode(newCode);
    const syntheticEvent = {
      target: {
        name: props.name,
        value: newCode.join(""),
      },
    } as ChangeEvent<HTMLInputElement>;
    props.onChange && props.onChange(syntheticEvent);
  }

  function handleKeyDown(e: KeyboardEvent<HTMLInputElement>, index: number) {
    if ((e.ctrlKey || e.metaKey) && e.key === "v") {
      return;
    }
    e.preventDefault();
    if (/^\d$/.test(e.key)) {
      updateCode(index, e.key);
      if (index < code.length - 1) {
        inputs.current[index + 1]?.focus();
      }
    }
    if (e.key === "Backspace" || e.key === "Delete") {
      if (inputs.current[index]?.value !== "") {
        updateCode(index, "");
      } else {
        if (index !== 0) {
          inputs.current[index - 1]?.focus();
        }
      }
    }
    if (e.key === "ArrowLeft") {
      if (index !== 0) {
        inputs.current[index - 1]?.focus();
      }
    }
    if (e.key === "ArrowRight") {
      if (index < code.length - 1) {
        inputs.current[index + 1]?.focus();
      }
    }
  }

  function handlePaste(e: ClipboardEvent<HTMLInputElement>) {
    e.preventDefault();
    const pasteData = e.clipboardData.getData("Text").slice(0, code.length);
    const newCode = [...code];

    pasteData.split("").forEach((char, i) => {
      if (/^\d$/.test(char)) {
        newCode[i] = char;
      }
    });
    setCode(newCode);

    const syntheticEvent = {
      target: {
        name: props.name,
        value: newCode.join(""),
      },
    } as ChangeEvent<HTMLInputElement>;
    props.onChange && props.onChange(syntheticEvent);

    const nextIndex = pasteData.length < code.length ? pasteData.length : code.length - 1;

    inputs.current[nextIndex]?.focus();
  }

  function dateChangeHandler(e: ChangeEvent<HTMLInputElement>) {
    let value = e.target.value.replace(/[^0-9]/g, "");
    let cursorPosition = e.target.selectionStart;

    if (value.length > 8) {
      value = value.slice(0, 8);
    }
    if (value.length >= 3) {
      value = value.slice(0, 2) + " / " + value.slice(2);
    }
    if (value.length >= 8) {
      value = value.slice(0, 7) + " / " + value.slice(7);
    }

    e.target.value = value;

    if (cursorPosition === 3 || cursorPosition === 8) {
      cursorPosition += 3;
    }

    e.target.setSelectionRange(cursorPosition, cursorPosition);
  }

  if (props.type === "code") {
    return (
      <>
        <div className={"verifyInfo "}>
          <h2 style={{ textAlign: "center" }}>Verify your email!</h2>
          <p style={{ textAlign: "justify" }}>
            We're sending a letter to your email with a code. You may need to check your spam or junk folder. Please enter the code below.
          </p>
        </div>
        <input type="hidden" id={id} name={props.name} value={props.value} readOnly></input>
        <div className={"codeContainer "}>
          {code.map((num, index) => (
            <input
              key={`${index}_code`}
              id={`${index}_code`}
              maxLength={1}
              type="text"
              className="codeNum"
              value={num}
              onKeyDown={(e) => handleKeyDown(e, index)}
              onPaste={handlePaste}
              autoComplete="one-time-code"
              ref={(ref) => {
                inputs.current[index] = ref;
              }}
              onChange={() => null}
            ></input>
          ))}
        </div>
        <span className={"errorMessage codeError " + (props.shake ? "shake" : "")}>
          {props.valid || !props.touched ? <span>&nbsp;</span> : props.errorMessage}
        </span>
      </>
    );
  } else {
    return (
      <label htmlFor={id} className={"inp "}>
        <input
          type={showPassword || props.type === "date" ? "text" : props.type}
          id={id}
          name={props.name}
          placeholder="&nbsp;"
          value={props.value}
          onChange={(e) => {
            if (props.type === "date") {
              dateChangeHandler(e);
            }
            props.onChange?.(e);
          }}
          autoComplete="one-time-code"
          style={props.type === "password" ? { paddingRight: "45px" } : {}}
        ></input>
        <span className={"label "}>{props.label}</span>
        <span className={"errorMessage " + (props.shake ? "shake" : "")}>
          {props.valid || !props.touched ? <span>&nbsp;</span> : props.errorMessage}
        </span>
        {props.type === "password" && (
          <img className="showPassword" alt="eye-image" src={showPassword ? showPassIcon : hidePassIcon} onMouseDown={mouseDownHandler}></img>
        )}
      </label>
    );
  }
}
