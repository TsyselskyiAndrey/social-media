import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

class LoginScreen extends StatefulWidget {
  final VoidCallback show;
  const LoginScreen(this.show, {super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final TextEditingController email = TextEditingController();
  final FocusNode emailFocus = FocusNode();
  final TextEditingController password = TextEditingController();
  final FocusNode passwordFocus = FocusNode();

  @override
  void dispose() {
    email.dispose();
    password.dispose();
    emailFocus.dispose();
    passwordFocus.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
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
              _buildTextField(email, emailFocus, 'Username', Icons.email),
              SizedBox(height: 15.h),
              _buildTextField(password, passwordFocus, 'Password', Icons.lock),
              SizedBox(height: 15.h),
              _buildForgotPassword(),
              SizedBox(height: 15.h),
              _buildLoginButton(),
              SizedBox(height: 150.h),
              _buildSignUpPrompt(),

            ],
          ),
        ),
      ),
    );
  }

  Widget _buildForgotPassword() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 30.w),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.end,
        children: [
          Text(
            "Forgot password? Don’t worry   ",
            style: TextStyle(fontSize: 14.sp, color: Colors.white),
          ),
          GestureDetector(
            onTap: widget.show,
            child: Text(
              "reset it here!",
              style: TextStyle(
                  fontSize: 15.sp,
                  color: Colors.black,
                  fontWeight: FontWeight.bold),
            ),
          ),
        ],
      ),
    );
  }




  Widget _buildSignUpPrompt() {
    return Align(
      alignment: Alignment.bottomCenter, // Выравнивание внизу
      child: Container(
        constraints: BoxConstraints(maxWidth: 600),  // Ограничиваем максимальную ширину
        width: double.infinity,  // Задаем ширину на весь экран
        padding: EdgeInsets.symmetric(vertical: 20, horizontal: 20),
        decoration: BoxDecoration(
          color: Colors.white.withOpacity(0.2), // полупрозрачный белый
          borderRadius: BorderRadius.circular(0.r),

        ),
        child: Padding(
          padding: EdgeInsets.symmetric(horizontal: 20.w),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center, // Выравнивание текста по центру
            children: [
              Text(
                "Don’t have an Account ? ",
                style: TextStyle(fontSize: 14.sp, color: Colors.white),
              ),
              GestureDetector(
                onTap: widget.show,
                child: Text(
                  "Sign up",
                  style: TextStyle(
                      fontSize: 15.sp,
                      color: Colors.black,
                      fontWeight: FontWeight.bold),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }






  Widget _buildLoginButton() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 10.w),
      child: InkWell(
        onTap: () {
          print("Email: ${email.text}");
          print("Password: ${password.text}");
        },
        child: Container(
          alignment: Alignment.center,
          width: double.infinity,
          height: 44.h,
          decoration: BoxDecoration(
            color: Colors.white.withOpacity(0.2), // полупрозрачный белый
            borderRadius: BorderRadius.circular(10.r),
            border: Border.all(
              color: Colors.white,
              width: 2.w
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
              color:  Colors.white,
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
