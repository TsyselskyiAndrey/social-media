import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:glowee/bloc/post_bloc/post_bloc.dart';
import 'package:glowee/bloc/post_bloc/post_events.dart';
import 'package:glowee/bloc/post_bloc/post_states.dart';
import 'package:glowee/model/post.dart';
import 'package:glowee/util/error_dialog.dart';
import 'package:glowee/widgets/navigation_menu.dart';
import 'package:glowee/widgets/post.dart';

class FeedScreen extends StatefulWidget {
  const FeedScreen({super.key});

  @override
  State<FeedScreen> createState() => _FeedScreenState();
}

class _FeedScreenState extends State<FeedScreen> {
  List<Post> _posts = [];
  final ScrollController _scrollController = ScrollController();
  bool _hasMore = true;
  bool _isLoadingMore = false;
  int postAmount = 10;

  @override
  void initState() {
    super.initState();
    context.read<PostBloc>().add(LoadPostsEvent(postAmount: postAmount));

    _scrollController.addListener(() {
      if (_scrollController.position.pixels ==
              _scrollController.position.maxScrollExtent &&
          !_isLoadingMore &&
          _hasMore) {
        _isLoadingMore = true;

        context.read<PostBloc>().add(LoadPostsEvent(postAmount: postAmount));
        postAmount += 10;
      }
    });
  }

  @override
  void dispose() {
    _scrollController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return BlocConsumer<PostBloc, PostState>(
      listener: (context, state) async {
        if (state is PostLoaded) {
          _hasMore = state.posts.length != _posts.length;
          _posts = state.posts;
          _isLoadingMore = false;
        } else if (state is PostError) {
          _isLoadingMore = false;
          _hasMore = false;
          await showErrorDialog(context, state.messages);
        }
      },
      builder: (context, state) {
        return Scaffold(
          bottomNavigationBar: const NavigationMenu(),
          appBar: AppBar(
            elevation: 0,
            automaticallyImplyLeading: false,
            title: SizedBox(
              width: 105.w,
              height: 30.h,
              child: Image.asset('assets/images/logo.png'),
            ),
            centerTitle: true,
            backgroundColor: const Color(0xffFAFAFA),
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
                child: (state is PostLoading && _posts.isEmpty)
                    ? const Center(
                        child: CircularProgressIndicator(color: Colors.white),
                      )
                    : CustomScrollView(
                        controller: _scrollController,
                        slivers: [
                          if (_posts.isEmpty)
                            SliverFillRemaining(
                              hasScrollBody: false,
                              child: Center(
                                child: Text(
                                  'There are no posts',
                                  style: TextStyle(
                                    fontSize: 25.sp,
                                    color: Colors.black,
                                  ),
                                ),
                              ),
                            )
                          else
                            SliverList(
                              delegate: SliverChildBuilderDelegate(
                                (context, index) {
                                  if (index < _posts.length) {
                                    return PostWidget(post: _posts[index]);
                                  } else {
                                    if (_hasMore) {
                                      return Padding(
                                        padding: EdgeInsets.all(16.w),
                                        child: Center(
                                          child: CircularProgressIndicator(),
                                        ),
                                      );
                                    }
                                  }
                                },
                                childCount: _posts.length,
                              ),
                            ),
                        ],
                      ),
              ),
            ],
          ),
        );
      },
    );
  }
}
