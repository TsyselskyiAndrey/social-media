import 'dart:io';

abstract class PostEvent {}

class LoadPostsEvent extends PostEvent {
  final String? postTitle;
  final int postAmount;
  final int? postId;
  final List<int>? tags;
  final int? userId;

  LoadPostsEvent({
    this.postTitle = null,
    required this.postAmount,
    this.postId,
    this.tags = null,
    this.userId = null,
  });
}

class CreatePostBtnClicked extends PostEvent {
  final String caption;
  final List<String> tags;
  final List<File>? postMedias;
  final File? thumbnail;

  CreatePostBtnClicked({
    required this.caption,
    required this.tags,
    this.postMedias = null,
    this.thumbnail = null,
  });
}

class LikePostBtnClicked extends PostEvent {
  final int postId;

  LikePostBtnClicked({required this.postId});
}

class LoadPersonalPostsBtnClicked extends PostEvent {
  LoadPersonalPostsBtnClicked();
}

class SavePostBtnClicked extends PostEvent {
  final int postId;

  SavePostBtnClicked({required this.postId});
}

class LoadSavedPostsEvent extends PostEvent {}

class LoadAllTags extends PostEvent {
  LoadAllTags();
}
