import 'dart:io';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:glowee/bloc/post_bloc/post_bloc.dart';
import 'package:image_picker/image_picker.dart';

import 'package:glowee/screens/addpost_text.dart';

class AddPostScreen extends StatefulWidget {
  const AddPostScreen({super.key});

  @override
  State<AddPostScreen> createState() => _AddPostScreenState();
}

class _AddPostScreenState extends State<AddPostScreen> {
  final List<File> _images = [];
  File? _selectedFile;
  final ImagePicker _picker = ImagePicker();
  int _currentPage = 0;

  Future<void> _pickImage() async {
    final XFile? pickedFile =
        await _picker.pickImage(source: ImageSource.gallery);
    if (pickedFile != null) {
      setState(() {
        final file = File(pickedFile.path);
        _images.add(file);
        _selectedFile = file;
        _currentPage = _images.length - 1;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      resizeToAvoidBottomInset: false,
      appBar: AppBar(
        backgroundColor: Colors.white,
        elevation: 0,
        title: const Text('New Post', style: TextStyle(color: Colors.black)),
        centerTitle: false,
        actions: [
          Center(
            child: Padding(
              padding: EdgeInsets.symmetric(horizontal: 10.w),
              child: GestureDetector(
                onTap: () {
                  if (_selectedFile != null) {
                    Navigator.of(context).pushReplacement(
                      MaterialPageRoute(
                        builder: (context) => BlocProvider(
                          create: (context) => PostBloc(),
                          child: AddPostTextScreen(_selectedFile!),
                        ),
                      ),
                    );
                  }
                },
                child: Text(
                  'Next',
                  style: TextStyle(fontSize: 15.sp, color: Colors.blue),
                ),
              ),
            ),
          ),
        ],
      ),
      body: SafeArea(
        child: Column(
          children: [
            SizedBox(height: 10.h),
            ElevatedButton(
              onPressed: _pickImage,
              child: const Text('Choose photo from gallery'),
            ),
            SizedBox(height: 10.h),
            if (_selectedFile != null)
              Container(
                height: 375.h,
                width: double.infinity,
                child: Image.file(_selectedFile!, fit: BoxFit.cover),
              ),
            SizedBox(height: 10.h),
            Container(
              width: double.infinity,
              height: 40.h,
              color: Colors.white,
              padding: EdgeInsets.symmetric(horizontal: 10.w),
              alignment: Alignment.centerLeft,
              child: Text(
                'Selected Photos',
                style: TextStyle(fontSize: 15.sp, fontWeight: FontWeight.w600),
              ),
            ),
            Expanded(
              child: GridView.builder(
                padding: EdgeInsets.all(10.w),
                itemCount: _images.length,
                gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
                  crossAxisCount: 3,
                  mainAxisSpacing: 5,
                  crossAxisSpacing: 5,
                ),
                itemBuilder: (context, index) {
                  final file = _images[index];
                  return GestureDetector(
                    onTap: () {
                      setState(() {
                        _selectedFile = file;
                        _currentPage = index;
                      });
                    },
                    child: Stack(
                      children: [
                        Image.file(file, fit: BoxFit.cover),
                        if (index == _currentPage)
                          Container(
                            color: Colors.black.withOpacity(0.3),
                            child: const Center(
                              child: Icon(Icons.check, color: Colors.white),
                            ),
                          ),
                      ],
                    ),
                  );
                },
              ),
            ),
          ],
        ),
      ),
    );
  }
}
