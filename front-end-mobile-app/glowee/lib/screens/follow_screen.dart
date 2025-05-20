
import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:glowee/widgets/navigation_menu.dart';

class FollowScreen extends StatefulWidget {
  final String uid;
  const FollowScreen({Key? key, required this.uid}) : super(key: key);

  @override
  State<FollowScreen> createState() => _FollowScreenState();
}

class _FollowScreenState extends State<FollowScreen> {
  bool yourse = false;
  bool follow = false;

  final int postLength = 5;
  final int followersCount = 123;
  final int followingCount = 87;
  final String username = 'mock_user';
  final String bio = 'Just a mock bio';
  final String profileImageUrl = 'https://via.placeholder.com/150';

  @override
  Widget build(BuildContext context) {
    return DefaultTabController(
      length: 3,
      child: Scaffold(
        backgroundColor: const Color.fromRGBO(27, 97, 103, 1),
        appBar: AppBar(
          title: const Text('Profile', style: TextStyle(color: Colors.white)),
          backgroundColor: const Color.fromRGBO(36, 54, 66, 1),
          elevation: 0,
          iconTheme: const IconThemeData(color: Colors.white),
          actions: [
            Builder(
              builder: (context) => IconButton(
                icon: const Icon(Icons.menu),
                onPressed: () {
                  Scaffold.of(context).openEndDrawer();
                },
              ),
            )
          ],
        ),
        endDrawer: Drawer(
          child: ListView(
            padding: EdgeInsets.zero,
            children: [
              DrawerHeader(
                decoration: const BoxDecoration(color: Colors.teal),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    CircleAvatar(
                      backgroundImage: NetworkImage(profileImageUrl),
                      radius: 30.r,
                    ),
                    SizedBox(height: 10.h),
                    Text(username,
                        style: TextStyle(color: Colors.white, fontSize: 16.sp)),
                    Text(bio,
                        style: TextStyle(color: Colors.white70, fontSize: 12.sp)),
                  ],
                ),
              ),
              ListTile(
                leading: const Icon(Icons.settings, color: Colors.black),
                title: const Text('Settings'),
                onTap: () {
                  Navigator.pop(context);
                  print('Settings tapped');
                },
              ),
              ListTile(
                leading: const Icon(Icons.notifications_active, color: Colors.black),
                title: const Text('Notifications'),
                onTap: () {
                  Navigator.pop(context);
                  print('Settings tapped');
                },
              ),
              ListTile(
                leading: const Icon(Icons.save, color: Colors.black),
                title: const Text('Saved'),
                onTap: () {
                  Navigator.pop(context);
                  print('Settings tapped');
                },
              ),
              ListTile(
                leading: const Icon(Icons.payment, color: Colors.black),
                title: const Text('Payment'),
                onTap: () {
                  Navigator.pop(context);
                  print('Settings tapped');
                },

              ),
              ListTile(
                leading: const Icon(Icons.account_box, color: Colors.black),
                title: const Text('Account'),
                onTap: () {
                  Navigator.pop(context);
                  print('Settings tapped');
                },
              ),
              ListTile(
                leading: const Icon(Icons.help, color: Colors.black),
                title: const Text('Help'),
                onTap: () {
                  Navigator.pop(context);
                  print('Settings tapped');
                },
              ),
              ListTile(
                leading: const Icon(Icons.sunny, color: Colors.black),
                title: const Text('Interface'),
                onTap: () {
                  Navigator.pop(context);
                  print('Settings tapped');
                },
              ),
              ListTile(
                leading: const Icon(Icons.logout, color: Colors.black),
                title: const Text('Logout'),
                onTap: () {
                  Navigator.pop(context);
                  print('Logout tapped');
                },
              ),
            ],
          ),
        ),
        body: SafeArea(
          child: Stack(
            children: [
              CustomScrollView(
                slivers: [
                  SliverToBoxAdapter(child: _buildHead()),
                  SliverGrid(
                    delegate: SliverChildBuilderDelegate(
                          (context, index) => GestureDetector(
                        onTap: () {},
                        child: Image.network(
                          'https://via.placeholder.com/150',
                          fit: BoxFit.cover,
                        ),
                      ),
                      childCount: postLength,
                    ),
                    gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
                      crossAxisCount: 3,
                      crossAxisSpacing: 4,
                      mainAxisSpacing: 4,
                    ),
                  ),
                ],
              ),
              Positioned(
                left: 0,
                right: 0,
                bottom: 0,
                child: Container(
                  padding: EdgeInsets.only(bottom: 10.h),
                  color: Colors.transparent,
                  child: const NavigationMenu(),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildHead() {
    return Container(
      color: const Color.fromRGBO(27, 97, 103, 1),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Padding(
                padding: EdgeInsets.symmetric(horizontal: 13.w, vertical: 10.h),
                child: ClipOval(
                  child: SizedBox(
                    width: 80.w,
                    height: 80.h,
                    child: Image.network(
                      profileImageUrl,
                      fit: BoxFit.cover,
                    ),
                  ),
                ),
              ),
              Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    children: [
                      SizedBox(width: 35.w),
                      Text('$postLength',
                          style: TextStyle(
                              color: Colors.white,
                              fontWeight: FontWeight.bold,
                              fontSize: 16.sp)),
                      SizedBox(width: 53.w),
                      Text('$followersCount',
                          style: TextStyle(
                              color: Colors.white,
                              fontWeight: FontWeight.bold,
                              fontSize: 16.sp)),
                      SizedBox(width: 70.w),
                      Text('$followingCount',
                          style: TextStyle(
                              color: Colors.white,
                              fontWeight: FontWeight.bold,
                              fontSize: 16.sp)),
                    ],
                  ),
                  Row(
                    children: [
                      SizedBox(width: 30.w),
                      Text('Posts',
                          style: TextStyle(color: Colors.white, fontSize: 13.sp)),
                      SizedBox(width: 25.w),
                      Text('Followers',
                          style: TextStyle(color: Colors.white, fontSize: 13.sp)),
                      SizedBox(width: 19.w),
                      Text('Following',
                          style: TextStyle(color: Colors.white, fontSize: 13.sp)),
                    ],
                  ),
                ],
              )
            ],
          ),
          Padding(
            padding: EdgeInsets.symmetric(horizontal: 15.w),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(username,
                    style: TextStyle(
                        color: Colors.white,
                        fontSize: 12.sp,
                        fontWeight: FontWeight.bold)),
                SizedBox(height: 5.h),
                Text(bio,
                    style: TextStyle(
                        color: Colors.white70,
                        fontSize: 12.sp,
                        fontWeight: FontWeight.w300)),
              ],
            ),
          ),
          SizedBox(height: 20.h),
          Padding(
            padding: EdgeInsets.symmetric(horizontal: 13.w),
            child: followButton(),
          ),
          SizedBox(height: 10.h),
        ],
      ),
    );
  }

  Widget followButton() {
    if (yourse) {
      return Container(
        alignment: Alignment.center,
        height: 30.h,
        width: double.infinity,
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(5.r),
          border: Border.all(color: Colors.grey.shade400),
        ),
        child: const Text('Edit Your Profile'),
      );
    } else if (!follow) {
      return GestureDetector(
        onTap: () => setState(() => follow = true),
        child: Container(
          alignment: Alignment.center,
          height: 30.h,
          width: double.infinity,
          decoration: BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.circular(5.r),
            border: Border.all(color: Colors.blue),
          ),
          child: const Text('Follow', style: TextStyle(color: Colors.black)),
        ),
      );
    } else {
      return Row(
        children: [
          Expanded(
            child: GestureDetector(
              onTap: () => setState(() => follow = false),
              child: Container(
                alignment: Alignment.center,
                height: 30.h,
                decoration: BoxDecoration(
                  color: Colors.grey.shade200,
                  borderRadius: BorderRadius.circular(5.r),
                  border: Border.all(color: Colors.grey.shade200),
                ),
                child: const Text('Unfollow', style: TextStyle(color: Colors.black)),
              ),
            ),
          ),
          SizedBox(width: 8.w),
          Expanded(
            child: Container(
              alignment: Alignment.center,
              height: 30.h,
              decoration: BoxDecoration(
                color: Colors.grey.shade200,
                borderRadius: BorderRadius.circular(5.r),
                border: Border.all(color: Colors.grey.shade200),
              ),
              child: const Text('Message', style: TextStyle(color: Colors.black)),
            ),
          ),
        ],
      );
    }
  }
}
