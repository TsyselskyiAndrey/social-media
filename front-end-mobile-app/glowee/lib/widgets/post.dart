import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

class Post extends StatelessWidget {
  const Post({super.key});

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Container(
          width: double.infinity,
          height: 55.h,
          color: Colors.white,
          child: Center(
            child: ListTile(
              leading: ClipOval(
                child: SizedBox(
                  width: 45.w,
                  height: 45.h,
                  child: Image.asset('assets/images/logo.png'),
                ),
              ),
              title: Text(
                'username',
                style: TextStyle(
                  fontSize: 15.sp,
                ),
              ),
            ),
          ),
        ),
        Container(
          width: double.infinity,
          height: 375.h,
          child: Image.asset(
            'assets/images/logo.png',
            fit: BoxFit.cover,
          ),
        ),
        Container(
          width: double.infinity,
          color: Colors.white,
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              SizedBox(height: 14.w),
              Row(
                children: [
                  SizedBox(width: 14.w),
                  Icon(
                    Icons.favorite_outline,
                    size: 30.w,
                  ),
                  SizedBox(width: 17.w),
                  Icon(
                    Icons.chat_bubble_outline,
                    size: 30.w,
                  ),
                  Spacer(),
                  Padding(
                    padding: EdgeInsets.only(right: 15.w),
                    child: Icon(
                      Icons.bookmark_outline,
                      size: 30.w,
                    ),
                  ),
                ],
              ),
              Padding(
                padding: EdgeInsets.only(
                  left: 30.w,
                  top: 4.h,
                  bottom: 8.h,
                ),
                child: Text(
                  '0',
                  style: TextStyle(
                    fontSize: 13.sp,
                    fontWeight: FontWeight.w500,
                  ),
                ),
              ),
              Padding(
                padding: EdgeInsets.symmetric(
                  horizontal: 15.w,
                ),
                child: Row(
                  children: [
                    Text(
                      'username' + '',
                      style: TextStyle(
                        fontSize: 13.sp,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    Text(
                      'caption',
                      style: TextStyle(
                        fontSize: 13.sp,
                      ),
                    ),
                  ],
                ),
              ),
              Padding(
                padding: EdgeInsets.only(
                  left: 15.w,
                  top: 20.h,
                  bottom: 8.h,
                ),
                child: Text(
                  'dateformat',
                  style: TextStyle(
                    fontSize: 11.sp,
                    color: Colors.grey,
                  ),
                ),
              ),
            ],
          ),
        ),
      ],
    );
  }
}
