import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:flutter_staggered_grid_view/flutter_staggered_grid_view.dart';
import 'package:glowee/bloc/post_bloc/post_bloc.dart';
import 'package:glowee/bloc/post_bloc/post_events.dart';
import 'package:glowee/bloc/post_bloc/post_states.dart';
import 'package:glowee/model/post.dart';
import 'package:glowee/util/error_dialog.dart';
import 'package:glowee/util/image_cached.dart';
import 'package:glowee/widgets/follow_button.dart';
import 'package:glowee/widgets/navigation_menu.dart';

class RecommendationScreen extends StatefulWidget {
  const RecommendationScreen({super.key});

  @override
  State<RecommendationScreen> createState() => _RecommendationScreenState();
}

class _RecommendationScreenState extends State<RecommendationScreen> {
  final _searchController = TextEditingController();
  final _searchFocusNode = FocusNode();
  bool _isFocused = false;

  List<Post> _posts = [];
  final ScrollController _scrollController = ScrollController();
  bool _hasMore = true;
  bool _isLoadingMore = false;
  int postAmount = 50;

  @override
  void initState() {
    super.initState();
    _searchFocusNode.addListener(() {
      setState(() {
        _isFocused = _searchFocusNode.hasFocus;
      });
    });

    context.read<PostBloc>().add(LoadPostsEvent(postAmount: postAmount));

    _scrollController.addListener(() {
      if (_scrollController.position.pixels ==
              _scrollController.position.maxScrollExtent &&
          !_isLoadingMore &&
          _hasMore) {
        _isLoadingMore = true;

        context.read<PostBloc>().add(LoadPostsEvent(postAmount: postAmount));
        postAmount += 50;
      }
    });
  }

  @override
  void dispose() {
    _searchController.dispose();
    _searchFocusNode.dispose();
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
          extendBody: true,
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
                child: CustomScrollView(
                  controller: _scrollController,
                  slivers: [
                    if (_posts.isEmpty)
                      SliverFillRemaining(
                        hasScrollBody: false,
                        child: Center(
                          child: Text(
                            'Постів немає',
                            style: TextStyle(
                              fontSize: 25.sp,
                              color: Colors.black,
                            ),
                          ),
                        ),
                      )
                    else
                      SliverToBoxAdapter(
                        child: Padding(
                          padding: EdgeInsets.symmetric(
                            horizontal: 12.w,
                            vertical: 25.h,
                          ),
                          child: Row(
                            crossAxisAlignment: CrossAxisAlignment.center,
                            children: [
                              if (_isFocused) ...[
                                Padding(
                                  padding:
                                      EdgeInsets.symmetric(horizontal: 8.w),
                                  child: GestureDetector(
                                    onTap: () {
                                      _searchFocusNode.unfocus();
                                    },
                                    child: const Icon(Icons.arrow_back_ios),
                                  ),
                                ),
                              ],
                              Expanded(
                                child: Container(
                                  width: double.infinity,
                                  height: 45.h,
                                  decoration: BoxDecoration(
                                    color:
                                        const Color.fromRGBO(238, 238, 238, 1),
                                    borderRadius: BorderRadius.all(
                                      Radius.circular(10.r),
                                    ),
                                  ),
                                  child: Padding(
                                    padding:
                                        EdgeInsets.symmetric(horizontal: 5.w),
                                    child: Row(
                                      children: [
                                        Padding(
                                          padding: EdgeInsets.only(left: 8.w),
                                          child: const Icon(
                                            Icons.search,
                                            color: Colors.black,
                                          ),
                                        ),
                                        SizedBox(width: 10.w),
                                        Expanded(
                                          child: TextField(
                                            controller: _searchController,
                                            focusNode: _searchFocusNode,
                                            textAlignVertical:
                                                TextAlignVertical.center,
                                            decoration: const InputDecoration(
                                              isCollapsed: true,
                                              hintText: 'Search',
                                              hintStyle: TextStyle(
                                                color: Colors.black,
                                              ),
                                              enabledBorder: InputBorder.none,
                                              focusedBorder: InputBorder.none,
                                            ),
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
                    _isFocused
                        ? _buildSuggestionsList()
                        : _buildRecommendation(),
                  ],
                ),
              ),
            ],
          ),
        );
      },
    );
  }

  Widget _buildRecommendation() {
    return SliverGrid(
      delegate: SliverChildBuilderDelegate(
        (context, index) {
          if (index < _posts.length) {
            return GestureDetector(
              child: CachedImage(_posts[index].postMedia.first.mediaUrl),
            );
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
      gridDelegate: SliverQuiltedGridDelegate(
        crossAxisCount: 3,
        mainAxisSpacing: 3,
        crossAxisSpacing: 3,
        repeatPattern: QuiltedGridRepeatPattern.inverted,
        pattern: [
          const QuiltedGridTile(1, 1),
          const QuiltedGridTile(2, 2),
          const QuiltedGridTile(1, 1),
        ],
      ),
    );
  }

  Widget _buildSuggestionsList() {
    return SliverList(
      delegate: SliverChildBuilderDelegate(
        (BuildContext context, int index) {
          return Padding(
            padding: EdgeInsets.symmetric(vertical: 5.h, horizontal: 5.w),
            child: ListTile(
              leading: CircleAvatar(
                radius: 20,
                backgroundImage: null,
              ),
              title: Text(
                'User $index',
                style: TextStyle(fontSize: 18.sp),
              ),
              trailing: const FollowButton(),
            ),
          );
        },
        childCount: 10,
      ),
    );
  }
}
