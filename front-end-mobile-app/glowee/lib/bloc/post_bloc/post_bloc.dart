import 'dart:io';

import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:glowee/bloc/post_bloc/post_events.dart';
import 'package:glowee/bloc/post_bloc/post_states.dart';
import 'package:glowee/model/post.dart';
import 'package:glowee/secure_storage.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'package:http_parser/http_parser.dart';

class PostBloc extends Bloc<PostEvent, PostState> {
  final SecureStorageService _storageService = SecureStorageService.instance;

  Future<http.Response> _postJson({
    required String url,
    required Map<String, dynamic> body,
    String? token,
  }) async {
    final headers = {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer $token',
    };

    return await http.post(
      Uri.parse(url),
      headers: headers,
      body: jsonEncode(body),
    );
  }

  Future<http.Response> _getJson({
    required String url,
    required String path,
    Map<String, dynamic>? queryParams,
    String? token,
  }) {
    final headers = {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer $token',
    };

    final uri = Uri.https(
        url,
        path,
        queryParams?.map(
          (key, value) => MapEntry(key, value.toString()),
        ));

    return http.get(uri, headers: headers);
  }

  List<String> _createErrorList(String responseBody) {
    List<String> errors = [];
    final decoded = jsonDecode(responseBody);
    final errorsMap = decoded["errors"] as Map<String, dynamic>;
    if (errorsMap.isEmpty) {
      return errors;
    }
    for (dynamic fieldErrors in errorsMap.values) {
      for (String error in fieldErrors) {
        errors.add(error);
      }
    }
    return errors;
  }

  String _getMimeType(String filename) {
    final ext = filename.toLowerCase();
    if (ext.endsWith('.jpg') || ext.endsWith('.jpeg')) {
      return 'image/jpeg';
    }
    if (ext.endsWith('.png')) {
      return 'image/png';
    }
    if (ext.endsWith('.webp')) {
      return 'image/webp';
    }
    if (filename.endsWith('.mp4')) {
      return 'video/mp4';
    }
    if (filename.endsWith('.mov')) {
      return 'video/quicktime';
    }
    if (filename.endsWith('.avi')) {
      return 'video/x-msvideo';
    }
    return 'application/octet-stream';
  }

  List<Post> _parsePosts(String responseBody) {
    final List<dynamic> parsed = jsonDecode(responseBody);
    return parsed
        .map((json) => Post.fromJson(json as Map<String, dynamic>))
        .toList();
  }

  List<Post> _updatePostInfo(
    PostLoaded loadedState,
    int postId,
    bool changeLikes,
  ) {
    final allPosts = loadedState.posts;
    final updatedPosts = allPosts.map((post) {
      if (post.id == postId) {
        if (changeLikes) {
          final isLikedNow = !post.isLiked;
          post.isLiked = isLikedNow;
          if (isLikedNow) {
            post.likes += 1;
          } else {
            post.likes -= 1;
          }
        } else {
          final isSavedNow = !post.isSaved;
          post.isSaved = isSavedNow;
        }
      }
      return post;
    }).toList();
    return updatedPosts;
  }

  PostBloc() : super(PostLoading()) {
    on<LoadPostsEvent>((event, emit) async {
      final initialState = state;
      emit(PostLoading());
      final accessToken = await _storageService.read(key: 'accessToken') ?? "";

      List<String>? newTags;

      if (event.tags != null && event.tags!.isNotEmpty) {
        newTags = event.tags!.map((e) => e.toString()).toList();
      }

      final response = await _getJson(
        url: '10.0.2.2:7048',
        path: 'api/Post/getPosts',
        queryParams: {
          'postAmount': event.postAmount ?? '',
          'postId': event.postId ?? '',
          'userId': event.userId ?? '',
          'postTitle': event.postTitle ?? '',
          'tags': newTags?.join(',') ?? '',
        },
        token: accessToken,
      );

      if (response.statusCode == 200) {
        final posts = _parsePosts(response.body);

        if (initialState is PostLoaded) {
          initialState.posts.addAll(posts);
        }
        emit(PostLoaded(posts: posts));
      } else {
        List<String> errors = _createErrorList(response.body);
        emit(PostError(messages: errors));
      }
    });

    on<CreatePostBtnClicked>(
      (event, emit) async {
        final accessToken = await _storageService.read(key: 'accessToken');
        final uri = Uri.parse('https://10.0.2.2:7048/api/Post/createPost');
        final request = http.MultipartRequest('POST', uri);
        request.headers['Authorization'] = 'Bearer $accessToken';
        request.fields['Caption'] = event.caption;

        for (var i = 0; i < event.tags.length; i++) {
          request.fields['Tags[$i]'] = event.tags[i];
        }

        for (var file in event.postMedias!) {
          final bytes = await File(file.path).readAsBytes();
          final filename = file.path.split('/').last;
          final mimeTypeStr = _getMimeType(filename);
          final mimeTypeParts = mimeTypeStr.split('/');

          request.files.add(
            http.MultipartFile.fromBytes(
              'PostMedias',
              bytes,
              filename: filename,
              contentType: MediaType(mimeTypeParts[0], mimeTypeParts[1]),
            ),
          );
        }

        if (event.thumbnail != null) {
          request.files.add(await http.MultipartFile.fromPath(
            'Thumbnail',
            event.thumbnail!.path,
          ));
        }

        final response = await request.send();
        final responseBody = await http.Response.fromStream(response);
        if (response.statusCode == 200) {
          emit(PostStepSuccess());
        } else {
          List<String> errors = _createErrorList(responseBody.body);
          emit(PostError(messages: errors));
        }
      },
    );

    on<LikePostBtnClicked>((event, emit) async {
      if (state is PostLoaded) {
        final loadedState = state as PostLoaded;
        final updatedPosts = _updatePostInfo(loadedState, event.postId, true);
        emit(loadedState.copyWith(posts: updatedPosts));

        final accessToken = await _storageService.read(key: 'accessToken');
        final response = await _postJson(
          url: 'https://10.0.2.2:7048/api/Post/like',
          body: {
            "postId": event.postId,
          },
          token: accessToken ?? "",
        );
        if (response.statusCode != 200) {
          emit(loadedState);
        }
      }
    });

    on<SavePostBtnClicked>((event, emit) async {
      if (state is PostLoaded) {
        final loadedState = state as PostLoaded;
        final updatedPosts = _updatePostInfo(loadedState, event.postId, false);
        emit(loadedState.copyWith(posts: updatedPosts));

        final accessToken = await _storageService.read(key: 'accessToken');
        final response = await _postJson(
          url: 'https://10.0.2.2:7048/api/Post/save',
          body: {
            "postId": event.postId,
          },
          token: accessToken ?? "",
        );
        if (response.statusCode != 200) {
          emit(loadedState);
        }
      }
    });

    on<LoadSavedPostsEvent>((event, emit) async {
      final accessToken = await _storageService.read(key: 'accessToken') ?? "";
      final response = await _getJson(
        url: '10.0.2.2:7048',
        path: 'api/Post/getSavedPosts',
        token: accessToken,
      );

      if (response.statusCode == 200) {
        final posts = _parsePosts(response.body);
        emit(PostLoaded(posts: posts));
      } else {
        List<String> errors = _createErrorList(response.body);
        emit(PostError(messages: errors));
      }
    });
  }
}
