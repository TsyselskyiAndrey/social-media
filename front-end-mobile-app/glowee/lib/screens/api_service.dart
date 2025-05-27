import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:shared_preferences/shared_preferences.dart';

class ApiService {
  static const String baseUrl = 'https://your-api-url.com'; // Замените на ваш URL
  final FlutterSecureStorage storage = const FlutterSecureStorage();

  static final ApiService _instance = ApiService._internal();
  factory ApiService() => _instance;
  ApiService._internal();

  Future<Map<String, dynamic>> login(String login, String password) async {
    try {
      final response = await http.post(
        Uri.parse('$baseUrl/api/auth/login'),
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'login': login,
          'password': password,
          'deviceId': await _getDeviceId(),
        }),
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        await storage.write(key: 'accessToken', value: data['token']);
        return data;
      } else {
        final error = jsonDecode(response.body);
        throw Exception(error['message'] ?? 'Failed to login: ${response.statusCode}');
      }
    } on http.ClientException catch (e) {
      throw Exception('Network error: ${e.message}');
    } catch (e) {
      throw Exception('Failed to login: ${e.toString()}');
    }
  }

  Future<String> _getDeviceId() async {
    final prefs = await SharedPreferences.getInstance();
    String? deviceId = prefs.getString('deviceId');
    if (deviceId == null) {
      deviceId = 'flutter-${DateTime.now().millisecondsSinceEpoch}';
      await prefs.setString('deviceId', deviceId);
    }
    return deviceId;
  }

  Future<Map<String, dynamic>> refreshToken() async {
    try {
      final accessToken = await storage.read(key: 'accessToken');
      if (accessToken == null) throw Exception('No access token found');

      final response = await http.post(
        Uri.parse('$baseUrl/api/auth/refresh'),
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer $accessToken',
        },
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        await storage.write(key: 'accessToken', value: data['token']);
        return data;
      } else {
        throw Exception('Failed to refresh token: ${response.statusCode}');
      }
    } catch (e) {
      await storage.delete(key: 'accessToken');
      throw e;
    }
  }

  Future<http.Response> _sendRequest(
      String method,
      String endpoint, {
        dynamic body,
        Map<String, String>? headers,
      }) async {
    final uri = Uri.parse('$baseUrl/$endpoint');
    final defaultHeaders = {
      'Content-Type': 'application/json',
    };

    final accessToken = await storage.read(key: 'accessToken');
    if (accessToken != null) {
      defaultHeaders['Authorization'] = 'Bearer $accessToken';
    }

    if (headers != null) {
      defaultHeaders.addAll(headers);
    }

    switch (method.toLowerCase()) {
      case 'get':
        return await http.get(uri, headers: defaultHeaders);
      case 'post':
        return await http.post(
          uri,
          headers: defaultHeaders,
          body: jsonEncode(body),
        );
      case 'put':
        return await http.put(
          uri,
          headers: defaultHeaders,
          body: jsonEncode(body),
        );
      case 'delete':
        return await http.delete(
          uri,
          headers: defaultHeaders,
        );
      default:
        throw Exception('Unsupported HTTP method');
    }
  }
}