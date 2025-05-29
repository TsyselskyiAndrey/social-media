import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:glowee/bloc/auth_bloc/auth_bloc.dart';
import 'package:glowee/bloc/auth_bloc/auth_events.dart';
import 'package:glowee/bloc/auth_bloc/auth_states.dart';
import 'package:glowee/screens/login_screen.dart';

import 'package:glowee/screens/otp_verification.dart';
import 'package:intl/intl.dart';
import 'dart:io';
import 'package:image_picker/image_picker.dart';

class RegisterDetails extends StatefulWidget {
  final String? emailText;
  const RegisterDetails({super.key, this.emailText});

  @override
  State<RegisterDetails> createState() => _RegisterDetailsState();
}

class _RegisterDetailsState extends State<RegisterDetails> {
  final TextEditingController _firstNameController = TextEditingController();
  final TextEditingController _lastNameController = TextEditingController();
  final TextEditingController _birthDateController = TextEditingController();

  final FocusNode _firstNameFocusNode = FocusNode();
  final FocusNode _lastNameFocusNode = FocusNode();
  final FocusNode _birthDateFocusNode = FocusNode();
  DateTime? _birthDate;
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  File? _imageFile;
  bool isPhotoSent = false;
  bool isInfoSent = false;

  @override
  void dispose() {
    _firstNameController.dispose();
    _lastNameController.dispose();
    _birthDateController.dispose();
    _firstNameFocusNode.dispose();
    _lastNameFocusNode.dispose();
    _birthDateFocusNode.dispose();
    super.dispose();
  }

  Future<void> _pickImage() async {
    final pickedFile = await ImagePicker().pickImage(source: ImageSource.gallery);
    if (pickedFile != null) {
      setState(() {
        _imageFile = File(pickedFile.path);
      });
      context.read<AuthBloc>().add(AddPhotoWhileSignUpBtnClicked(file: _imageFile!));
    }
  }

  Future<void> _selectDate(BuildContext context) async {
    final DateTime? picked = await showDatePicker(
      context: context,
      initialDate: DateTime.now(),
      firstDate: DateTime(1900),
      lastDate: DateTime.now(),
    );
    if (picked != null) {
      // Format the date as "yyyy-MM-dd" which is a common API-friendly format
      final formattedDate = DateFormat('MM/dd/yyyy').format(picked);
      setState(() {
        _birthDateController.text = formattedDate;
      });
    }
  }

  String? _validateFirstName(String? value) {
    if (value == null || value.isEmpty) {
      return 'First name is required';
    }
    if (value.length < 2) {
      return 'Too short';
    }
    return null;
  }

  String? _validateLastName(String? value) {
    if (value == null || value.isEmpty) {
      return 'Last name is required';
    }
    if (value.length < 2) {
      return 'Too short';
    }
    return null;
  }

  String? _validateBirthDate(String? value) {
    if (value == null || value.isEmpty) {
      return 'Birth date is required';
    }
    return null;
  }

