import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:flutter_svg/flutter_svg.dart';

class PaymentMethod extends StatelessWidget {
  final String paymentMethod;
  final String cardNumber;
  final String svgName;

  const PaymentMethod({
    super.key,
    required this.paymentMethod,
    required this.cardNumber,
    required this.svgName,
  });

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: EdgeInsets.all(16.w),
      child: Row(
        children: [
          SvgPicture.asset(
            svgName,
            width: 45.w,
            height: 45.h,
          ),
          SizedBox(width: 16.w),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              mainAxisSize: MainAxisSize.min,
              children: [
                Text(
                  paymentMethod,
                  style: TextStyle(
                    fontSize: 20.sp,
                    fontWeight: FontWeight.normal,
                    color: Colors.black,
                  ),
                ),
                SizedBox(height: 4.h),
                Text(
                  cardNumber,
                  style: TextStyle(
                    fontSize: 18.sp,
                    color: Colors.blue,
                    fontWeight: FontWeight.w500,
                  ),
                ),
              ],
            ),
          ),
          Icon(
            Icons.arrow_forward_ios,
            size: 20.r,
            color: Colors.black,
          ),
        ],
      ),
    );
  }
}
