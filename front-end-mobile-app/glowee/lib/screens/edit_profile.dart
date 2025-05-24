import 'dart:io';
import 'package:flutter/material.dart';
import 'package:image_picker/image_picker.dart';
import 'package:glowee/screens/profile_image_notifier.dart';
import 'package:glowee/data/post_data.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';


class EditProfileScreen extends StatefulWidget {
  const EditProfileScreen({super.key});

  @override
  State<EditProfileScreen> createState() => _EditProfileScreenState();
}

class _EditProfileScreenState extends State<EditProfileScreen> {
  File? _imageFile;
  late final TextEditingController _nameController;
  late final TextEditingController _usernameController;
  late final TextEditingController _bioController;
  late final TextEditingController _birthdayController;

  @override
  void initState() {
    super.initState();
    _nameController = TextEditingController(text: nameNotifier.value);
    _usernameController = TextEditingController(text: usernameNotifier.value);
    _bioController = TextEditingController(text: bioNotifier.value);
    _birthdayController=TextEditingController(text: birthdayNotifier.value);
  }

  @override
  void dispose() {
    _nameController.dispose();
    _usernameController.dispose();
    _bioController.dispose();
    _birthdayController.dispose();
    super.dispose();
  }


  Future<void> _pickImage() async {
    final pickedFile = await ImagePicker().pickImage(source: ImageSource.gallery);
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
              nameNotifier.value = _nameController.text;
              usernameNotifier.value = _usernameController.text;
              bioNotifier.value = _bioController.text;
              Navigator.pop(context);
            },
            child: const Text(
              'Save',
              style: TextStyle(
                color: Colors.blue,
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
                        : const AssetImage('assets/images/stars.png') as ImageProvider,
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
              EditableProfileInfoRow(title: 'Name', controller: _nameController),
              const Divider(),
              EditableProfileInfoRow(title: 'Username', controller: _usernameController),
              const Divider(),
              EditableProfileInfoRow(title: 'Bio', controller: _bioController),
              const Divider(),
              EditableProfileInfoRow(title: 'Birthdate', controller: _birthdayController),
              SizedBox(height: 10.h),

            ],
          ),
        ),
      ),
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
                initialDate: DateTime.tryParse(controller.text) ?? DateTime(2000),
                firstDate: DateTime(1900),
                lastDate: DateTime.now(),
              );
              if (pickedDate != null) {
                controller.text = pickedDate.toLocal().toIso8601String().split('T').first;
                birthdayNotifier.value = controller.text;
              }
            },
            child: Container(
              alignment: Alignment.centerRight,
              width: 200.w,
              padding: const EdgeInsets.symmetric(vertical: 8.0),
              child: Text(
                controller.text.isNotEmpty ? controller.text : 'Select date',
                style: const TextStyle(fontSize: 16, color: Colors.black87),
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

