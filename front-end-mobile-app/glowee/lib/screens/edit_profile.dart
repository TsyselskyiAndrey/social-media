import 'dart:io';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:glowee/bloc/user_bloc/user_bloc.dart';
import 'package:glowee/bloc/user_bloc/user_events.dart';
import 'package:glowee/model/user.dart';
import 'package:glowee/screens/profile.dart';
import 'package:image_picker/image_picker.dart';
import 'package:glowee/screens/profile_image_notifier.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:intl/intl.dart';
import '../bloc/user_bloc/user_states.dart';

class EditProfileScreen extends StatefulWidget {
  final User user;

  const EditProfileScreen({super.key, required this.user});

  @override
  State<EditProfileScreen> createState() => _EditProfileScreenState();
}

class _EditProfileScreenState extends State<EditProfileScreen> {
  File? _imageFile;
  late final TextEditingController _firstNameController;
  late final TextEditingController _lastNameController;
  late final TextEditingController _usernameController;
  late final TextEditingController _bioController;
  late final TextEditingController _birthdayController;

  @override
  void initState() {
    super.initState();
    _firstNameController = TextEditingController(text: widget.user.firstName);
    _lastNameController = TextEditingController(text: widget.user.lastName);
    _usernameController = TextEditingController(text: widget.user.userName);
    _bioController = TextEditingController(text: widget.user.biography);
    _birthdayController = TextEditingController(
        text: widget.user.birthDate != null
            ? DateFormat('yyyy-MM-dd').format(widget.user.birthDate!)
            : '');
  }

  @override
  void dispose() {
    _firstNameController.dispose();
    _lastNameController.dispose();
    _usernameController.dispose();
    _bioController.dispose();
    _birthdayController.dispose();
    super.dispose();
  }

  Future<void> _pickImage() async {
    final pickedFile =
        await ImagePicker().pickImage(source: ImageSource.gallery);
    if (pickedFile != null) {
      final file = File(pickedFile.path);
      setState(() {
        _imageFile = file;
      });
      profileImageNotifier.value = file;
    }
  }

  @override
  Widget build(BuildContext context) {
    return BlocConsumer<UserBloc, UserState>(
      listener: (context, state) {
        if (state is UserStepSuccess) {
          Navigator.pushReplacement(
            context,
            MaterialPageRoute(
              builder: (context) => BlocProvider(
                create: (context) => UserBloc(),
                child: ProfileScreen(),
              ),
            ),
          );
        }
      },
      builder: (context, state) {
        return Scaffold(
          appBar: AppBar(
            leading: TextButton(
              onPressed: () => Navigator.pop(context),
              child: const Text(
                'Cancel',
                style: TextStyle(
                  color: Colors.black,
                  fontSize: 16,
                ),
              ),
            ),
            title: const Text(
              'Edit Profile',
              style: TextStyle(
                fontWeight: FontWeight.bold,
                fontSize: 18,
              ),
            ),
            centerTitle: true,
            actions: [
              TextButton(
                onPressed: () {
                  final firstName = _firstNameController.text.trim();
                  final lastName = _lastNameController.text.trim();
                  final username = _usernameController.text.trim();
                  final bio = _bioController.text.trim();
                  final birthday = _birthdayController.text.trim();
                  final profilePhoto = _imageFile;
                  context.read<UserBloc>().add(
                        UpdateUserProfileBtnClicked(
                          firstName: firstName,
                          lastName: lastName,
                          username: username,
                          Biography: bio,
                          Birthday: birthday,
                          ProfilePhoto: profilePhoto,
                        ),
                      );
                },
                child: const Text(
                  'Save',
                  style: TextStyle(
                    color: Colors.white,
                    fontSize: 16,
                  ),
                ),
              ),
            ],
          ),
          body: SingleChildScrollView(
            child: Padding(
              padding: EdgeInsets.all(16.0.w),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.center,
                children: [
                  SizedBox(height: 20.h),
                  Center(
                    child: InkWell(
                      onTap: _pickImage,
                      child: CircleAvatar(
                        radius: 60.r,
                        backgroundColor: Colors.grey.shade300,
                        backgroundImage: profileImageNotifier.value != null
                            ? FileImage(profileImageNotifier.value!)
                            : const AssetImage('assets/images/stars.png')
                                as ImageProvider,
                      ),
                    ),
                  ),
                  SizedBox(height: 10.h),
                  GestureDetector(
                    onTap: _pickImage,
                    child: const Text(
                      'Change Profile Photo',
                      style: TextStyle(
                        fontWeight: FontWeight.bold,
                        fontSize: 16,
                        color: Colors.blue,
                      ),
                    ),
                  ),
                  SizedBox(height: 30.h),
                  EditableProfileInfoRow(
                      title: 'First Name', controller: _firstNameController),
                  const Divider(),
                  EditableProfileInfoRow(
                      title: 'Last Name', controller: _lastNameController),
                  const Divider(),
                  EditableProfileInfoRow(
                      title: 'Username', controller: _usernameController),
                  const Divider(),
                  EditableProfileInfoRow(
                      title: 'Bio', controller: _bioController),
                  const Divider(),
                  EditableProfileInfoRow(
                      title: 'Birthdate', controller: _birthdayController),
                  SizedBox(height: 10.h),
                ],
              ),
            ),
          ),
        );
      },
    );
  }
}

class EditableProfileInfoRow extends StatelessWidget {
  final String title;
  final TextEditingController controller;

  const EditableProfileInfoRow({
    super.key,
    required this.title,
    required this.controller,
  });

  @override
  Widget build(BuildContext context) {
    final isBirthdate = title.toLowerCase() == 'birthdate';

    return Padding(
      padding: EdgeInsets.symmetric(vertical: 12.h),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Text(
            title,
            style: const TextStyle(
              fontWeight: FontWeight.bold,
              fontSize: 16,
            ),
          ),
          isBirthdate
              ? GestureDetector(
                  onTap: () async {
                    DateTime? pickedDate = await showDatePicker(
                      context: context,
                      initialDate:
                          DateTime.tryParse(controller.text) ?? DateTime(2000),
                      firstDate: DateTime(1900),
                      lastDate: DateTime.now(),
                    );
                    if (pickedDate != null) {
                      controller.text = pickedDate
                          .toLocal()
                          .toIso8601String()
                          .split('T')
                          .first;
                    }
                  },
                  child: Container(
                    alignment: Alignment.centerRight,
                    width: 200.w,
                    padding: const EdgeInsets.symmetric(vertical: 8.0),
                    child: Text(
                      controller.text.isNotEmpty
                          ? controller.text
                          : 'Select date',
                      style:
                          const TextStyle(fontSize: 16, color: Colors.black87),
                      textAlign: TextAlign.end,
                    ),
                  ),
                )
              : SizedBox(
                  width: 200.w,
                  child: TextField(
                    controller: controller,
                    decoration: const InputDecoration(
                      border: InputBorder.none,
                      isDense: true,
                    ),
                    style: const TextStyle(fontSize: 16),
                    textAlign: TextAlign.end,
                  ),
                ),
        ],
      ),
    );
  }
}
