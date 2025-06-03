import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:glowee/bloc/comment_bloc/comment_events.dart';
import 'package:glowee/bloc/comment_bloc/comment_states.dart';
import 'package:glowee/model/comment.dart';
import 'package:glowee/secure_storage.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

class CommentBloc extends Bloc<CommentEvent, CommentState> {
  final SecureStorageService _storageService = SecureStorageService.instance;

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

  CommentBloc() : super(CommentLoading()) {
    on<GetCommentsBtnClicked>((event, emit) async {
      emit(CommentLoading());
      final accessToken = await _storageService.read(key: 'accessToken') ?? "";

      final response = await _getJson(
        url: '10.0.2.2:7048',
        path: '/api/Comment/getComments',
        queryParams: {
          'postId': event.postId,
        },
        token: accessToken,
      );

      if (response.statusCode == 200) {
        final List<dynamic> parsed = json.decode(response.body);

        final comments =
            parsed.map<Comment>((json) => Comment.fromJson(json)).toList();

        emit(CommentsLoaded(comments: comments));
      } else {
        List<String> errors = _createErrorList(response.body);
        emit(CommentError(messages: errors));
      }
    });

    on<CreateCommentBtnClicked>((event, emit) async {
      emit(CommentLoading());
      final accessToken = await _storageService.read(key: 'accessToken') ?? "";

      final uri = Uri.parse('https://10.0.2.2:7048/api/Comment/createComment');

      final request = http.MultipartRequest('POST', uri);

      request.fields['Content'] = event.content;
      request.fields['PostId'] = event.postId.toString();
      if (event.parentCommentId != null) {
        request.fields['ParentCommentId'] = event.parentCommentId.toString();
      }

      request.headers['Authorization'] = 'Bearer $accessToken';

      final response = await request.send();

      final responseBody = await response.stream.bytesToString();

      if (response.statusCode == 200) {
        emit(CommentStepSuccess());
      } else {
        List<String> errors = _createErrorList(responseBody);
        emit(CommentError(messages: errors));
      }
    });

    on<LikeCommentBtnClicked>((event, emit) async {
      final accessToken = await _storageService.read(key: 'accessToken');

      final uri = Uri.parse('https://10.0.2.2:7048/api/Comment/likeComment');
      final response = await http.patch(
        uri,
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer $accessToken',
        },
        body: jsonEncode(
          {"commentId": event.commentId},
        ),
      );
    });
  }
}
