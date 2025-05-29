import 'dart:io';

import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:glowee/bloc/auth_bloc/auth_events.dart';
import 'package:glowee/bloc/auth_bloc/auth_states.dart';
import 'package:glowee/secure_storage.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'package:http_parser/http_parser.dart';
import 'package:uuid/uuid.dart';

class AuthBloc extends Bloc<AuthEvent, AuthState> {
  final SecureStorageService _storageService = SecureStorageService.instance;

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
    return 'application/octet-stream';
  }

  Future<String> _getDeviceId() async {
    final _uuid = Uuid();
    const key = 'device_id';
    String? deviceId = await _storageService.read(key: key);
    if (deviceId == null) {
      deviceId = _uuid.v4();
      await _storageService.write(key: key, value: deviceId);
    }
    return deviceId;
  }

  void _setCookie(http.Response response) async {
    final setCookie = response.headers['set-cookie'];
    await _storageService.write(key: 'setCookie', value: setCookie);
  }

  AuthBloc() : super(NotAuthorized()) {
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
      final registrationToken = await _storageService.read(key: 'setCookie');
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
        final registrationToken = await _storageService.read(key: 'setCookie');
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

    on<VerifyYourOTPBtnClicked>((event, emit) async {
      final registrationToken = await _storageService.read(key: 'setCookie');
      final response = await _postJson(
        url: 'https://10.0.2.2:7048/api/Auth/registration-step-3',
        body: {"code": event.code},
        cookie: registrationToken != null ? registrationToken : "",
      );
      print(response.body);
      print(response.headers);
      if (response.statusCode == 200) {
        emit(AuthStepSucess(flow: AuthFlow.RegisterStep3));
      } else {
        List<String> errors = _createErrorList(response.body);
        emit(AuthError(messages: errors));
      }
    });

    on<LoginBtnClicked>((event, emit) async {
      final deviceId = await _getDeviceId();
      final response = await _postJson(
        url: 'https://10.0.2.2:7048/api/Auth/login',
        body: {
          "login": event.login,
          "password": event.password,
          "deviceId": deviceId,
        },
      );
      print(response.body);
      print(response.headers);

      if (response.statusCode == 200) {
        final Map<String, dynamic> decoded = jsonDecode(response.body);
        await _storageService.write(
            key: "accessToken", value: decoded["token"]);
        await _storageService.write(
            key: "refreshToken", value: response.headers["set-cookie"]);
        //_setCookie(response);
        emit(Authorized());
      } else {
        List<String> errors = _createErrorList(response.body);
        emit(AuthError(messages: errors));
      }
    });

    on<LogInWithGoogleBtnClciked>((event, emit) async {
      final deviceId = await _getDeviceId();
      final response = await _postJson(
        url: 'https://10.0.2.2:7048/api/Auth/google-login',
        body: {
          "codeOrIdToken": event.codeOrIdToken,
          "deviceId": deviceId,
          "isMobile": event.isMobile,
        },
      );
      print(response.body);
      print(response.headers);

      if (response.statusCode == 200) {
        final Map<String, dynamic> decoded = jsonDecode(response.body);
        await _storageService.write(
            key: "accessToken", value: decoded["token"]);
        await _storageService.write(
            key: "refreshToken", value: response.headers["set-cookie"]);
        // _setCookie(response);
        emit(Authorized());
      } else {
        List<String> errors = _createErrorList(response.body);
        emit(AuthError(messages: errors));
      }
    });
  }
}
