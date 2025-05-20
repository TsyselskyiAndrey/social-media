import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:glowee/widgets/navigation_menu.dart';
import 'package:glowee/widgets/payment_text_field.dart';

class AddPaymentScreen extends StatefulWidget {
  const AddPaymentScreen({super.key});

  @override
  State<AddPaymentScreen> createState() => _AddPaymentScreenState();
}

class _AddPaymentScreenState extends State<AddPaymentScreen> {
  final _formKey = GlobalKey<FormState>();

  final _nameController = TextEditingController();
  final _cardNumberController = TextEditingController();
  final _expiryDateController = TextEditingController();
  final _securityCodeController = TextEditingController();
  final _zipController = TextEditingController();

  @override
  void dispose() {
    _nameController.dispose();
    _cardNumberController.dispose();
    _expiryDateController.dispose();
    _securityCodeController.dispose();
    _zipController.dispose();
    super.dispose();
  }

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
                margin: EdgeInsets.only(
                  left: 25.w,
                  right: 25.w,
                  top: 25.h,
                  bottom: 8.h,
                ),
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
                        "Add Payment",
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
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  SvgPicture.asset(
                    'assets/svg/visa.svg',
                    width: 90.w,
                    height: 90.h,
                  ),
                  SizedBox(width: 25.w),
                  SvgPicture.asset(
                    'assets/svg/mastercard.svg',
                    width: 90.w,
                    height: 90.h,
                  ),
                ],
              ),
              Expanded(
                child: SingleChildScrollView(
                  padding: EdgeInsets.symmetric(
                    horizontal: 25.w,
                    vertical: 25.h,
                  ),
                  child: Form(
                    key: _formKey,
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      mainAxisSize: MainAxisSize.max,
                      children: [
                        PaymentTextField(
                          controller: _nameController,
                          labelText: 'Name on Card',
                          hintText: 'Name',
                          validatorFunc: (value) {
                            if (value == null || value.isEmpty) {
                              return 'Please enter some text';
                            }
                            return null;
                          },
                        ),
                        SizedBox(height: 30.h),
                        PaymentTextField(
                          controller: _cardNumberController,
                          labelText: 'Card Number',
                          hintText: 'XXXX  XXXX  XXXX  XXXX',
                          validatorFunc: (value) {
                            if (value == null || value.isEmpty) {
                              return 'Please enter some text';
                            }
                            return null;
                          },
                        ),
                        SizedBox(height: 30.h),
                        PaymentTextField(
                          controller: _expiryDateController,
                          labelText: 'Expiry Date',
                          hintText: 'MM/YY',
                          validatorFunc: (value) {
                            if (value == null || value.isEmpty) {
                              return 'Please enter some text';
                            }
                            return null;
                          },
                        ),
                        SizedBox(height: 30.h),
                        PaymentTextField(
                          controller: _securityCodeController,
                          labelText: 'Security Code',
                          hintText: 'CVV',
                          validatorFunc: (value) {
                            if (value == null || value.isEmpty) {
                              return 'Please enter some text';
                            }
                            return null;
                          },
                        ),
                        SizedBox(height: 30.h),
                        PaymentTextField(
                          controller: _zipController,
                          labelText: 'ZIP/Postal Code',
                          hintText: 'XXXXX',
                          validatorFunc: (value) {
                            if (value == null || value.isEmpty) {
                              return 'Please enter some text';
                            }
                            return null;
                          },
                        ),
                        SizedBox(height: 30.h),
                        SizedBox(
                          width: 300.w,
                          height: 50.h,
                          child: ElevatedButton(
                            style: ElevatedButton.styleFrom(
                              backgroundColor:
                                  const Color.fromRGBO(0, 148, 255, 1),
                              foregroundColor: Colors.white,
                              textStyle: TextStyle(
                                fontSize: 23.sp,
                                fontWeight: FontWeight.w500,
                              ),
                            ),
                            onPressed: () {
                              if (_formKey.currentState!.validate()) {
                                _nameController.clear();
                                _cardNumberController.clear();
                                _expiryDateController.clear();
                                _securityCodeController.clear();
                                _zipController.clear();
                              }
                            },
                            child: const Text('Add'),
                          ),
                        ),
                      ],
                    ),
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
