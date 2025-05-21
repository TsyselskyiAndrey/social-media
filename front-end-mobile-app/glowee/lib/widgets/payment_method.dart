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
    return Material(
      color: Colors.white,
      child: InkWell(
        onTap: () {
          showDialog(
            context: context,
            builder: (BuildContext context) => _buildDialog(context),
          );
        },
        child: Padding(
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
        ),
      ),
    );
  }

  Widget _buildDialog(BuildContext context) {
    return Dialog(
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(12.r),
      ),
      child: Container(
        padding: EdgeInsets.all(20.w),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Center(
              child: Text(
                'Pay?',
                style: TextStyle(fontSize: 25.sp),
              ),
            ),
            Padding(
              padding: EdgeInsets.symmetric(
                vertical: 10.h,
                horizontal: 10.w,
              ),
              child: Text(
                "Apple ID will be used for your purchase",
                style: TextStyle(fontSize: 20.sp),
                textAlign: TextAlign.center,
              ),
            ),
            SizedBox(height: 20.h),
            Table(
              border: const TableBorder(
                top: BorderSide(
                  width: 2,
                  color: Color.fromRGBO(224, 224, 224, 1),
                ),
                verticalInside: BorderSide(
                  width: 2,
                  color: Color.fromRGBO(224, 224, 224, 1),
                ),
              ),
              children: [
                TableRow(
                  children: [
                    InkWell(
                      onTap: () => Navigator.of(context).pop(),
                      child: Center(
                        child: Container(
                          height: 50.h,
                          alignment: Alignment.center,
                          child: Text(
                            'Cancel',
                            style: TextStyle(
                              color: Colors.blue,
                              fontSize: 20.sp,
                            ),
                          ),
                        ),
                      ),
                    ),
                    Container(
                      alignment: Alignment.center,
                      height: 50.h,
                      child: InkWell(
                        onTap: () {
                          Navigator.of(context).pop();
                        },
                        child: Center(
                          child: Text(
                            'Pay',
                            style: TextStyle(
                              color: Colors.blue,
                              fontSize: 20.sp,
                            ),
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}
