import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:glowee/bloc/auth_bloc/auth_bloc.dart';

import 'package:glowee/screens/email_enter.dart';

class EmailPasswordConfirmationScreen extends StatefulWidget {
  final VoidCallback? onSignUpTap;
  const EmailPasswordConfirmationScreen({this.onSignUpTap, super.key});

  @override
  State<EmailPasswordConfirmationScreen> createState() =>
      _EmailPasswordConfirmationScreenState();
}

class _EmailPasswordConfirmationScreenState
    extends State<EmailPasswordConfirmationScreen> {
  final email = TextEditingController();
  final password = TextEditingController();
  final passwordConfirme = TextEditingController();
  final username = TextEditingController();
  final bio = TextEditingController();
  final _formKey = GlobalKey<FormState>();
  final email_F = FocusNode();
  final password_F = FocusNode();
  final passwordConfirme_F = FocusNode();
  final username_F = FocusNode();
  final bio_F = FocusNode();

  @override
  void initState() {
    super.initState();
  }

  @override
  void dispose() {
    email.dispose();
    password.dispose();
    passwordConfirme.dispose();
    username.dispose();
    bio.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      resizeToAvoidBottomInset: false,
      backgroundColor: Colors.white,
      body: Container(
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
          child: Form(
            key: _formKey,
            child: Column(
              children: [
                SizedBox(height: 70.h),
                Center(child: Image.asset('assets/images/logo.png')),
                SizedBox(height: 90.h),
                EnterEmailText(),
                SizedBox(height: 25.h),
                buildForgotPassword(),
                SizedBox(height: 20.h),
                SizedBox(height: 15.h),
              ],
            ),
          ),
        ),
      ),
    );
  }

  Widget buildForgotPassword() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 10.w),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.end,
        children: [
          Text(
            "Didn’t receive the link? ",
            style: TextStyle(fontSize: 18.sp, color: Colors.white),
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
              "Resend email ",
              style: TextStyle(
                fontSize: 18,
                color: Colors.yellow,
                fontWeight: FontWeight.bold,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget EnterEmailText() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 40.w),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        children: [
          Text(
            "We sent you an email with link to",
            textAlign: TextAlign.center,
            style: TextStyle(fontSize: 18.sp, color: Colors.white),
          ),
          SizedBox(height: 5.h),
          Text(
            "create new password. Check your inbox",
            textAlign: TextAlign.center,
            style: TextStyle(fontSize: 18.sp, color: Colors.white),
          ),
        ],
      ),
    );
  }

  Padding Textfild(
    TextEditingController controll,
    FocusNode focusNode,
    String typename,
    IconData icon, {
    String? Function(String?)? validator,
  }) {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 10.w),
      child: TextFormField(
        style: TextStyle(fontSize: 18.sp, color: Colors.black),
        controller: controll,
        focusNode: focusNode,
        validator: validator,
        decoration: InputDecoration(
          hintText: typename,
          prefixIcon: Icon(icon,
              color: focusNode.hasFocus ? Colors.black : Colors.grey[600]),
          contentPadding:
              EdgeInsets.symmetric(horizontal: 15.w, vertical: 15.h),
          filled: true,
          fillColor: Colors.white,
          enabledBorder: OutlineInputBorder(
            borderRadius: BorderRadius.circular(5.r),
            borderSide: BorderSide(width: 1.w, color: Colors.black),
          ),
          focusedBorder: OutlineInputBorder(
            borderRadius: BorderRadius.circular(5.r),
            borderSide: BorderSide(width: 2.w, color: Colors.black),
          ),
          errorBorder: OutlineInputBorder(
            borderRadius: BorderRadius.circular(5.r),
            borderSide: BorderSide(width: 1.5.w, color: Colors.red),
          ),
          focusedErrorBorder: OutlineInputBorder(
            borderRadius: BorderRadius.circular(5.r),
            borderSide: BorderSide(width: 2.w, color: Colors.red),
          ),
        ),
      ),
    );
  }
}
