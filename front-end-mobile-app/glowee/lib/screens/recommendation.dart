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
import 'package:glowee/widgets/navigation_menu.dart';

class RecommendationScreen extends StatefulWidget {
  const RecommendationScreen({super.key});

  @override
  State<RecommendationScreen> createState() => _RecommendationScreenState();
}

class _RecommendationScreenState extends State<RecommendationScreen> {
  List<Post> _posts = [];
  final ScrollController _scrollController = ScrollController();
  bool _hasMore = true;
  bool _isLoadingMore = false;
  int postAmount = 50;

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
        postAmount += 50;
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
                      _buildRecommendation(),
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
            return CachedImage(_posts[index].postMedia.first.mediaUrl);
          }
          if (_hasMore) {
            return Padding(
              padding: EdgeInsets.all(16.w),
              child: Center(
                child: CircularProgressIndicator(),
              ),
            );
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
}
