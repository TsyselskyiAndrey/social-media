import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:glowee/bloc/auth_bloc/auth_bloc.dart';
import 'package:glowee/bloc/auth_bloc/auth_events.dart';
import 'package:glowee/bloc/auth_bloc/auth_states.dart';
import 'package:glowee/screens/login_screen.dart';
import 'package:glowee/screens/register_details.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class Register extends StatefulWidget {
  Register({super.key});

  @override
  State<Register> createState() => _RegisterState();
}

class _RegisterState extends State<Register> {
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
    return BlocListener<AuthBloc, AuthState>(
      listener: (context, state) async {
        if (state is AuthStepSucess && state.flow == AuthFlow.RegisterStep1) {
          Navigator.pushReplacement(
            context,
            MaterialPageRoute(
              builder: (context) => BlocProvider(
                create: (context) => AuthBloc(),
                child: RegisterDetails(emailText: email.text),
              ),
            ),
          );
        }
      },
      child: Scaffold(
        resizeToAvoidBottomInset: false,
        backgroundColor: Colors.white,
        body: Container(
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: [
                Color.fromRGBO(17, 140, 140, 0.7),
                Color.fromRGBO(242, 188, 23, 0.5)
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
                  SizedBox(height: 20.h),
                  Center(child: Image.asset('assets/images/logo.png')),
                  SizedBox(height: 30.h),
                  createAccountText(),
                  SizedBox(height: 35.h),
                  Textfild(
                    email,
                    email_F,
                    'Email',
                    Icons.email,
                    // validator: (value) {
                    //   if (value == null || value.isEmpty) return 'Email is required';
                    //   final emailRegex = RegExp(r'^[\w\.-]+@[\w\.-]+\.\w+$');
                    //   if (!emailRegex.hasMatch(value)) return 'Invalid email format';
                    //   return null;
                    // },
                  ),
                  SizedBox(height: 25.h),
                  Textfild(
                    username,
                    username_F,
                    'Username',
                    Icons.person,
                    // validator: (value) {
                    //   if (value == null || value.isEmpty) return 'Username is required';
                    //   if (RegExp(r'[@?,\*^]').hasMatch(value)) {
                    //     return 'Username contains invalid characters';
                    //   }
                    //
                    //   return null;
                    // },
                  ),
                  SizedBox(height: 25.h),
                  Textfild(
                    password,
                    password_F,
                    'Password',
                    Icons.lock,
                    // validator: (value) {
                    //   if (value == null || value.isEmpty) return 'Password is required';
                    //   if (value.length < 6) return 'Password must be at least 6 characters';
                    //   if (!RegExp(r'[A-Z]').hasMatch(value)) return 'Password must contain an uppercase letter';
                    //   if (!RegExp(r'[0-9]').hasMatch(value)) return 'Password must contain a number';
                    //   if (RegExp(r'[ @?,*^]').hasMatch(value)) return 'Password contains invalid characters';
                    //   return null;
                    // },
                  ),
                  SizedBox(height: 25.h),
                  Textfild(
                    passwordConfirme,
                    passwordConfirme_F,
                    'Confirm Password',
                    Icons.lock_outline,
                    // validator: (value) {
                    //   if (value == null || value.isEmpty) return 'Please confirm your password';
                    //   if (value != password.text) return 'Passwords do not match';
                    //   return null;
                    // }
                  ),
                  SizedBox(height: 20.h),
                  Next(context),
                  SizedBox(height: 15.h),
                  Have(),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }

  Widget createAccountText() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 70.w),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.end,
        children: [
          Text(
            "Create An Account and Sign Up",
            style: TextStyle(fontSize: 14.sp, color: Colors.white),
          ),
        ],
      ),
    );
  }

  Widget Have() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 10.w),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.end,
        children: [
          Text(
            "Already have an account? ",
            style: TextStyle(fontSize: 14.sp, color: Colors.black),
          ),
          GestureDetector(
            onTap: () {
              Navigator.pushReplacement(
                context,
                MaterialPageRoute(
                  builder: (context) => BlocProvider(
                    create: (context) => AuthBloc(),
                    child: const LoginScreen(),
                  ),
                ),
              );
            },
            child: Text(
              "Login",
              style: TextStyle(
                fontSize: 15.sp,
                color: Colors.black,
                fontWeight: FontWeight.bold,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget Next(BuildContext context) {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 10.w),
      child: InkWell(
        onTap: () {
          if (_formKey.currentState!.validate()) {
            context.read<AuthBloc>().add(
                  Register1BtnClicked(
                    email: email.text,
                    userName: username.text,
                    password: password.text,
                    confirmPassword: passwordConfirme.text,
                  ),
                );
          }
        },
        child: Container(
          alignment: Alignment.center,
          width: double.infinity,
          height: 44.h,
          decoration: BoxDecoration(
            color: Colors.blue.withOpacity(0.1),
            borderRadius: BorderRadius.circular(10.r),
            border: Border.all(color: Colors.black, width: 1.w),
          ),
          child: Text(
            'Next',
            style: TextStyle(fontSize: 23.sp, color: Colors.black),
          ),
        ),
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
