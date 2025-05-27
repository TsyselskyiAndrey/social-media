import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';

final localeNotifier = ValueNotifier<Locale>(const Locale('en'));

Future<void> loadLocale() async {
  final prefs = await SharedPreferences.getInstance();
  final langCode = prefs.getString('languageCode') ?? 'en';
  localeNotifier.value = Locale(langCode);
}

Future<void> saveLocale(String langCode) async {
  final prefs = await SharedPreferences.getInstance();
  await prefs.setString('languageCode', langCode);
}
