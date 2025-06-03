import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:glowee/bloc/auth_bloc/auth_bloc.dart';
import 'package:glowee/bloc/auth_bloc/auth_events.dart';
import 'package:glowee/bloc/auth_bloc/auth_states.dart';
import 'package:glowee/screens/login_screen.dart';

import 'package:glowee/screens/otp_verification.dart';
import 'package:glowee/util/error_dialog.dart';
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

  @override
  Widget build(BuildContext context) {
    return BlocConsumer<AuthBloc, AuthState>(
      listener: (context, state) async {
        if (state is AuthStepSucess && state.flow == AuthFlow.RegisterStep2) {
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
            state.flow == AuthFlow.UploadPhoto) {
          isPhotoSent = true;
        } else if (state is AuthError) {
          await showErrorDialog(context, state.messages);
        }
      },
      builder: (context, state) {
        return Scaffold(
          resizeToAvoidBottomInset: false,
          backgroundColor: Colors.white,
          body: Container(
            height: MediaQuery.of(context).size.height,
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
            child: (state is Authorizing ||
                    (state is AuthStepSucess &&
                        state.flow == AuthFlow.RegisterStep2))
                ? const Center(
                    child: CircularProgressIndicator(
                      color: Colors.white,
                    ),
                  )
                : SafeArea(
                    child: Form(
                      key: _formKey,
                      child: SingleChildScrollView(
                        padding: EdgeInsets.symmetric(horizontal: 20.w),
                        child: Column(
                          children: [
                            SizedBox(height: 20.h),
                            Center(
                                child: Image.asset('assets/images/logo.png')),
                            SizedBox(height: 10.h),
                            InkWell(
                              onTap: _pickImage,
                              child: CircleAvatar(
                                radius: 36.r,
                                backgroundColor: Colors.grey,
                                child: _imageFile == null
                                    ? CircleAvatar(
                                        radius: 34.r,
                                        backgroundImage: AssetImage(
                                            'assets/images/stars.png'),
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
                                    color: _birthDate != null
                                        ? Colors.black
                                        : Colors.grey,
                                  ),
                                ),
                              ),
                            ),
                            SizedBox(height: 25.h),

                            ElevatedButton(
                              onPressed: () async {
                                if (_formKey.currentState!.validate() &&
                                    _birthDate != null) {
                                  if (!isPhotoSent) {
                                    return await showErrorDialog(
                                      context,
                                      [
                                        "Information can't be send without a photo"
                                      ],
                                    );
                                  }
                                  context
                                      .read<AuthBloc>()
                                      .add(Register2BtnClicked(
                                        firstName: _firstNameController.text,
                                        lastName: _lastNameController.text,
                                        birthDate:
                                            '${_birthDate!.month.toString().padLeft(2, '0')}/'
                                            '${_birthDate!.day.toString().padLeft(2, '0')}/'
                                            '${_birthDate!.year}',
                                      ));
                                } else if (_birthDate == null) {
                                  ScaffoldMessenger.of(context).showSnackBar(
                                    SnackBar(
                                      content:
                                          Text('Please select your birth date'),
                                    ),
                                  );
                                }
                              },
                              child: Text('Next'),
                            ),
                            SizedBox(height: 15.h),

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
        );
      },
    );
  }
}
