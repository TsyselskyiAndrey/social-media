import 'package:glowee/model/post.dart';
import 'package:glowee/model/user.dart';

abstract class UserState {}

class UserLoading extends UserState {}

class UserLoaded extends UserState {
  final User user;
  final List<Post> posts;

  UserLoaded({
    required this.user,
    required this.posts,
  });
}

class UserStepSuccess extends UserState {
  UserStepSuccess();
}

class UserError extends UserState {
  final List<String> errors;

  UserError({required this.errors});
}
