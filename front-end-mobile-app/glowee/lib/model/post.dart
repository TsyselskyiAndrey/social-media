import 'package:glowee/model/post_media.dart';

class Post {
  int id;
  String authorName;
  String authorIconUrl;
  String? caption;
  String postType;
  List<String> tags;
  int likes;
  int views;
  bool isLiked;
  bool isSaved;
  bool isUninteresting;
  List<PostMedia> postMedia;

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
    required this.postMedia,
  });

  factory Post.fromJson(Map<String, dynamic> json) {
    return Post(
      id: json['id'] as int,
      authorName: json['authorName'] as String,
      authorIconUrl: json['authorIconUrl'] as String,
      caption: json['caption'] as String?,
      postType: json['postType'] as String,
      tags: json['tags'] != null ? List<String>.from(json['tags']) : [],
      likes: json['likes'] as int,
      views: json['views'] as int,
      isLiked: json['isLiked'] as bool,
      isSaved: json['isSaved'] as bool,
      isUninteresting: json['isUninteresting'] as bool,
      postMedia: (json['postMedias'] as List?)
              ?.map((item) => PostMedia.fromJson(item as Map<String, dynamic>))
              .toList() ??
          [],
    );
  }
}
