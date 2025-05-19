import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:flutter_svg/flutter_svg.dart';

class PlanCard extends StatelessWidget {
  final String title;
  final String price;
  final List<String> features;
  final bool isPopular;

  const PlanCard({
    required this.title,
    required this.price,
    required this.features,
    this.isPopular = false,
    super.key,
  });

  Widget get checkSvg => SvgPicture.asset(
        'assets/svg/check.svg',
        width: 20.w,
        height: 20.h,
      );

  Widget get starsImg => Image.asset(
        'assets/images/stars.png',
        width: 30.w,
        height: 30.h,
      );

  @override
  Widget build(BuildContext context) {
    return Container(
      height: (MediaQuery.sizeOf(context).height * 0.35).h,
      margin: EdgeInsets.all(20.w),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(15.r),
      ),
      child: LayoutBuilder(
        builder: (context, constraints) {
          return Column(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: [
              Stack(
                children: [
                  Align(
                    alignment: Alignment.bottomCenter,
                    child: Row(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        starsImg,
                        SizedBox(width: 7.w),
                        Text(
                          title,
                          style: TextStyle(
                            fontSize: 30.sp,
                            fontWeight: FontWeight.w500,
                          ),
                        ),
                        SizedBox(width: 7.w),
                        if (title == 'Elite Plan') starsImg,
                      ],
                    ),
                  ),
                  if (isPopular)
                    Padding(
                      padding: EdgeInsets.only(right: 8.w),
                      child: Align(
                        alignment: Alignment.topRight,
                        child: Container(
                          width: (constraints.maxWidth * 0.25).w,
                          height: (constraints.minHeight * 0.09).h,
                          decoration: BoxDecoration(
                            color: Colors.black,
                            borderRadius: BorderRadius.circular(12.r),
                          ),
                          child: Center(
                            child: Text(
                              "Popular",
                              style: TextStyle(
                                fontSize: 15.sp,
                                color: Colors.white,
                              ),
                            ),
                          ),
                        ),
                      ),
                    ),
                ],
              ),
              Row(
                children: [
                  Padding(
                    padding: EdgeInsets.only(left: 28.w),
                    child: Text(
                      "\$",
                      style: TextStyle(
                        fontSize: 30.sp,
                        fontWeight: FontWeight.w500,
                      ),
                    ),
                  ),
                  Text(
                    price,
                    style: TextStyle(
                      fontSize: 30.sp,
                      fontWeight: FontWeight.w500,
                    ),
                  ),
                  Text(
                    " / month",
                    style: TextStyle(
                      fontSize: 23.sp,
                      fontWeight: FontWeight.w400,
                    ),
                  ),
                ],
              ),
              SizedBox(
                width: (constraints.maxWidth * 0.8).w,
                height: (constraints.maxHeight * 0.15).h,
                child: TextButton(
                  onPressed: () {},
                  style: TextButton.styleFrom(
                    backgroundColor: Colors.black,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(12.r),
                    ),
                  ),
                  child: Text(
                    "Get Started",
                    style: TextStyle(color: Colors.white, fontSize: 18.sp),
                  ),
                ),
              ),
              Column(
                children: features.map((feature) {
                  return Padding(
                    padding: EdgeInsets.only(left: 30.w, bottom: 8.h),
                    child: Row(
                      children: [
                        checkSvg,
                        SizedBox(width: 10.w),
                        Expanded(
                          child: Text(
                            feature,
                            style: TextStyle(
                              color: Colors.black,
                              fontSize: 20.sp,
                              fontWeight: FontWeight.w500,
                            ),
                          ),
                        ),
                      ],
                    ),
                  );
                }).toList(),
              ),
            ],
          );
        },
      ),
    );
  }
}
