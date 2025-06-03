class CommentUser {
  int id;
  String userName;
  String? profileImageUrl;

  CommentUser({
    required this.id,
    required this.userName,
    this.profileImageUrl,
  });

  factory CommentUser.fromJson(Map<String, dynamic> json) {
    return CommentUser(
      id: json['id'] as int,
      userName: json['userName'] as String,
      profileImageUrl: json['profileImageUrl'] as String?,
    );
  }
}
