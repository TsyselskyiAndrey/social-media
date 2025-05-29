import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:glowee/bloc/auth_bloc/auth_bloc.dart';
import 'package:glowee/bloc/auth_bloc/auth_events.dart';
import 'package:glowee/bloc/auth_bloc/auth_states.dart';
import 'package:glowee/screens/login_screen.dart';
import 'package:glowee/screens/otp_verification.dart';
import 'dart:io';
import 'package:image_picker/image_picker.dart';
import 'package:glowee/screens/email_enter.dart';

class RegisterDetails extends StatefulWidget {
  final String? emailText;
  const RegisterDetails({super.key, this.emailText});

  @override
  State<RegisterDetails> createState() => _RegisterDetailsState();
}

class _RegisterDetailsState extends State<RegisterDetails> {
  // Контролери для текстових полів
  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();
  final TextEditingController _confirmPasswordController =
      TextEditingController();
  final TextEditingController _usernameController = TextEditingController();
  final TextEditingController _bioController = TextEditingController();

  // Фокус ноди для керування фокусом полів вводу
  final FocusNode _emailFocusNode = FocusNode();
  final FocusNode _passwordFocusNode = FocusNode();
  final FocusNode _confirmPasswordFocusNode = FocusNode();
  final FocusNode _usernameFocusNode = FocusNode();
  final FocusNode _bioFocusNode = FocusNode();

  // Ключ для форми
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();

  // Змінна для збереження обраного зображення
  File? _imageFile;

  bool isPhotoSent = false;
  bool isInfoSent = false;

  @override
  void initState() {
    super.initState();
    // Якщо email переданий, встановлюємо його в контролер
    if (widget.emailText != null) {
      _emailController.text = widget.emailText!;
    }
  }

  @override
  void dispose() {
    // Обов'язково звільняємо ресурси
    _emailController.dispose();
    _passwordController.dispose();
    _confirmPasswordController.dispose();
    _usernameController.dispose();
    _bioController.dispose();
    _emailFocusNode.dispose();
    _passwordFocusNode.dispose();
    _confirmPasswordFocusNode.dispose();
    _usernameFocusNode.dispose();
    _bioFocusNode.dispose();
    super.dispose();
  }

  // Метод для вибору зображення з галереї
  Future<void> _pickImage() async {
    final pickedFile =
        await ImagePicker().pickImage(source: ImageSource.gallery);
    if (pickedFile != null) {
      setState(() {
        _imageFile = File(pickedFile.path);
      });
      context
          .read<AuthBloc>()
          .add(AddPhotoWhileSignUpBtnClicked(file: _imageFile!));
    }
  }

  // Метод для валідації email
  String? _validateEmail(String? value) {
    if (value == null || value.isEmpty) {
      return 'Email is required';
    }
    if (!RegExp(r'^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$').hasMatch(value)) {
      return 'Please enter a valid email';
    }
    return null;
  }

  // Метод для валідації пароля
  String? _validatePassword(String? value) {
    if (value == null || value.isEmpty) {
      return 'Password is required';
    }
    if (value.length < 6) {
      return 'Password must be at least 6 characters';
    }
    if (!RegExp(r'[A-Z]').hasMatch(value)) {
      return 'Password must contain an uppercase letter';
    }
    if (!RegExp(r'[0-9]').hasMatch(value)) {
      return 'Password must contain a number';
    }
    return null;
  }

  // Метод для підтвердження пароля
  String? _validateConfirmPassword(String? value) {
    if (value == null || value.isEmpty) {
      return 'Please confirm your password';
    }
    if (value != _passwordController.text) {
      return 'Passwords do not match';
    }
    return null;
  }

  // Метод для валідації імені користувача
  String? _validateUsername(String? value) {
    if (value == null || value.isEmpty) {
      return 'Username is required';
    }
    if (RegExp(r'[@?,\*^]').hasMatch(value)) {
      return 'Username contains invalid characters';
    }
    return null;
  }

