import 'package:glowee/model/post_model.dart';
import 'package:flutter/material.dart';

final ValueNotifier<List<Post>> postsNotifier = ValueNotifier<List<Post>>([]);
List<Post> posts = []; // Your existing posts list

// Function to add a post and notify listeners
void addPost(Post post) {
  posts.add(post);
  postsNotifier.value = List.from(posts);
}
final nameNotifier = ValueNotifier<String>('Software Engineering');
final usernameNotifier = ValueNotifier<String>('software_nure');
final bioNotifier = ValueNotifier<String>('Why this task has 5 scores???');
final birthdayNotifier = ValueNotifier<String>('01.01.2020');
