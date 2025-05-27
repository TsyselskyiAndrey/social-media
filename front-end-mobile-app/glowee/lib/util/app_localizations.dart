import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'dart:convert';
import 'dart:async';

class AppLocalizations {
  final Locale locale;

  AppLocalizations(this.locale);

  static AppLocalizations? of(BuildContext context) {
    return Localizations.of<AppLocalizations>(context, AppLocalizations);
  }

  static const LocalizationsDelegate<AppLocalizations> delegate =
  _AppLocalizationsDelegate();

  Map<String, String> _localizedStrings = {};

  Future<bool> load() async {
    try {
      final jsonString = await rootBundle.loadString(
        'assets/locals/${locale.languageCode}.json',
      );
      final jsonMap = json.decode(jsonString) as Map<String, dynamic>;
      _localizedStrings = jsonMap.map((key, value) =>
          MapEntry(key, value.toString()));
      return true;
    } catch (e) {
      debugPrint('Error loading localization: $e');
      return false;
    }
  }

  String translate(String key) => _localizedStrings[key] ?? key;

  // Getter methods for your strings
  String get settings => translate('settings');
  String get done => translate('done');
  String get cancel => translate('cancel');
  String get general => translate('general');
  String get theme => translate('theme');
  String get light => translate('light');
  String get dark => translate('dark');
  String get language => translate('language');
}

class _AppLocalizationsDelegate
    extends LocalizationsDelegate<AppLocalizations> {
  const _AppLocalizationsDelegate();

  @override
  bool isSupported(Locale locale) => ['en', 'uk'].contains(locale.languageCode);

  @override
  Future<AppLocalizations> load(Locale locale) async {
    final localizations = AppLocalizations(locale);
    await localizations.load();
    return localizations;
  }

  @override
  bool shouldReload(_AppLocalizationsDelegate old) => false;
}