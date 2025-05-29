enum AuthFlow {
  RegisterStep1,
  RegisterStep2,
  RegisterStep3,
  ForgotPassword,
  UploadPhoto,
  ResetPassword,
}

abstract class AuthState {}

class NotAuthorized extends AuthState {}

class Authorized extends AuthState {
  Authorized();
}

class Authorizing extends AuthState {}

class AuthStepSucess extends AuthState {
  final AuthFlow flow;

  AuthStepSucess({required this.flow});
}

class AuthError extends AuthState {
  List<String> messages;
  AuthError({required this.messages});
}
