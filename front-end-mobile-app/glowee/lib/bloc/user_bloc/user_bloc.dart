import 'dart:io';

import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:glowee/bloc/user_bloc/user_events.dart';
import 'package:glowee/bloc/user_bloc/user_states.dart';
import 'package:glowee/model/post.dart';
import 'package:glowee/model/user.dart';
import 'package:glowee/secure_storage.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'package:http_parser/http_parser.dart';

class UserBloc extends Bloc<UserEvent, UserState> {
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

  UserBloc() : super(UserLoading()) {
    on<LoadUserProfileInfo>((event, emit) async {
      final accessToken = await _storageService.read(key: 'accessToken') ?? "";

      final response = await _getJson(
        url: '10.0.2.2:7048',
        path: 'api/User/getUserProfileInfo',
        token: accessToken,
      );

      final response1 = await _getJson(
        url: '10.0.2.2:7048',
        path: 'api/Post/getPersonalPosts',
        token: accessToken,
      );

      if (response.statusCode == 200) {
        final Map<String, dynamic> parsed = jsonDecode(response.body);
        final user = User.fromJson(parsed);
        final posts = _parsePosts(response1.body);
        emit(UserLoaded(user: user, posts: posts));
      } else {
        List<String> errors = _createErrorList(response.body);
        emit(UserError(errors: errors));
      }
    });

    on<UpdateUserProfileBtnClicked>((event, emit) async {
      final uri =
          Uri.parse('https://10.0.2.2:7048/api/User/updateUserProfileInfo');

      final request = http.MultipartRequest('PUT', uri);
      final accessToken = await _storageService.read(key: 'accessToken');
      request.fields['FirstName'] = event.firstName;
      request.fields['LastName'] = event.lastName;
      request.fields['Username'] = event.username;
      request.fields['Biography'] = event.Biography;
      request.fields['Birthday'] = event.Birthday;
      request.headers['Authorization'] = 'Bearer $accessToken';

      if (event.ProfilePhoto != null) {
        final file = event.ProfilePhoto as File;
        final bytes = await file.readAsBytes();
        final filename = file.path.split('/').last;

        final mimeTypeStr = _getMimeType(filename);
        final mimeTypeParts = mimeTypeStr.split('/');

        request.files.add(
          http.MultipartFile.fromBytes(
            'ProfilePhoto',
            bytes,
            filename: filename,
            contentType: MediaType(mimeTypeParts[0], mimeTypeParts[1]),
          ),
        );
      }

      final response = await request.send();

      if (response.statusCode == 200) {
        emit(UserStepSuccess());
      } else {
        emit(UserError(errors: []));
      }
    });
  }
}
