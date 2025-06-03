import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:glowee/bloc/comment_bloc/comment_bloc.dart';
import 'package:glowee/bloc/post_bloc/post_bloc.dart';
import 'package:glowee/bloc/post_bloc/post_events.dart';
import 'package:glowee/model/post.dart';
import 'package:glowee/util/image_cached.dart';
import 'package:glowee/widgets/comment_section.dart';

class PostWidget extends StatefulWidget {
  final Post post;

  PostWidget({super.key, required this.post});

  @override
  State<PostWidget> createState() => _PostWidgetState();
}

class _PostWidgetState extends State<PostWidget> {
  final PageController _pageController = PageController();
  int _currentPage = 0;

  @override
  void initState() {
    super.initState();
    _pageController.addListener(() {
      setState(() {
        _currentPage = _pageController.page?.round() ?? 0;
      });
    });
  }

  @override
  void dispose() {
    _pageController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Container(
          width: double.infinity,
          height: 55.h,
          color: Colors.white,
          child: Center(
            child: ListTile(
              leading: ClipOval(
                child: SizedBox(
                  width: 45.w,
                  height: 45.h,
                  child: CachedImage(widget.post.authorIconUrl),
                ),
              ),
              title: Text(
                widget.post.authorName,
                style: TextStyle(fontSize: 16.sp),
              ),
            ),
          ),
        ),
        Container(
          width: double.infinity,
          height: 375.h,
          child: widget.post.postMedia.length == 1
              ? CachedImage(widget.post.postMedia.first.mediaUrl)
              : Stack(
                  alignment: Alignment.bottomCenter,
                  children: [
                    PageView.builder(
                      controller: _pageController,
                      itemCount: widget.post.postMedia.length,
                      itemBuilder: (context, index) {
                        return CachedImage(
                          widget.post.postMedia[index].mediaUrl,
                        );
                      },
                    ),
                    Positioned(
                      bottom: 10,
                      child: Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: List.generate(widget.post.postMedia.length,
                            (index) {
                          return Container(
                            margin: EdgeInsets.symmetric(horizontal: 4),
                            width: _currentPage == index ? 10 : 6,
                            height: 6,
                            decoration: BoxDecoration(
                              color: _currentPage == index
                                  ? Colors.white
                                  : Colors.white54,
                              shape: BoxShape.circle,
                            ),
                          );
                        }),
                      ),
                    ),
                  ],
                ),
        ),
        Container(
          width: double.infinity,
          color: Colors.white,
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              SizedBox(height: 14.w),
              Row(
                children: [
                  SizedBox(width: 14.w),
                  GestureDetector(
                    onTap: () {
                      context
                          .read<PostBloc>()
                          .add(LikePostBtnClicked(postId: widget.post.id));
                    },
                    child: Icon(widget.post.isLiked
                        ? Icons.favorite_outlined
                        : Icons.favorite_outline),
                  ),
                  SizedBox(width: 5.w),
                  Padding(
                    padding: EdgeInsets.only(left: 5.w),
                    child: Text(
                      widget.post.likes.toString(),
                      style: TextStyle(
                        fontSize: 15.sp,
                        fontWeight: FontWeight.w500,
                      ),
                    ),
                  ),
                  SizedBox(width: 20.w),
                  GestureDetector(
                    onTap: () {
                      showBottomSheet(
                        backgroundColor: Colors.transparent,
                        context: context,
                        builder: (context) {
                          return Padding(
                            padding: EdgeInsets.only(
                              bottom: MediaQuery.of(context).viewInsets.bottom,
                            ),
                            child: DraggableScrollableSheet(
                              maxChildSize: 0.5,
                              initialChildSize: 0.5,
                              minChildSize: 0.2,
                              builder: (context, scrollController) {
                                return BlocProvider(
                                  create: (context) => CommentBloc(),
                                  child: CommentSection(postId: widget.post.id),
                                );
                              },
                            ),
                          );
                        },
                      );
                    },
                    child: Icon(
                      Icons.chat_bubble_outline,
                      size: 30.w,
                    ),
                  ),
                  Spacer(),
                  Padding(
                    padding: EdgeInsets.only(right: 15.w),
                    child: GestureDetector(
                      onTap: () {
                        context
                            .read<PostBloc>()
                            .add(SavePostBtnClicked(postId: widget.post.id));
                      },
                      child: Icon(
                        widget.post.isSaved
                            ? Icons.bookmark
                            : Icons.bookmark_outline,
                        size: 30.w,
                      ),
                    ),
                  ),
                ],
              ),
              Padding(
                padding: EdgeInsets.symmetric(horizontal: 15.w, vertical: 10.h),
                child: Row(
                  children: [
                    Text(
                      widget.post.authorName,
                      style: TextStyle(
                        fontSize: 15.sp,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    Padding(
                      padding: EdgeInsets.only(left: 8.w),
                      child: Text(
                        widget.post.caption ?? '',
                        style: TextStyle(fontSize: 15.sp),
                      ),
                    ),
                  ],
                ),
              ),
              Padding(
                padding: EdgeInsets.only(left: 15.w, top: 7.h, bottom: 25.h),
                child: Text(
                  widget.post.tags.map((e) => '#$e').join(' '),
                  style: TextStyle(fontSize: 15.sp, color: Colors.black),
                ),
              ),
            ],
          ),
        ),
      ],
    );
  }
}
