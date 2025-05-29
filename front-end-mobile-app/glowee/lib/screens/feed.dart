import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:glowee/widgets/navigation_menu.dart';
import 'package:glowee/widgets/post.dart';

class FeedScreen extends StatefulWidget {
  const FeedScreen({super.key});

  @override
  State<FeedScreen> createState() => _FeedScreenState();
}

class _FeedScreenState extends State<FeedScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      bottomNavigationBar: const NavigationMenu(),
      appBar: AppBar(
        elevation: 0,
        title: SizedBox(
          width: 105.w,
          height: 30.h,
          child: Image.asset('assets/images/logo.png'),
        ),
        centerTitle: true,
        backgroundColor: Color(0xffFAFAFA),
      ),
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
            child: CustomScrollView(
              slivers: [
                SliverList(
                  delegate: SliverChildBuilderDelegate(
                    (context, index) {
                      return Post();
                    },
                    childCount: 5,
                  ),
                )
              ],
            ),
          ),
        ],
      ),
    );
  }
}
