export interface FormControl {
  type: string;
  name: string;
  label: string;
  errorMessage: string;
  value: string;
  valid: boolean;
  validation: ValidationRules;
  touched: boolean;
  shake: boolean;
}
