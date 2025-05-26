import 'package:flutter/material.dart';
import 'package:glowee/util/app_localizations.dart';
import 'package:glowee/util/locale_notifier.dart';

class InterfaceScreen extends StatefulWidget {
  final void Function(bool isDarkTheme)? onThemeChange;

  const InterfaceScreen({super.key, this.onThemeChange});

  @override
  State<InterfaceScreen> createState() => _InterfaceScreenState();
}

class _InterfaceScreenState extends State<InterfaceScreen> {
  bool _isDarkTheme = false;
  bool _isPrivateAccount = true;
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
    return Scaffold(
      appBar: AppBar(
        title: Text(AppLocalizations.of(context)!.settings),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              AppLocalizations.of(context)!.done,
              style: const TextStyle(color: Colors.blue),
            ),
          ),
        ],
        leading: IconButton(
          icon: const Icon(Icons.arrow_back),
          onPressed: () => Navigator.pop(context),
        ),
      ),
      body: ListView(
        children: [
          Padding(
            padding: const EdgeInsets.all(16.0),
            child: Text(
              AppLocalizations.of(context)!.general,
              style: const TextStyle(
                fontSize: 18,
                fontWeight: FontWeight.bold,
              ),
            ),
          ),
          _buildThemeSetting(context),
          _buildPrivacySetting(context),
          _buildLanguageSetting(context),
        ],
      ),
    );
  }

  Widget _buildThemeSetting(BuildContext context) {
    return ListTile(
      title: Text(AppLocalizations.of(context)!.theme),
      trailing: DropdownButton<String>(
        value: _isDarkTheme
            ? AppLocalizations.of(context)!.dark
            : AppLocalizations.of(context)!.light,
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
        onChanged: (String? newValue) {
          if (newValue != null) {
            setState(() {
              _isDarkTheme = newValue == AppLocalizations.of(context)!.dark;
            });
            widget.onThemeChange?.call(_isDarkTheme);
          }
        },
      ),
    );
  }

  Widget _buildPrivacySetting(BuildContext context) {
    return SwitchListTile(
      title: const Text('Private Account'), // Consider adding to localization
      value: _isPrivateAccount,
      onChanged: (bool value) {
        setState(() {
          _isPrivateAccount = value;
        });
      },
    );
  }

  Widget _buildLanguageSetting(BuildContext context) {
    return ListTile(
      title: Text(AppLocalizations.of(context)!.language),
      trailing: DropdownButton<String>(
        value: _selectedLanguage,
        items: _languages.entries.map((entry) {
          return DropdownMenuItem<String>(
            value: entry.value,
            child: Text(entry.value),
          );
        }).toList(),
        onChanged: (String? newValue) {
          if (newValue != null) {
            setState(() {
              _selectedLanguage = newValue;
              final newLocale = _languages.entries
                  .firstWhere((entry) => entry.value == newValue)
                  .key;
              localeNotifier.value = Locale(newLocale);
            });
          }
        },
      ),
    );
  }
}