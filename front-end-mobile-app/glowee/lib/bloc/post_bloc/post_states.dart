import 'package:glowee/model/post.dart';
import 'package:glowee/model/tag.dart';

abstract class PostState {}

class PostLoading extends PostState {}

class PostLoaded extends PostState {
  final List<Post> posts;

  PostLoaded({required this.posts});

  PostLoaded copyWith({required posts}) {
    return PostLoaded(posts: posts);
  }
}

class PostStepSuccess extends PostState {
  PostStepSuccess();
}

class PostError extends PostState {
  final List<String> messages;

  PostError({required this.messages});
}

class TagsLoaded extends PostState {
  final List<Tag> tags;

  TagsLoaded({required this.tags});
}
