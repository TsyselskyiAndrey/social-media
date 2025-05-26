import 'package:flutter/material.dart';
import 'package:glowee/util/app_localizations.dart';
import 'package:glowee/util/locale_notifier.dart';
import 'package:glowee/screens/theme_notifier.dart';

class InterfaceScreen extends StatefulWidget {
  final void Function(bool isDarkTheme)? onThemeChange;

  const InterfaceScreen({super.key, this.onThemeChange});

  @override
  State<InterfaceScreen> createState() => _InterfaceScreenState();
}

class _InterfaceScreenState extends State<InterfaceScreen> {
  bool _isDarkTheme = themeNotifier.value == ThemeMode.dark;
  late String _selectedLanguage;

  final Map<String, String> _languages = {
    'en': 'English',
    'uk': 'Українська',
  };

  @override
  void initState() {
    super.initState();
    _selectedLanguage = _languages[localeNotifier.value.languageCode] ?? 'English';
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);
    return Scaffold(
      appBar: AppBar(
        title: Text(AppLocalizations.of(context)!.settings),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              AppLocalizations.of(context)!.done,
              style: TextStyle(color: theme.colorScheme.primary),
            ),
          ),
        ],
        leading: IconButton(
          icon: const Icon(Icons.arrow_back),
          onPressed: () => Navigator.pop(context),
        ),
        backgroundColor: theme.appBarTheme.backgroundColor,
      ),
      backgroundColor: theme.scaffoldBackgroundColor,
      body: ListView(
        children: [
          Padding(
            padding: const EdgeInsets.all(16.0),
            child: Text(
              AppLocalizations.of(context)!.general,
              style: TextStyle(
                fontSize: 18,
                fontWeight: FontWeight.bold,
                color: theme.textTheme.bodyLarge?.color,
              ),
            ),
          ),
          _buildThemeSetting(context),
          _buildLanguageSetting(context),
        ],
      ),
    );
  }

  Widget _buildThemeSetting(BuildContext context) {
    final theme = Theme.of(context);
    return ListTile(
      title: Text(AppLocalizations.of(context)!.theme,
          style: TextStyle(color: theme.textTheme.bodyLarge?.color)),
      trailing: DropdownButton<String>(
        value: _isDarkTheme
            ? AppLocalizations.of(context)!.dark
            : AppLocalizations.of(context)!.light,
        dropdownColor: theme.cardColor,
        items: [
          DropdownMenuItem(
            value: AppLocalizations.of(context)!.light,
            child: Text(AppLocalizations.of(context)!.light),
          ),
          DropdownMenuItem(
            value: AppLocalizations.of(context)!.dark,
            child: Text(AppLocalizations.of(context)!.dark),
          ),
        ],
        onChanged: (String? newValue) async {
          if (newValue != null) {
            final newIsDark = newValue == AppLocalizations.of(context)!.dark;
            setState(() {
              _isDarkTheme = newIsDark;
            });
            widget.onThemeChange?.call(newIsDark);
            await saveTheme(newIsDark);
          }
        },
      ),
    );
  }

  Widget _buildLanguageSetting(BuildContext context) {
    final theme = Theme.of(context);
    return ListTile(
      title: Text(AppLocalizations.of(context)!.language,
          style: TextStyle(color: theme.textTheme.bodyLarge?.color)),
      trailing: DropdownButton<String>(
        value: _selectedLanguage,
        dropdownColor: theme.cardColor,
        items: _languages.entries.map((entry) {
          return DropdownMenuItem<String>(
            value: entry.value,
            child: Text(entry.value),
          );
        }).toList(),
        onChanged: (String? newValue) async {
          if (newValue != null) {
            setState(() {
              _selectedLanguage = newValue;
            });

            final newLocale = _languages.entries
                .firstWhere((entry) => entry.value == newValue)
                .key;

            localeNotifier.value = Locale(newLocale);
            await saveLocale(newLocale);
          }
        },
      ),
    );
  }
}
