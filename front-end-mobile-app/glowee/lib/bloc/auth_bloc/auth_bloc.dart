import 'dart:io';

import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:glowee/bloc/auth_bloc/auth_events.dart';
import 'package:glowee/bloc/auth_bloc/auth_states.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:http_parser/http_parser.dart';
import 'package:uuid/uuid.dart';

class AuthBloc extends Bloc<AuthEvent, AuthState> {
  final FlutterSecureStorage storage;

  Future<http.Response> _postJson({
    required String url,
    required Map<String, dynamic> body,
    String? cookie,
  }) async {
    final headers = {
      'Content-Type': 'application/json',
      if (cookie != null && cookie.isNotEmpty) 'Cookie': cookie,
    };
    return await http.post(
      Uri.parse(url),
      headers: headers,
      body: jsonEncode(body),
    );
  }

  List<String> _createErrorList(String responseBody) {
    List<String> errors = [];
    final decoded = jsonDecode(responseBody);
    final errorsMap = decoded["errors"] as Map<String, List<String>>;
    if (errorsMap.isEmpty) {
      return errors;
    }
    for (List<String> fieldErrors in errorsMap.values) {
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
    return 'application/octet-stream';
  }

  Future<String> _getDeviceId() async {
    final _uuid = Uuid();
    const key = 'device_id';
    String? deviceId = await storage.read(key: key);
    if (deviceId == null) {
      deviceId = _uuid.v4();
      await storage.write(key: key, value: deviceId);
    }
    return deviceId;
  }

  void _setCookie(http.Response response) async {
    final setCookie = response.headers['set-cookie'];
    await storage.write(key: 'setCookie', value: setCookie);
  }

  AuthBloc({required this.storage}) : super(NotAuthorized()) {
    on<Register1BtnClicked>((event, emit) async {
      final response = await _postJson(
        url: 'https://10.0.2.2:7048/api/Auth/registration-step-1',
        body: {
          "email": event.email,
          "userName": event.userName,
          "password": event.password,
          "confirmPassword": event.confirmPassword
        },
      );
      if (response.statusCode == 200) {
        _setCookie(response);
        emit(AuthStepSucess(flow: AuthFlow.RegisterStep1));
      } else {
        List<String> errors = _createErrorList(response.body);
        emit(AuthError(messages: errors));
      }
    });

    on<Register2BtnClicked>((event, emit) async {
      final registrationToken = await storage.read(key: 'setCookie');
      final response = await _postJson(
        url: 'https://10.0.2.2:7048/api/Auth/registration-step-2',
        body: {
          "firstName": event.firstName,
          "lastName": event.lastName,
          "birthDate": event.birthDate,
        },
        cookie: registrationToken != null ? registrationToken : "",
      );
      if (response.statusCode == 200) {
        emit(AuthStepSucess(flow: AuthFlow.RegisterStep2));
      } else {
        List<String> errors = _createErrorList(response.body);
        emit(AuthError(messages: errors));
      }
    });

    on<AddPhotoWhileSignUpBtnClicked>(
      (event, emit) async {
        final registrationToken = await storage.read(key: 'setCookie');
        final file = File(event.file.path);
        final fileBytes = await file.readAsBytes();
        final filename = event.file.path.split('/').last;
        final mimeTypeStr = _getMimeType(filename);
        final mimeTypeParts = mimeTypeStr.split('/');
        final uri =
            Uri.parse('https://10.0.2.2:7048/api/Auth/upload-profile-image');
        final request = http.MultipartRequest('POST', uri);
        request.files.add(
          http.MultipartFile.fromBytes(
            'File',
            fileBytes,
            filename: filename,
            contentType: MediaType(mimeTypeParts[0], mimeTypeParts[1]),
          ),
        );
        request.headers['Cookie'] =
            registrationToken != null ? registrationToken : "";
        final response = await request.send();
        final responseBody = await http.Response.fromStream(response);
        if (response.statusCode == 200) {
          emit(AuthStepSucess(flow: AuthFlow.UploadPhoto));
        } else {
          List<String> errors = _createErrorList(responseBody.body);
          emit(AuthError(messages: errors));
        }
      },
    );
  }
}
