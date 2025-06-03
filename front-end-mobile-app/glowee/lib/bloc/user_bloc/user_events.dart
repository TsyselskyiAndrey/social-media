import 'dart:io';

abstract class UserEvent {}

class LoadUserProfileInfo extends UserEvent {}

class UpdateUserProfileBtnClicked extends UserEvent {
  String firstName;
  String lastName;
  String username;
  String Biography;
  String Birthday;
  File? ProfilePhoto;

  UpdateUserProfileBtnClicked({
    required this.firstName,
    required this.lastName,
    required this.username,
    required this.Biography,
    required this.Birthday,
    required this.ProfilePhoto,
  });
}

class FollowUserBtnClicked extends UserEvent {}
