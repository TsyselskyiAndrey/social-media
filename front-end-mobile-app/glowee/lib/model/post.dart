import 'package:glowee/model/post_media.dart';

class Post {
  int id;
  int authorName;
  String authorIconUrl;
  String? caption;
  String postType;
  List<String> tags;
  int likes;
  int views;
  bool isLiked;
  bool isSaved;
  bool isUninteresting;
  List<PostMedia> postMediaDtos;

  Post({
    required this.id,
    required this.authorName,
    required this.authorIconUrl,
    required this.caption,
    required this.postType,
    required this.tags,
    required this.likes,
    required this.views,
    required this.isLiked,
    required this.isSaved,
    required this.isUninteresting,
    required this.postMediaDtos,
  });

  factory Post.fromJson(Map<String, dynamic> json) {
    return Post(
      id: json['id'] as int,
      authorName: json['authorName'] as int,
      authorIconUrl: json['authorIconUrl'] as String,
      caption: json['caption'] as String?,
      postType: json['postType'] as String,
      tags: json['tags'] as List<String>,
      likes: json['likes'] as int,
      views: json['views'] as int,
      isLiked: json['isLiked'] as bool,
      isSaved: json['isSaved'] as bool,
      isUninteresting: json['isUninteresting'] as bool,
      postMediaDtos: json['postMediaDtos'] as List<PostMedia>,
    );
  }
}
