import 'dart:io';
import 'package:flutter/material.dart';


class Post {
  final File image;
  final String caption;
  final String username;
  final File? userImage;

  Post({
    required this.image,
    required this.caption,
    required this.username,
    this.userImage,
  });

  ImageProvider get profileImage {
    return userImage != null
        ? FileImage(userImage!)
        : const AssetImage('assets/images/default_profile_picture.jpg') as ImageProvider;
  }
}
