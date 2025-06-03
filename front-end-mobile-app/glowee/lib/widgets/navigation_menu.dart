import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:glowee/bloc/post_bloc/post_bloc.dart';
import 'package:glowee/bloc/user_bloc/user_bloc.dart';
import 'package:glowee/screens/addPostScreen.dart';
import 'package:glowee/screens/feed.dart';
import 'package:glowee/screens/profile.dart';
import 'package:glowee/screens/recommendation.dart';

class NavigationMenu extends StatelessWidget {
  const NavigationMenu({super.key});

  @override
  Widget build(BuildContext context) {
    return Container(
      decoration: const BoxDecoration(
        color: Colors.transparent,
      ),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceAround,
        children: [
          IconButton(
            enableFeedback: false,
            onPressed: () {
              Navigator.of(context).push(
                MaterialPageRoute(
                  builder: (_) => BlocProvider(
                    create: (context) => PostBloc(),
                    child: const FeedScreen(),
                  ),
                ),
              );
            },
            icon: const Icon(
              Icons.home_filled,
              color: Colors.black,
              size: 40,
            ),
          ),
          IconButton(
            enableFeedback: false,
            onPressed: () {
              Navigator.of(context).push(
                MaterialPageRoute(
                  builder: (_) => BlocProvider(
                    create: (context) => PostBloc(),
                    child: const RecommendationScreen(),
                  ),
                ),
              );
            },
            icon: const Icon(
              Icons.search,
              color: Colors.black,
              size: 40,
            ),
          ),
          IconButton(
            enableFeedback: false,
            onPressed: () {
              Navigator.of(context).pushReplacement(
                MaterialPageRoute(
                  builder: (_) => BlocProvider(
                    create: (context) => PostBloc(),
                    child: const AddPostScreen(),
                  ),
                ),
              );
            },
            icon: const Icon(
              Icons.add,
              color: Colors.black,
              size: 40,
            ),
          ),
          GestureDetector(
            onTap: () {
              Navigator.of(context).pushReplacement(
                MaterialPageRoute(
                  builder: (_) => BlocProvider(
                    create: (context) => UserBloc(),
                    child: ProfileScreen(),
                  ),
                ),
              );
            },
            child: CircleAvatar(
              radius: 20,
              backgroundImage:
                  AssetImage('assets/images/default_profile_picture.jpg')
                      as ImageProvider,
              backgroundColor: Colors.transparent,
            ),
          ),
        ],
      ),
    );
  }
}
