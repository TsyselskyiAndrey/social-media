import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:glowee/screens/login_screen.dart';
import 'dart:io';
import 'package:glowee/util/imagepicker.dart';
import 'package:image_picker/image_picker.dart';

class RegisterDetails extends StatefulWidget {
  const RegisterDetails({super.key});

  @override
  State<RegisterDetails> createState() => _RegisterDetailsState();
}

class _RegisterDetailsState extends State<RegisterDetails> {
  File? _imageFile;
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
  void dispose() {
    email.dispose();
    password.dispose();
    passwordConfirme.dispose();
    username.dispose();
    bio.dispose();
    super.dispose();
  }

  Future<void> _pickImage() async {
    final pickedFile = await ImagePicker().pickImage(source: ImageSource.gallery);
    if (pickedFile != null) {
      setState(() {
        _imageFile = File(pickedFile.path);
      });
    }
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
              Color.fromRGBO(27, 36, 136, 0.7019607843137254),
              Color.fromRGBO(242, 188, 23, 0.5)
            ],
            begin: Alignment.topLeft,
            end: Alignment.bottomRight,
          ),
        ),
        child: SafeArea(
          child: Form(
            key:_formKey,
            child: Column(
              children: [
                SizedBox(height: 20.h),
                Center(child: Image.asset('assets/images/logo.png')),
                SizedBox(height: 10.h),
                InkWell(
                  onTap: _pickImage,
                  child: CircleAvatar(
                    radius: 36.r,
                    backgroundColor: Colors.grey,
                    child: _imageFile == null
                        ? CircleAvatar(
                      radius: 34.r,
                      backgroundImage: AssetImage('assets/images/stars.png'),
                      backgroundColor: Colors.grey.shade200,
                    )
                        : CircleAvatar(
                      radius: 34.r,
                      backgroundImage: FileImage(_imageFile!),
                      backgroundColor: Colors.grey.shade200,
                    ),
                  ),
                ),
                createAccountText(),
                SizedBox(height: 25.h),
                Textfild(
                  email,
                  email_F,
                  'Email',
                  Icons.email,
                  validator: (value) {
                    if (value == null || value.isEmpty) return 'Email is required';
                    final emailRegex = RegExp(r'^[\w\.-]+@[\w\.-]+\.\w+$');
                    if (!emailRegex.hasMatch(value)) return 'Invalid email format';
                    return null;
                  },
                ),
                SizedBox(height: 5.h),
                Textfild(
                  username,
                  username_F,
                  'Username',
                  Icons.person,
                  validator: (value) {
                    if (value == null || value.isEmpty) return 'Username is required';
                    if (RegExp(r'[@?,\*^]').hasMatch(value)) {
                      return 'Username contains invalid characters';
                    }

                    return null;
                  },
                ),
                SizedBox(height: 5.h),
                Textfild(
                  password,
                  password_F,
                  'Password',
                  Icons.lock,
                  validator: (value) {
                    if (value == null || value.isEmpty) return 'Password is required';
                    if (value.length < 6) return 'Password must be at least 6 characters';
                    if (!RegExp(r'[A-Z]').hasMatch(value)) return 'Password must contain an uppercase letter';
                    if (!RegExp(r'[0-9]').hasMatch(value)) return 'Password must contain a number';
                    if (RegExp(r'[ @?,*^]').hasMatch(value)) return 'Password contains invalid characters';
                    return null;
                  },
                ),
                SizedBox(height: 5.h),
                Textfild(
                    passwordConfirme,
                    passwordConfirme_F,
                    'Confirm Password',
                    Icons.lock_outline,
                    validator: (value) {
                      if (value == null || value.isEmpty) return 'Please confirm your password';
                      if (value != password.text) return 'Passwords do not match';
                      return null;
                    }
                ),
                SizedBox(height: 20.h),
                Signup(),
                SizedBox(height: 15.h),
                Have(),
              ],
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
              Navigator.push(
                context,
                MaterialPageRoute(builder: (context) => const LoginScreen()),
              );
            },
            child: Text(
              "Login",
              style: TextStyle(fontSize: 15.sp, color: Colors.black, fontWeight: FontWeight.bold),
            ),
          ),
        ],
      ),
    );
  }

  Widget Signup() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 10.w),
      child: InkWell(
        onTap: () {
          if (_formKey.currentState!.validate()) {
            // Всё ок, можно продолжать
            print("Signup Email: ${email.text}");
          } else {
            print("Форма содержит ошибки");
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
          prefixIcon: Icon(icon, color: focusNode.hasFocus ? Colors.black : Colors.grey[600]),
          contentPadding: EdgeInsets.symmetric(horizontal: 15.w, vertical: 15.h),
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
