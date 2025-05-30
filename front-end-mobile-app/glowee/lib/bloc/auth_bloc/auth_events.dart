import 'dart:io';

abstract class AuthEvent {}

class LoginBtnClicked extends AuthEvent {
  final String login;
  final String password;
  final String? deviceId;

  LoginBtnClicked({
    required this.login,
    required this.password,
    this.deviceId,
  });
}

class ForgotPaswordBtnClicked extends AuthEvent {
  final String email;
  final String? clientUri;

  ForgotPaswordBtnClicked({
    required this.email,
    this.clientUri,
  });
}

class LogInWithGoogleBtnClciked extends AuthEvent {
  final String codeOrIdToken;
  final String? deviceId;
  final bool isMobile;

  LogInWithGoogleBtnClciked({
    required this.codeOrIdToken,
    this.deviceId,
    this.isMobile = true,
  });
}

class AddPhotoWhileSignUpBtnClicked extends AuthEvent {
  final File file;

  AddPhotoWhileSignUpBtnClicked({required this.file});
}

class Register1BtnClicked extends AuthEvent {
  final String email;
  final String userName;
  final String password;
  final String confirmPassword;

  Register1BtnClicked({
    required this.email,
    required this.userName,
    required this.password,
    required this.confirmPassword,
  });
}

class Register2BtnClicked extends AuthEvent {
  final String firstName;
  final String lastName;
  final String birthDate;

  Register2BtnClicked({
    required this.firstName,
    required this.lastName,
    required this.birthDate,
  });
}

class VerifyYourOTPBtnClicked extends AuthEvent {
  final String code;

  VerifyYourOTPBtnClicked({required this.code});
}

class ResetPasswordBtnClicked extends AuthEvent {
  final String password;
  final String confirmPassword;
  final String email;
  final String? token;

  ResetPasswordBtnClicked({
    required this.password,
    required this.confirmPassword,
    required this.email,
    this.token,
  });
}

class LogoutBtnClicked extends AuthEvent {}
