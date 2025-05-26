import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';

final themeNotifier = ValueNotifier<ThemeMode>(ThemeMode.light);

Future<void> loadTheme() async {
  final prefs = await SharedPreferences.getInstance();
  final isDark = prefs.getBool('isDarkTheme') ?? false;
  themeNotifier.value = isDark ? ThemeMode.dark : ThemeMode.light;
}

Future<void> saveTheme(bool isDark) async {
  final prefs = await SharedPreferences.getInstance();
  await prefs.setBool('isDarkTheme', isDark);
  themeNotifier.value = isDark ? ThemeMode.dark : ThemeMode.light;
}
