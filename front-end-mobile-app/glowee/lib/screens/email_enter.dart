import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:glowee/bloc/auth_bloc/auth_bloc.dart';
import 'package:glowee/bloc/auth_bloc/auth_events.dart';
import 'package:glowee/bloc/auth_bloc/auth_states.dart';
import 'package:glowee/screens/email_password_text_comfirmation_screen.dart';
import 'package:glowee/screens/login_screen.dart';
import 'package:glowee/util/error_dialog.dart';

class EmailEnter extends StatefulWidget {
  final String? emailText;

  const EmailEnter({Key? key, this.emailText}) : super(key: key);

  @override
  State<EmailEnter> createState() => _EmailEnterState();
}

class _EmailEnterState extends State<EmailEnter> {
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
    print('получен email: ${widget.emailText}');
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
        if (state is AuthStepSucess && state.flow == AuthFlow.EmailSent) {
          Navigator.pushReplacement(
            context,
            MaterialPageRoute(
              builder: (context) => BlocProvider(
                create: (context) => AuthBloc(),
                child: const EmailPasswordConfirmationScreen(),
              ),
            ),
          );
        } else if (state is AuthError) {
          await showErrorDialog(context, state.messages);
        }
      },
      builder: (context, state) {
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
            child: (state is Authorizing || state is AuthStepSucess)
                ? const Center(
                    child: CircularProgressIndicator(
                      color: Colors.white,
                    ),
                  )
                : SafeArea(
                    child: Form(
                      key: _formKey,
                      child: Column(
                        children: [
                          SizedBox(height: 20.h),
                          Center(child: Image.asset('assets/images/logo.png')),
                          SizedBox(height: 30.h),
                          EnterEmailText(),
                          SizedBox(height: 35.h),
                          SizedBox(height: 25.h),
                          Textfild(
                            email,
                            email_F,
                            'Email',
                            Icons.email,
                            // validator: (value) {
                            //   if (value == null || value.isEmpty) return 'Email is required';
                            //   final emailRegex = RegExp(r'^[\w\.-]+@[\w\.-]+\.\w+$');
                            //   if (!emailRegex.hasMatch(value)) return 'Invalid email format';
                            //   if (widget.emailText != null && value != widget.emailText) {
                            //     return 'Email does not match';
                            //   }
                            //   return null;
                            // },
                          ),
                          SizedBox(height: 20.h),
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

  Widget EnterEmailText() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 40.w),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        children: [
          Text(
            "Enter your email, phone, or username",
            textAlign: TextAlign.center,
            style: TextStyle(fontSize: 14.sp, color: Colors.white),
          ),
          SizedBox(height: 5.h),
          Text(
            "and we'll send you a link to create new password",
            textAlign: TextAlign.center,
            style: TextStyle(fontSize: 14.sp, color: Colors.white),
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
              Navigator.pushReplacement(
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
                fontWeight: FontWeight.bold,
              ),
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
                .add(ForgotPaswordBtnClicked(email: email.text));
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
