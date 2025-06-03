import 'dart:io';
import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
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

  List<String> _tags = [];
  String? _selectedTag;

  @override
  void initState() {
    super.initState();
    _loadTagsFromBackend();
  }

  Future<void> _loadTagsFromBackend() async {

    await Future.delayed(const Duration(seconds: 1));
    setState(() {
      _tags = ['Nature', 'Travel', 'Food', 'Art', 'Technology'];
      _selectedTag = _tags.first;
    });
  }

  Future<void> _pickImage() async {
    final XFile? pickedFile = await _picker.pickImage(source: ImageSource.gallery);
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
                    Navigator.of(context).push(
                      MaterialPageRoute(
                        builder: (context) => AddPostTextScreen(_selectedFile!),
                      ),
                    );
                  }
                },
                child: Text('Next', style: TextStyle(fontSize: 15.sp, color: Colors.blue)),
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

            if (_images.isNotEmpty)
              Stack(
                alignment: Alignment.topRight,
                children: [
                  SizedBox(
                    height: 375.h,
                    child: PageView.builder(
                      itemCount: _images.length,
                      onPageChanged: (index) {
                        setState(() {
                          _currentPage = index;
                          _selectedFile = _images[index];
                        });
                      },
                      itemBuilder: (context, index) {
                        return Image.file(
                          _images[index],
                          fit: BoxFit.cover,
                          width: double.infinity,
                        );
                      },
                    ),
                  ),
                  Container(
                    margin: EdgeInsets.all(12.w),
                    padding: EdgeInsets.symmetric(
                      horizontal: 10.w,
                      vertical: 5.h,
                    ),
                    decoration: BoxDecoration(
                      color: Colors.black.withOpacity(0.5),
                      borderRadius: BorderRadius.circular(20),
                    ),
                    child: Text(
                      '${_currentPage + 1}/${_images.length}',
                      style: TextStyle(
                        color: Colors.white,
                        fontSize: 14.sp,
                      ),
                    ),
                  ),
                ],
              ),

            SizedBox(height: 10.h),

            if (_tags.isNotEmpty)
              Padding(
                padding: EdgeInsets.symmetric(horizontal: 10.w),
                child: Row(
                  children: [
                    Text('Tag:', style: TextStyle(fontSize: 15.sp)),
                    SizedBox(width: 10.w),
                    DropdownButton<String>(
                      value: _selectedTag,
                      onChanged: (value) {
                        setState(() {
                          _selectedTag = value;
                        });
                      },
                      items: _tags
                          .map((tag) => DropdownMenuItem(
                        value: tag,
                        child: Text(tag),
                      ))
                          .toList(),
                    ),
                  ],
                ),
              ),

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