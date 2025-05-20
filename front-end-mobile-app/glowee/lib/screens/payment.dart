import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:glowee/widgets/navigation_menu.dart';
import 'package:glowee/widgets/payment_method.dart';

class PaymentScreen extends StatefulWidget {
  const PaymentScreen({super.key});

  @override
  State<PaymentScreen> createState() => _PaymentScreenState();
}

class _PaymentScreenState extends State<PaymentScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      bottomNavigationBar: const NavigationMenu(),
      extendBody: true,
      body: Container(
        decoration: const BoxDecoration(
          color: Color.fromARGB(255, 255, 255, 255),
        ),
        child: SafeArea(
          child: Column(
            children: [
              Container(
                margin: EdgeInsets.all(25.w),
                alignment: Alignment.centerLeft,
                child: Row(
                  children: [
                    Icon(
                      Icons.arrow_back_ios,
                      size: 20.r,
                      color: Colors.black,
                    ),
                    Padding(
                      padding: EdgeInsets.only(left: 8.w),
                      child: Text(
                        "Payment",
                        style: TextStyle(
                          color: const Color.fromRGBO(0, 148, 255, 1),
                          fontSize: 22.sp,
                          fontWeight: FontWeight.w500,
                        ),
                      ),
                    ),
                  ],
                ),
              ),
              Table(
                border: TableBorder.all(
                  color: const Color.fromRGBO(153, 153, 153, 0.15),
                  width: 2,
                ),
                children: const [
                  TableRow(
                    children: [
                      PaymentMethod(
                        paymentMethod: "Apple ID",
                        cardNumber: "Balance: PKR2,6000",
                        svgName: 'assets/svg/apple.svg',
                      ),
                    ],
                  ),
                  TableRow(
                    children: [
                      PaymentMethod(
                        paymentMethod: "Master Card",
                        cardNumber: "****6356",
                        svgName: 'assets/svg/mastercard.svg',
                      ),
                    ],
                  ),
                  TableRow(
                    children: [
                      PaymentMethod(
                        paymentMethod: "Visa",
                        cardNumber: "****5645",
                        svgName: 'assets/svg/visa.svg',
                      ),
                    ],
                  ),
                ],
              ),
              Container(
                margin: EdgeInsets.all(25.w),
                alignment: Alignment.centerLeft,
                child: Text(
                  "Add Payment Method",
                  style: TextStyle(
                    color: const Color.fromRGBO(0, 148, 255, 1),
                    fontSize: 20.sp,
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
