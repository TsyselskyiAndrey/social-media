import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

class PaymentTextField extends StatefulWidget {
  final String labelText;
  final String hintText;
  final TextEditingController controller;
  final String? Function(String?)? validatorFunc;

  const PaymentTextField({
    super.key,
    required this.labelText,
    required this.hintText,
    required this.controller,
    this.validatorFunc,
  });

  @override
  State<PaymentTextField> createState() => _PaymentTextFieldState();
}

class _PaymentTextFieldState extends State<PaymentTextField> {
  static const Color _textColor = Color.fromRGBO(69, 69, 69, 1);

  final TextStyle labelAndFloatingLabelStyle = TextStyle(
    fontSize: 20.sp,
    color: _textColor,
  );

  final OutlineInputBorder borderStyle = const OutlineInputBorder(
    borderSide: BorderSide(color: _textColor),
  );

  @override
  Widget build(BuildContext context) {
    return TextFormField(
      controller: widget.controller,
      decoration: InputDecoration(
        labelText: widget.labelText,
        hintText: widget.hintText,
        errorStyle: TextStyle(fontSize: 15.sp),
        hintStyle: const TextStyle(
          fontSize: 20,
          color: Color.fromRGBO(0, 148, 255, 0.45),
        ),
        floatingLabelStyle: labelAndFloatingLabelStyle,
        labelStyle: labelAndFloatingLabelStyle,
        enabledBorder: borderStyle,
        focusedBorder: borderStyle,
        errorBorder: borderStyle,
        focusedErrorBorder: borderStyle,
      ),
      validator: widget.validatorFunc,
    );
  }
}
