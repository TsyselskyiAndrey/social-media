import 'package:flutter/material.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:glowee/util/app_localizations.dart';
import 'package:glowee/screens/login_screen.dart';
import 'package:glowee/screens/theme_notifier.dart';
import 'package:glowee/util/locale_notifier.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await loadTheme();
  await loadLocale();
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    final ThemeData lightTheme = ThemeData(
      brightness: Brightness.light,
      scaffoldBackgroundColor: Colors.white,
      colorScheme: const ColorScheme.light(
        primary: Color.fromRGBO(17, 140, 140, 0.7),
        secondary: Color.fromRGBO(242, 188, 23, 0.5),
      ),
      appBarTheme: const AppBarTheme(
        backgroundColor: Color.fromRGBO(242, 188, 23, 0.5),
      ),
    );

    final ThemeData darkTheme = ThemeData(
      brightness: Brightness.dark,
      scaffoldBackgroundColor: const Color.fromRGBO(27, 97, 103, 1),
      colorScheme: const ColorScheme.dark(
        primary: Color.fromRGBO(17, 140, 140, 1.0),
        secondary: Color.fromRGBO(38, 75, 198, 1.0),
      ),
      appBarTheme: const AppBarTheme(
        backgroundColor: Color.fromRGBO(17, 140, 140, 1.0),
      ),
    );

    return ValueListenableBuilder<ThemeMode>(
      valueListenable: themeNotifier,
      builder: (context, themeMode, _) {
        return ValueListenableBuilder<Locale>(
          valueListenable: localeNotifier,
          builder: (context, locale, _) {
            return ScreenUtilInit(
              designSize: const Size(360, 690),
              minTextAdapt: true,
              splitScreenMode: true,
              builder: (context, child) {
                return MaterialApp(
                  debugShowCheckedModeBanner: false,
                  title: 'Glowee',
                  theme: lightTheme,
                  darkTheme: darkTheme,
                  themeMode: themeMode,
                  locale: locale,
                  supportedLocales: const [
                    Locale('en'),
                    Locale('uk'),
                  ],
                  localizationsDelegates: const [
                    AppLocalizations.delegate,
                    GlobalMaterialLocalizations.delegate,
                    GlobalWidgetsLocalizations.delegate,
                    GlobalCupertinoLocalizations.delegate,
                  ],
                  home: LoginScreen(),
                );
              },
            );
          },
        );
      },
    );
  }
}
