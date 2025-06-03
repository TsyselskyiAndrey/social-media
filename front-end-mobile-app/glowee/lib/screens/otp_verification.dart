import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:glowee/bloc/auth_bloc/auth_bloc.dart';
import 'package:glowee/bloc/auth_bloc/auth_events.dart';
import 'package:glowee/bloc/auth_bloc/auth_states.dart';
import 'package:glowee/screens/email_enter.dart';
import 'package:glowee/screens/login_screen.dart';
import 'package:glowee/screens/profile.dart';
import 'package:glowee/util/error_dialog.dart';

class OtpVerificationScreen extends StatefulWidget {
  final String? emailText;

  const OtpVerificationScreen({super.key, this.emailText});

  @override
  State<OtpVerificationScreen> createState() => _OtpVerificationScreenState();
}

class _OtpVerificationScreenState extends State<OtpVerificationScreen> {
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
    email.text = widget.emailText ?? '';
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
    return BlocConsumer<AuthBloc, AuthState>(
      listener: (context, state) async {
        if (state is AuthStepSucess && state.flow == AuthFlow.RegisterStep3) {
          Navigator.pushReplacement(
            context,
            MaterialPageRoute(
              builder: (context) => BlocProvider(
                create: (context) => AuthBloc(),
                child: ProfileScreen(),
              ),
            ),
          );
        } else if (state is AuthError) {
          await showErrorDialog(context, state.messages);
        }
      },
      builder: (context, state) {
        return (state is Authorizing || state is AuthStepSucess)
            ? const Center(
                child: CircularProgressIndicator(
                  color: Colors.white,
                ),
              )
            : Scaffold(
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
                          SizedBox(height: 20.h),
                          Center(child: Image.asset('assets/images/logo.png')),
                          SizedBox(height: 30.h),
                          headerText(),
                          SizedBox(height: 20.h),
                          EnterEmailText(),
                          SizedBox(height: 15.h),
                          DisplayedEmail(),
                          SizedBox(height: 25.h),
                          SizedBox(height: 20.h),
                          TextFormField(
                            controller: email,
                            decoration: InputDecoration(
                              labelText: 'Email',
                              labelStyle: TextStyle(
                                color: Colors.deepPurple,
                                fontWeight: FontWeight.bold,
                                fontSize: 16,
                              ),
                              hintText: 'Введите ваш email',
                              hintStyle:
                                  TextStyle(color: Colors.deepPurple.shade200),
                              filled: true,
                              fillColor: Colors.deepPurple.shade50,
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide(
                                  color: Colors.deepPurple,
                                  width: 2,
                                ),
                              ),
                              focusedBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide(
                                  color: Colors.deepPurpleAccent,
                                  width: 3,
                                ),
                              ),
                              errorBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide(
                                  color: Colors.red,
                                  width: 2,
                                ),
                              ),
                              focusedErrorBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide(
                                  color: Colors.redAccent,
                                  width: 3,
                                ),
                              ),
                            ),
                            style: TextStyle(
                              color: Colors.deepPurple.shade900,
                              fontWeight: FontWeight.w600,
                            ),
                          ),
                          SendEmailBtn(context),
                          SizedBox(height: 15.h),
                          BackBtn(),
                        ],
                      ),
                    ),
                  ),
                ),
              );
      },
    );
  }

  Widget headerText() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 50.w),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.end,
        children: [
          Text(
            "Create An Account and Sign Up",
            style: TextStyle(fontSize: 18.sp, color: Colors.white),
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
            "We will send you a one time ",
            textAlign: TextAlign.center,
            style: TextStyle(fontSize: 18.sp, color: Colors.white),
          ),
          SizedBox(height: 5.h),
          Text(
            "password to your email address",
            textAlign: TextAlign.center,
            style: TextStyle(fontSize: 18.sp, color: Colors.white),
          ),
        ],
      ),
    );
  }

  Widget BackBtn() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 30.w),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.end,
        children: [
          GestureDetector(
            onTap: () {
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) => BlocProvider(
                    create: (context) => AuthBloc(),
                    child: LoginScreen(),
                  ),
                ),
              );
            },
            child: Text(
              "Back",
              style: TextStyle(
                  fontSize: 15.sp,
                  color: Colors.black,
                  fontWeight: FontWeight.bold),
            ),
          )
        ],
      ),
    );
  }

  Widget SendEmailBtn(BuildContext context) {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 10.w),
      child: InkWell(
        onTap: () {
          if (_formKey.currentState!.validate()) {
            context
                .read<AuthBloc>()
                .add(VerifyYourOTPBtnClicked(code: email.text));
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
            'Send',
            style: TextStyle(fontSize: 23.sp, color: Colors.black),
          ),
        ),
      ),
    );
  }

  Widget DisplayedEmail() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 100.w),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.start,
        children: [
          Text(
            widget.emailText ?? '',
            style: TextStyle(fontSize: 18.sp, color: Colors.yellow),
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
