import 'dart:io';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:glowee/bloc/post_bloc/post_bloc.dart';
import 'package:glowee/bloc/post_bloc/post_events.dart';
import 'package:glowee/bloc/post_bloc/post_states.dart';
import 'package:glowee/bloc/user_bloc/user_bloc.dart';
import 'package:glowee/model/tag.dart';
import 'package:glowee/screens/profile.dart';

class AddPostTextScreen extends StatefulWidget {
  final File _file;
  const AddPostTextScreen(this._file, {super.key});

  @override
  State<AddPostTextScreen> createState() => _AddPostTextScreenState();
}

class _AddPostTextScreenState extends State<AddPostTextScreen> {
  final caption = TextEditingController();
  final location = TextEditingController();
  bool isLoading = false;
  List<Tag>? _tags;
  List<Tag> _selectedTags = [];

  @override
  void initState() {
    context.read<PostBloc>().add(LoadAllTags());
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return BlocConsumer<PostBloc, PostState>(
      listener: (context, state) {
        if (state is TagsLoaded) {
          _tags = state.tags;
        } else if (state is PostStepSuccess) {
          Navigator.of(context).pushReplacement(
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
            iconTheme: const IconThemeData(color: Colors.black),
            backgroundColor: Colors.white,
            elevation: 0,
            title:
                const Text('New post', style: TextStyle(color: Colors.black)),
            centerTitle: false,
            actions: [
              Center(
                child: Padding(
                  padding: EdgeInsets.symmetric(horizontal: 10.w),
                  child: GestureDetector(
                    onTap: () async {
                      context.read<PostBloc>().add(
                            CreatePostBtnClicked(
                              caption: caption.text,
                              postMedias: [
                                widget._file,
                              ],
                              tags:
                                  _selectedTags.map((tag) => tag.name).toList(),
                            ),
                          );
                    },
                    child: Text(
                      'Share',
                      style: TextStyle(color: Colors.blue, fontSize: 15.sp),
                    ),
                  ),
                ),
              ),
            ],
          ),
          body: SafeArea(
            child: isLoading
                ? Center(
                    child: CircularProgressIndicator(
                    color: Colors.black,
                  ))
                : Padding(
                    padding: EdgeInsets.only(top: 10.h),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Padding(
                          padding: EdgeInsets.symmetric(
                              horizontal: 10.w, vertical: 5.h),
                          child: Row(
                            children: [
                              Container(
                                width: 65.w,
                                height: 65.h,
                                decoration: BoxDecoration(
                                  color: Colors.amber,
                                  image: DecorationImage(
                                    image: FileImage(widget._file),
                                    fit: BoxFit.cover,
                                  ),
                                ),
                              ),
                              SizedBox(width: 10.w),
                              SizedBox(
                                width: 280.w,
                                height: 60.h,
                                child: TextField(
                                  controller: caption,
                                  decoration: const InputDecoration(
                                    hintText: 'Write a caption ...',
                                    border: InputBorder.none,
                                  ),
                                ),
                              ),
                            ],
                          ),
                        ),
                        const Divider(),
                        Padding(
                          padding: EdgeInsets.symmetric(horizontal: 10.w),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              const Text(
                                'Select tags:',
                                style: TextStyle(fontWeight: FontWeight.bold),
                              ),
                              _tags == null
                                  ? const CircularProgressIndicator()
                                  : ExpansionTile(
                                      title: Text(
                                        _selectedTags.isEmpty
                                            ? 'Tap to choose tags'
                                            : _selectedTags
                                                .map((e) => e.name)
                                                .join(', '),
                                        style: const TextStyle(fontSize: 14),
                                      ),
                                      children: _tags!.map(
                                        (tag) {
                                          final isSelected =
                                              _selectedTags.contains(tag);
                                          return CheckboxListTile(
                                            title: Text(tag.name),
                                            value: isSelected,
                                            onChanged: (bool? selected) {
                                              setState(
                                                () {
                                                  if (selected == true) {
                                                    _selectedTags.add(tag);
                                                  } else {
                                                    _selectedTags.remove(tag);
                                                  }
                                                },
                                              );
                                            },
                                          );
                                        },
                                      ).toList(),
                                    ),
                            ],
                          ),
                        ),
                      ],
                    ),
                  ),
          ),
        );
      },
    );
  }
}
