import 'package:glowee/model/comment_user.dart';

class Comment {
  int id;
  CommentUser author;
  String content;
  List<Comment> childComments;
  bool isLiked;
  int likes;

  Comment({
    required this.id,
    required this.author,
    required this.content,
    required this.childComments,
    required this.isLiked,
    required this.likes,
  });

  factory Comment.fromJson(Map<String, dynamic> json) {
    return Comment(
      id: json['id'] as int,
      author: CommentUser.fromJson(json['author'] as Map<String, dynamic>),
      content: json['content'] as String,
      childComments: (json['childComments'] as List<dynamic>)
          .map((e) => Comment.fromJson(e as Map<String, dynamic>))
          .toList(),
      isLiked: json['isLiked'] as bool,
      likes: json['likes'] as int,
    );
  }
}
