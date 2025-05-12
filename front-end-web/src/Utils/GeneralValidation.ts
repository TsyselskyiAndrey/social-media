import validator from "validator";
import { ValidationRules } from "../Types/ValidationRules";

type ValidationResult = [boolean, string];

export default function validateControl(
  value: string,
  validation: ValidationRules | null | undefined,
  name: string,
  password: string = ""
): ValidationResult {
  if (!validation) {
    return [true, ""];
  }
  if (validation.required && value.trim() === "") {
    return [false, `* This field is required.`];
  }
  if (validation.allowSpaces === false && /\s/.test(value)) {
    return [false, `* This field cannot contain spaces. Please remove any spaces.`];
  }

  if (validation.email) {
    if (value.length > 511) {
      return [false, "* The email address is invalid."];
    }
    if (!validator.isEmail(value)) {
      return [false, "* The email address you entered is invalid."];
    }
    if (!/^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(value)) {
      return [false, "* The email address is invalid."];
    }
  }
  if (validation.username) {
    if (value.length < 3 || value.length > 30) {
      return [false, "* Username is incorrect."];
    }
    if (!/^[a-zA-Z0-9._]+$/.test(value)) {
      return [false, "* Username is incorrect."];
    }
    if (value.includes("..") || value.includes("__")) {
      return [false, "* Username is incorrect."];
    }
  }
  if (validation.login) {
    const isEmail = value.includes("@");

    if (isEmail) {
      if (value.length > 511) {
        return [false, "* The email address is invalid."];
      }
      if (!validator.isEmail(value)) {
        return [false, "* The email address you entered is invalid."];
      }
      if (!/^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(value)) {
        return [false, "* The email address is invalid."];
      }
    } else {
      if (value.length < 3 || value.length > 30) {
        return [false, "* Username is incorrect."];
      }
      if (!/^[a-zA-Z0-9._]+$/.test(value)) {
        return [false, "* Username is incorrect."];
      }
      if (value.includes("..") || value.includes("__")) {
        return [false, "* Username is incorrect."];
      }
    }
  }
  if (validation.allowNums === false && /[0-9]/.test(value)) {
    return [false, `* The ${name} mustn't contain any numbers.`];
  }
  if (validation.allowSymbols === false && /[^a-zA-Z0-9 ]/.test(value)) {
    return [false, `* The ${name} mustn't contain any special symbols or non-latin letters.`];
  }
  if (validation.requireNums && /\d/.test(value) === false) {
    return [false, `* The ${name} must contain numbers.`];
  }
  if (validation.requireBothCases && (/[A-Z]/.test(value) === false || /[a-z]/.test(value) === false)) {
    return [false, `* The ${name} must contain both upper and lower case letters.`];
  }
  if (validation.minLength && value.length < validation.minLength) {
    return [false, `* The ${name} must contain at least ${validation.minLength} symbols.`];
  }
  if (validation.maxLength && value.length > validation.maxLength) {
    return [false, `* The ${name} must contain less than ${validation.maxLength} symbols.`];
  }
  if (validation.code && value.length !== 6) {
    return [false, `* The code is not complete.`];
  }
  if (validation.date) {
    const dateCopy = value.replaceAll(" ", "");
    const parts = dateCopy.split("/");

    if (!/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(dateCopy)) {
      return [false, `* Enter the complete date.`];
    }

    const day = parts[1];
    const month = parts[0];
    const year = parts[2];
    if (validator.isDate(`${year}/${month}/${day}`) === false) {
      return [false, `* The date is not valid.`];
    }

    const dayPart = parseInt(parts[1], 10);
    const monthPart = parseInt(parts[0], 10) - 1;
    const yearPart = parseInt(parts[2], 10);
    const date = new Date(yearPart, monthPart, dayPart);
    if (date.getFullYear() > new Date().getFullYear()) {
      return [false, `* Are you from the future?`];
    }
    if (date.getFullYear() + 13 > new Date().getFullYear()) {
      return [false, `* You are too young.`];
    }
    if (date.getFullYear() + 140 < new Date().getFullYear()) {
      return [false, `* You are too old.`];
    }
  }
  if (validation.confirmpassword && value !== password) {
    return [false, `* The passwords do not match.`];
  }
  return [true, ""];
}