  // Метод для обробки події реєстрації
  void _handleRegistration(BuildContext context) {
    if (_formKey.currentState!.validate()) {
      // Якщо форма валідна, продовжуємо
      context.read<AuthBloc>().add(
            Register2BtnClicked(
              firstName: _usernameController.text,
              lastName: _passwordController.text,
              birthDate: _confirmPasswordController.text,
            ),
          );
      // Navigator.push(
      //   context,
      //   MaterialPageRoute(
      //     builder: (context) => EmailEnter(emailText: _emailController.text),
      //   ),
      // );
    }
  }

  @override
  Widget build(BuildContext context) {
    return BlocListener<AuthBloc, AuthState>(
      listener: (context, state) {
        if (state is AuthStepSucess &&
            isPhotoSent &&
            state.flow == AuthFlow.RegisterStep2) {
          Navigator.pushReplacement(
            context,
            MaterialPageRoute(
              builder: (context) => BlocProvider(
                create: (context) => AuthBloc(),
                child: OtpVerificationScreen(),
              ),
            ),
          );
        } else if (state is AuthStepSucess &&
            state.flow == AuthFlow.RegisterStep2) {
          isInfoSent = true;
        } else if (state is AuthStepSucess &&
            state.flow == AuthFlow.UploadPhoto) {
          isPhotoSent = true;
        }
      },
      child: Scaffold(
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
          )),
          child: SafeArea(
            child: Form(
              key: _formKey,
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
                              backgroundImage:
                                  AssetImage('assets/images/stars.png'),
                              backgroundColor: Colors.grey.shade200,
                            )
                          : CircleAvatar(
                              radius: 34.r,
                              backgroundImage: FileImage(_imageFile!),
                              backgroundColor: Colors.grey.shade200,
                            ),
                    ),
                  ),
                  _createAccountText(),
                  SizedBox(height: 25.h),
                  SizedBox(height: 5.h),
                  _buildTextField(
                    controller: _usernameController,
                    focusNode: _usernameFocusNode,
                    label: 'Username',
                    icon: Icons.person,
                    validator: _validateUsername,
                  ),
                  SizedBox(height: 5.h),
                  _buildTextField(
                    controller: _emailController,
                    focusNode: _emailFocusNode,
                    label: 'Email',
                    icon: Icons.email,
                    validator: _validateEmail,
                  ),
                  SizedBox(height: 5.h),
                  _buildTextField(
                    controller: _passwordController,
                    focusNode: _passwordFocusNode,
                    label: 'Password',
                    icon: Icons.lock,
                    obscureText: true,
                    validator: _validatePassword,
                  ),
                  SizedBox(height: 5.h),
                  _buildTextField(
                    controller: _confirmPasswordController,
                    focusNode: _confirmPasswordFocusNode,
                    label: 'Confirm Password',
                    icon: Icons.lock_outline,
                    obscureText: true,
                    validator: _validateConfirmPassword,
                  ),
                  SizedBox(height: 20.h),
                  _buildNextButton(),
                  SizedBox(height: 15.h),
                  _buildLoginLink(),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }

  // Виджет для тексту "Create An Account"
  Widget _createAccountText() {
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

  // Виджет для посилання на сторінку входу
  Widget _buildLoginLink() {
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

  // Виджет для кнопки "Next"
  Widget _buildNextButton() {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 10.w),
      child: InkWell(
        onTap: () => _handleRegistration(context),
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

  // Загальний виджет для текстових полів
  Widget _buildTextField({
    required TextEditingController controller,
    required FocusNode focusNode,
    required String label,
    required IconData icon,
    bool obscureText = false,
    required String? Function(String?)? validator,
  }) {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 10.w),
      child: TextFormField(
        controller: controller,
        focusNode: focusNode,
        obscureText: obscureText,
        validator: validator,
        style: TextStyle(fontSize: 18.sp, color: Colors.black),
        decoration: InputDecoration(
          hintText: label,
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