  void _handleRegistration(BuildContext context) {
    if (_formKey.currentState!.validate()) {
      context.read<AuthBloc>().add(
        Register2BtnClicked(
          firstName: _firstNameController.text,
          lastName: _lastNameController.text,
          birthDate: _birthDateController.text, // Already a string
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return BlocListener<AuthBloc, AuthState>(
      listener: (context, state) {
        if (state is AuthStepSucess && isPhotoSent && state.flow == AuthFlow.RegisterStep2) {
          Navigator.pushReplacement(
            context,
            MaterialPageRoute(
              builder: (context) => BlocProvider(
                create: (context) => AuthBloc(),
                child: OtpVerificationScreen(),
              ),
            ),
          );
        } else if (state is AuthStepSucess && state.flow == AuthFlow.RegisterStep2) {
          isInfoSent = true;
        } else if (state is AuthStepSucess && state.flow == AuthFlow.UploadPhoto) {
          isPhotoSent = true;
        } else if (state is AuthError) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(content: Text(state.messages.join(', '))),
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
                Color.fromRGBO(27, 36, 136, 0.7),
                Color.fromRGBO(242, 188, 23, 0.5)
              ],
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
          ),
          child: SafeArea(
            child: Form(
              key: _formKey,
              child: SingleChildScrollView(
                padding: EdgeInsets.symmetric(horizontal: 20.w),
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
                        )
                            : CircleAvatar(
                          radius: 34.r,
                          backgroundImage: FileImage(_imageFile!),
                        ),
                      ),
                    ),
                    SizedBox(height: 25.h),

                    // First Name
                    TextFormField(
                      controller: _firstNameController,
                      focusNode: _firstNameFocusNode,
                      validator: (value) {
                        if (value == null || value.isEmpty) {
                          return 'Please enter your first name';
                        }
                        return null;
                      },
                      decoration: InputDecoration(
                        labelText: 'First Name',
                        prefixIcon: Icon(Icons.person),
                        filled: true,
                        fillColor: Colors.white,
                        border: OutlineInputBorder(),
                      ),
                    ),
                    SizedBox(height: 15.h),

                    // Last Name
                    TextFormField(
                      controller: _lastNameController,
                      focusNode: _lastNameFocusNode,
                      validator: (value) {
                        if (value == null || value.isEmpty) {
                          return 'Please enter your last name';
                        }
                        return null;
                      },
                      decoration: InputDecoration(
                        labelText: 'Last Name',
                        prefixIcon: Icon(Icons.person_outline),
                        filled: true,
                        fillColor: Colors.white,
                        border: OutlineInputBorder(),
                      ),
                    ),
                    SizedBox(height: 15.h),

                    // Birth Date
                    InkWell(
                      onTap: () async {
                        final pickedDate = await showDatePicker(
                          context: context,
                          initialDate: DateTime(2000),
                          firstDate: DateTime(1900),
                          lastDate: DateTime.now(),
                        );
                        if (pickedDate != null) {
                          setState(() {
                            _birthDate = pickedDate;
                          });
                        }
                      },
                      child: InputDecorator(
                        decoration: InputDecoration(
                          labelText: 'Birth Date',
                          prefixIcon: Icon(Icons.calendar_today),
                          filled: true,
                          fillColor: Colors.white,
                          border: OutlineInputBorder(),
                        ),
                        child: Text(
                          _birthDate != null
                              ? '${_birthDate!.month.toString().padLeft(2, '0')}/'
                              '${_birthDate!.day.toString().padLeft(2, '0')}/'
                              '${_birthDate!.year}'
                              : 'Select your birth date',
                          style: TextStyle(
                            fontSize: 16.sp,
                            color: _birthDate != null ? Colors.black : Colors.grey,
                          ),
                        ),
                      ),
                    ),
                    SizedBox(height: 25.h),

                    // Next Button
                    ElevatedButton(
                      onPressed: () {
                        if (_formKey.currentState!.validate() && _birthDate != null) {
                          context.read<AuthBloc>().add(Register2BtnClicked(
                            firstName: _firstNameController.text,
                            lastName: _lastNameController.text,
                            birthDate: '${_birthDate!.month.toString().padLeft(2, '0')}/'
                                '${_birthDate!.day.toString().padLeft(2, '0')}/'
                                '${_birthDate!.year}',
                          ));
                        } else if (_birthDate == null) {
                          ScaffoldMessenger.of(context).showSnackBar(
                            SnackBar(content: Text('Please select your birth date')),
                          );
                        }
                      },
                      child: Text('Next'),
                    ),
                    SizedBox(height: 15.h),

                    // Login Link
                    TextButton(
                      onPressed: () {
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
                      child: Text("Already have an account? Login"),
                    ),

                  ],
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }


  Widget _buildDateField(BuildContext context) {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 10.w),
      child: TextFormField(
        controller: _birthDateController,
        focusNode: _birthDateFocusNode,
        readOnly: true,
        onTap: () => _selectDate(context),
        validator: _validateBirthDate,
        decoration: InputDecoration(
          hintText: 'Birth Date (YYYY-MM-DD)',
          prefixIcon: Icon(Icons.calendar_today,
              color: _birthDateFocusNode.hasFocus ? Colors.black : Colors.grey[600]),
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
                fontWeight: FontWeight.bold,
              ),
            ),
          ),
        ],
      ),
    );
  }

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

  Widget _buildTextField({
    required TextEditingController controller,
    required FocusNode focusNode,
    required String label,
    required IconData icon,
    bool obscureText = false,
    bool enabled = true,
    required String? Function(String?)? validator,
  }) {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 10.w),
      child: TextFormField(
        controller: controller,
        focusNode: focusNode,
        obscureText: obscureText,
        enabled: enabled,
        validator: validator,
        style: TextStyle(fontSize: 18.sp, color: Colors.black),
        decoration: InputDecoration(
          hintText: label,
          prefixIcon: Icon(icon,
              color: focusNode.hasFocus ? Colors.black : Colors.grey[600]),
          contentPadding: EdgeInsets.symmetric(horizontal: 15.w, vertical: 15.h),
          filled: true,
          fillColor: enabled ? Colors.white : Colors.grey[200],
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