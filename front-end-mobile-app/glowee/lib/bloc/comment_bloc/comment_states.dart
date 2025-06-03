import 'package:glowee/model/comment.dart';

abstract class CommentState {}

class CommentLoading extends CommentState {}

class CommentsLoaded extends CommentState {
  final List<Comment> comments;
  CommentsLoaded({required this.comments});
}

class CommentStepSuccess extends CommentState {}

class CommentError extends CommentState {
  List<String> messages;
  CommentError({required this.messages});
}
