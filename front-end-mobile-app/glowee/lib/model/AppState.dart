import 'package:flutter/material.dart';

class AppState extends ChangeNotifier {
  ThemeMode _themeMode = ThemeMode.light;
  Locale _locale = const Locale('en');

  ThemeMode get themeMode => _themeMode;
  Locale get locale => _locale;

  void toggleTheme(bool isDark) {
    _themeMode = isDark ? ThemeMode.dark : ThemeMode.light;
    notifyListeners();
  }

  void changeLanguage(String languageCode) {
    _locale = Locale(languageCode);
    notifyListeners();
  }
}