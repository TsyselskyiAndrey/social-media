import 'dart:io';

import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:glowee/bloc/auth_bloc/auth_bloc.dart';
import 'package:glowee/bloc/auth_bloc/auth_events.dart';
import 'package:glowee/bloc/user_bloc/user_bloc.dart';
import 'package:glowee/bloc/user_bloc/user_events.dart';
import 'package:glowee/bloc/user_bloc/user_states.dart';
import 'package:glowee/model/post.dart';
import 'package:glowee/model/user.dart';
import 'package:glowee/screens/profile_image_notifier.dart';
import 'package:glowee/util/image_cached.dart';
import 'package:glowee/widgets/navigation_menu.dart';
import 'package:glowee/screens/edit_profile.dart';
import 'package:glowee/screens/login_screen.dart';
import 'package:glowee/screens/interface.dart';

class ProfileScreen extends StatefulWidget {
  final void Function(bool isDarkTheme)? onThemeChange;

  const ProfileScreen({Key? key, this.onThemeChange}) : super(key: key);

  @override
  State<ProfileScreen> createState() => _ProfileScreenState();
}

class _ProfileScreenState extends State<ProfileScreen> {
  late User? _user;
  List<Post> _posts = [];
  @override
  void initState() {
    context.read<UserBloc>().add(LoadUserProfileInfo());
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return BlocConsumer<UserBloc, UserState>(
      listener: (context, state) {
        if (state is UserLoaded) {
          _user = state.user;
          _posts = state.posts;
        }
      },
      builder: (context, state) {
        return (state is UserLoading || _user == null)
            ? const Center(child: CircularProgressIndicator())
            : DefaultTabController(
                length: 3,
                child: Scaffold(
                  backgroundColor: const Color.fromRGBO(27, 97, 103, 1),
                  appBar: AppBar(
                    title: Text(_user!.userName),
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
                  endDrawer: _buildDrawer(context),
                  body: SafeArea(
                    child: Stack(
                      children: [
                        CustomScrollView(
                          slivers: [
                            SliverToBoxAdapter(child: _buildHead(_user!)),
                            SliverGrid(
                              delegate: SliverChildBuilderDelegate(
                                (context, index) {
                                  print(_posts[index]);
                                  if (index >= _posts.length) {
                                    return Container();
                                  }
                                  return GestureDetector(
                                    onTap: () {},
                                    child: CachedImage(
                                      _posts[index].postMedia.first.mediaUrl,
                                    ),
                                  );
                                },
                                childCount: _posts.length,
                              ),
                              gridDelegate:
                                  const SliverGridDelegateWithFixedCrossAxisCount(
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
      },
    );
  }

  Widget _buildDrawer(BuildContext context) {
    return Drawer(
      child: ListView(
        padding: EdgeInsets.zero,
        children: [
          DrawerHeader(
            decoration: const BoxDecoration(color: Colors.teal),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                ValueListenableBuilder<File?>(
                  valueListenable: profileImageNotifier,
                  builder: (context, file, _) {
                    return CircleAvatar(
                      radius: 30.r,
                      backgroundImage: file != null
                          ? FileImage(file)
                          : const AssetImage(
                              'assets/images/default_profile_picture.jpg',
                            ) as ImageProvider,
                    );
                  },
                ),
              ],
            ),
          ),
          ListTile(
            leading: const Icon(Icons.save),
            title: const Text('Saved'),
            onTap: () => Navigator.pop(context),
          ),
          ListTile(
            leading: const Icon(Icons.settings),
            title: const Text('Payment'),
            onTap: () => Navigator.of(context).pop(),
          ),
          ListTile(
            leading: const Icon(Icons.person),
            title: const Text('Account'),
            onTap: () {
              Navigator.pop(context);
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) => EditProfileScreen(
                    user: _user!,
                  ),
                ),
              );
            },
          ),
          ListTile(
            leading: const Icon(Icons.payment),
            title: const Text('Help'),
            onTap: () => Navigator.pop(context),
          ),
          ListTile(
            leading: const Icon(Icons.sunny),
            title: const Text('Interface'),
            onTap: () {
              Navigator.pop(context);
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) =>
                      InterfaceScreen(onThemeChange: widget.onThemeChange),
                ),
              );
            },
          ),
          ListTile(
            leading: const Icon(Icons.exit_to_app),
            title: const Text('Logout'),
            onTap: () {
              Navigator.pushReplacement(
                context,
                MaterialPageRoute(
                  builder: (context) => BlocProvider(
                    create: (context) => AuthBloc(),
                    child: LoginScreen(),
                  ),
                ),
              );
            },
          ),
        ],
      ),
    );
  }

  Widget _buildHead(User user) {
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
                    child: user.profileImagePath != null
                        ? Image.network(
                            user.profileImagePath!,
                            fit: BoxFit.cover,
                            errorBuilder: (context, error, stackTrace) =>
                                Image.asset(
                              'assets/images/default_profile_picture.jpg',
                              fit: BoxFit.cover,
                            ),
                          )
                        : Image.asset(
                            'assets/images/default_profile_picture.jpg',
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
                      Text(
                        '${user.postsAmount}',
                        style: TextStyle(
                          color: Colors.white,
                          fontWeight: FontWeight.bold,
                          fontSize: 16.sp,
                        ),
                      ),
                      SizedBox(width: 55.w),
                      Text(
                        '${user.followers}',
                        style: TextStyle(
                          color: Colors.white,
                          fontWeight: FontWeight.bold,
                          fontSize: 16.sp,
                        ),
                      ),
                      SizedBox(width: 70.w),
                      Text(
                        '${user.followed}',
                        style: TextStyle(
                          color: Colors.white,
                          fontWeight: FontWeight.bold,
                          fontSize: 16.sp,
                        ),
                      ),
                    ],
                  ),
                  Row(
                    children: [
                      SizedBox(width: 30.w),
                      Text('Posts',
                          style:
                              TextStyle(color: Colors.white, fontSize: 13.sp)),
                      SizedBox(width: 25.w),
                      Text('Followers',
                          style:
                              TextStyle(color: Colors.white, fontSize: 13.sp)),
                      SizedBox(width: 19.w),
                      Text('Following',
                          style:
                              TextStyle(color: Colors.white, fontSize: 13.sp)),
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
                Text(
                  '${user.firstName} ${user.lastName}',
                  style: TextStyle(
                    color: Colors.white,
                    fontSize: 12.sp,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                SizedBox(height: 5.h),
                Text(
                  user.biography ?? '',
                  style: TextStyle(
                    color: Colors.white70,
                    fontSize: 12.sp,
                    fontWeight: FontWeight.w300,
                  ),
                ),
              ],
            ),
          ),
          SizedBox(height: 20.h),
          Padding(
            padding: EdgeInsets.symmetric(horizontal: 13.w),
            child: editProfileBtn(context),
          ),
          SizedBox(height: 10.h),
        ],
      ),
    );
  }

  Widget editProfileBtn(BuildContext context) {
    final isDark = Theme.of(context).brightness == Brightness.dark;

    return GestureDetector(
      onTap: () {
        Navigator.push(
          context,
          MaterialPageRoute(
            builder: (context) => BlocProvider(
              create: (context) => UserBloc(),
              child: EditProfileScreen(
                user: _user!,
              ),
            ),
          ),
        );
      },
      child: Container(
        alignment: Alignment.center,
        height: 30.h,
        width: double.infinity,
        decoration: BoxDecoration(
          color: isDark ? Colors.teal[700] : Colors.tealAccent[100],
          borderRadius: BorderRadius.circular(5.r),
          border:
              Border.all(color: isDark ? Colors.white54 : Colors.grey.shade400),
        ),
        child: Text(
          'Edit Your Profile',
          style: TextStyle(
            color: isDark ? Colors.white : Colors.black,
            fontWeight: FontWeight.bold,
          ),
        ),
      ),
    );
  }
}
