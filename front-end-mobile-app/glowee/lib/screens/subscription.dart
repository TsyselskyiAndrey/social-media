import 'package:flutter/material.dart';
import 'package:glowee/widgets/card.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';

class SubscriptionScreen extends StatelessWidget {
  const SubscriptionScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Stack(
        children: [
          Container(
            decoration: const BoxDecoration(
              gradient: LinearGradient(
                colors: [
                  Color.fromRGBO(242, 188, 23, 0.5),
                  Color.fromRGBO(17, 140, 140, 0.85),
                ],
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
              ),
            ),
          ),
          Container(
            decoration: const BoxDecoration(
              color: Color.fromRGBO(242, 188, 23, 0.2),
            ),
          ),
          SafeArea(
            child: Column(
              children: [
                Container(
                  padding: EdgeInsets.only(top: 35.h, left: 10.w, right: 10.w),
                  child: Stack(
                    children: [
                      Align(
                        alignment: Alignment.bottomLeft,
                        child: Text(
                          "Cancel",
                          style: TextStyle(
                            fontSize: 20.sp,
                          ),
                        ),
                      ),
                      Align(
                        alignment: Alignment.center,
                        child: Text(
                          "Subscription",
                          style: TextStyle(
                            fontSize: 25.sp,
                            fontWeight: FontWeight.w500,
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
                const Divider(
                  color: Colors.grey,
                  thickness: 1,
                ),
                const PlanCard(
                  title: 'Pro Plan',
                  price: '10',
                  features: [
                    'We are winners',
                    'Bondarev V. - our curator',
                    'Honestly we are tired',
                  ],
                ),
                const PlanCard(
                  title: 'Elite Plan',
                  price: '20',
                  features: [
                    'PZPI-23-4',
                    'OK!',
                    'We did it',
                  ],
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
