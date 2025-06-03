abstract class CommentEvent {}

class GetCommentsBtnClicked extends CommentEvent {
  final int postId;

  GetCommentsBtnClicked({required this.postId});
}

class CreateCommentBtnClicked extends CommentEvent {
  final String content;
  final int postId;
  final int? parentCommentId;

  CreateCommentBtnClicked({
    required this.content,
    required this.postId,
    required this.parentCommentId,
  });
}

class LikeCommentBtnClicked extends CommentEvent {
  final int commentId;

  LikeCommentBtnClicked({required this.commentId});
}
