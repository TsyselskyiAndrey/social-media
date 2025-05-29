import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

Future<void> showErrorDialog(
  BuildContext context,
  List<String> messages,
) async {
  return showDialog(
    context: context,
    builder: (BuildContext context) {
      return AlertDialog(
        title: Center(
            child: Text(
          'Error',
          style: TextStyle(fontSize: 25.sp),
        )),
        content: SingleChildScrollView(
          child: ListBody(
            children: messages
                .map(
                  (message) => Text(
                    message,
                    style: TextStyle(fontSize: 20.sp),
                  ),
                )
                .toList(),
          ),
        ),
      );
    },
  );
}
