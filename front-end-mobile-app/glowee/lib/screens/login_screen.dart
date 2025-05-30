import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:glowee/bloc/auth_bloc/auth_bloc.dart';
import 'package:glowee/bloc/auth_bloc/auth_events.dart';
import 'package:glowee/bloc/auth_bloc/auth_states.dart';
import 'package:glowee/screens/email_enter.dart';
import 'package:glowee/screens/feed.dart';
import 'package:glowee/screens/register.dart';
import 'package:glowee/util/error_dialog.dart';
import 'package:google_sign_in/google_sign_in.dart';
import 'package:device_info_plus/device_info_plus.dart';

class LoginScreen extends StatefulWidget {
  final VoidCallback? onSignUpTap;

  const LoginScreen({this.onSignUpTap, super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final TextEditingController login = TextEditingController();
  final FocusNode loginFocus = FocusNode();
  final GoogleSignIn _googleSignIn = GoogleSignIn(
    scopes: ['email', 'openid'],
    serverClientId: dotenv.env['SERVER_CLIENT_ID'],
  );
  final TextEditingController password = TextEditingController();
  final FocusNode passwordFocus = FocusNode();

  @override
  void dispose() {
    login.dispose();
    password.dispose();
    loginFocus.dispose();
    passwordFocus.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return BlocListener<AuthBloc, AuthState>(
      listener: (context, state) async {
        if (state is Authorized) {
          Navigator.pushReplacement(
            context,
            MaterialPageRoute(
              builder: (context) => BlocProvider(
                create: (context) => AuthBloc(),
                child: FeedScreen(),
              ),
            ),
          );
        } else if (state is AuthError) {
          await showErrorDialog(context, state.messages);
        }
      },
      child: Scaffold(
        resizeToAvoidBottomInset: false,
        body: Container(
          width: double.infinity,
          height: double.infinity,
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: [
                Color.fromRGBO(17, 140, 140, 1.0),
                Color.fromRGBO(38, 75, 198, 1.0),
                Color.fromRGBO(242, 188, 23, 1.0)
              ],
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
          ),
          child: SafeArea(
            child: Column(
              children: [
                SizedBox(width: 96.w, height: 100.h),
                Center(
                  child: Image.asset('assets/images/logo.png'),
                ),
                SizedBox(height: 15.h),
                _buildGreeting(),
                SizedBox(height: 50.h),
                _buildTextField(login, loginFocus, 'Username', Icons.email),
                SizedBox(height: 15.h),
                _buildTextField(
                    password, passwordFocus, 'Password', Icons.lock),
                SizedBox(height: 15.h),
                _buildForgotPassword(),
                SizedBox(height: 15.h),
                _buildLoginButton(context),
                SizedBox(height: 15.h),
                _buildGoogleSignInButton(),
                SizedBox(height: 80.h),
                _buildSignUpPrompt(),
              ],
            ),
          ),
        ),
      ),
    );
  }

  Future<void> _signInWithGoogle(BuildContext context) async {
    try {
      final GoogleSignInAccount? googleUser = await _googleSignIn.signIn();
      if (googleUser != null) {
        print("User signed in with Google: ${googleUser.displayName}");
        final googleAuth = await googleUser.authentication;
        final idToken = googleAuth.idToken;
        final deviceId = await _getDeviceId();
        context.read<AuthBloc>().add(LogInWithGoogleBtnClciked(
              codeOrIdToken: idToken ?? "",
              //deviceId: deviceId,
            ));
      }
    } catch (error) {
      print("Google sign-in error: $error");
    }
  }

  Future<String> _getDeviceId() async {
    final deviceInfo = DeviceInfoPlugin();
    final androidInfo = await deviceInfo.androidInfo;
    return androidInfo.id ?? 'unknown';
  }

  Widget _buildGoogleSignInButton() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 10.w),
      child: InkWell(
        onTap: () => _signInWithGoogle(context),
        child: Container(
          alignment: Alignment.center,
          width: double.infinity,
          height: 44.h,
          decoration: BoxDecoration(
            color: Colors.white.withOpacity(0.2),
            borderRadius: BorderRadius.circular(10.r),
            border: Border.all(
              color: Colors.white,
              width: 2.w,
            ),
          ),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Icon(Icons.login, color: Colors.white),
              SizedBox(width: 10.w),
              Text(
                'Sign in with Google',
                style: TextStyle(
                  fontSize: 18.sp,
                  color: Colors.white,
                  fontWeight: FontWeight.bold,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildForgotPassword() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 10.w),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.end,
        children: [
          Text(
            "Forgot password? Don’t worry",
            style: TextStyle(fontSize: 14.sp, color: Colors.white),
          ),
          TextButton(
            onPressed: () {
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) => BlocProvider(
                    create: (context) => AuthBloc(),
                    child: const EmailEnter(),
                  ),
                ),
              );
            },
            child: Text(
              "reset it here! ",
              style: TextStyle(
                fontSize: 14,
                color: Colors.yellow,
                fontWeight: FontWeight.bold,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildSignUpPrompt() {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        Text(
          "Don't have an account?",
          style: TextStyle(color: Colors.white),
        ),
        TextButton(
          onPressed: () {
            Navigator.pushReplacement(
              context,
              MaterialPageRoute(
                builder: (context) => BlocProvider(
                  create: (context) => AuthBloc(),
                  child: Register(),
                ),
              ),
            );
          },
          child: Text(
            "Sign up",
            style: TextStyle(
              fontSize: 23,
              color: Colors.yellow,
              fontWeight: FontWeight.bold,
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildLoginButton(BuildContext context) {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 10.w),
      child: InkWell(
        onTap: () {
          context.read<AuthBloc>().add(
                LoginBtnClicked(
                  login: login.text,
                  password: password.text,
                ),
              );
        },
        child: Container(
          alignment: Alignment.center,
          width: double.infinity,
          height: 44.h,
          decoration: BoxDecoration(
            color: Colors.white.withOpacity(0.2),
            borderRadius: BorderRadius.circular(10.r),
            border: Border.all(
              color: Colors.white,
              width: 2.w,
            ),
          ),
          child: Text(
            'Log in',
            style: TextStyle(
              fontSize: 23.sp,
              color: Colors.white,
              fontWeight: FontWeight.bold,
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildGreeting() {
    return Padding(
      padding: EdgeInsets.only(left: 20.w),
      child: GestureDetector(
        onTap: () {
          print("Forgot password tapped");
        },
        child: Text(
          'Sign up to see photos and videos of your friends.',
          style: TextStyle(
            fontSize: 13.sp,
            color: Colors.white,
            fontWeight: FontWeight.w500,
          ),
        ),
      ),
    );
  }

  Widget _buildTextField(
    TextEditingController controller,
    FocusNode focusNode,
    String hint,
    IconData icon,
  ) {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 10.w),
      child: Container(
        height: 44.h,
        decoration: BoxDecoration(
          color: Colors.white.withOpacity(0.2),
          borderRadius: BorderRadius.circular(5.r),
        ),
        child: TextField(
          style: TextStyle(fontSize: 18.sp, color: Colors.white),
          controller: controller,
          focusNode: focusNode,
          decoration: InputDecoration(
            hintText: hint,
            prefixIcon: Icon(
              icon,
              color: Colors.white,
            ),
            contentPadding:
                EdgeInsets.symmetric(horizontal: 15.w, vertical: 15.h),
            enabledBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(5.r),
              borderSide: BorderSide(width: 2.w, color: Colors.white),
            ),
            focusedBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(5.r),
              borderSide: BorderSide(width: 2.w, color: Colors.white),
            ),
          ),
        ),
      ),
    );
  }
}
