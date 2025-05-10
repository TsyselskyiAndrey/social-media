export interface ValidationRules {
  required?: boolean;
  allowSpaces?: boolean;
  login?: boolean;
  email?: boolean;
  username?: boolean;
  allowNums?: boolean;
  allowSymbols?: boolean;
  requireNums?: boolean;
  requireBothCases?: boolean;
  minLength?: number;
  maxLength?: number;
  code?: boolean;
  date?: boolean;
  confirmpassword?: boolean;
}
