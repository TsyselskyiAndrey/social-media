import 'package:flutter/material.dart';
import 'package:glowee/screens/add_post.dart';
import 'package:glowee/screens/chat.dart';
import 'package:glowee/screens/feed.dart';
import 'package:glowee/screens/interaction.dart';
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
                MaterialPageRoute(builder: (_) => const FeedScreen()),
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
                MaterialPageRoute(builder: (_) => const RecommendationScreen()),
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
              Navigator.of(context).push(
                MaterialPageRoute(builder: (_) => const AddPostScreen()),
              );
            },
            icon: const Icon(
              Icons.add,
              color: Colors.black,
              size: 40,
            ),
          ),
          IconButton(
            enableFeedback: false,
            onPressed: () {
              Navigator.of(context).push(
                MaterialPageRoute(builder: (_) => const InteractionScreen()),
              );
            },
            icon: const Icon(
              Icons.favorite_border,
              color: Colors.black,
              size: 40,
            ),
          ),
          IconButton(
            enableFeedback: false,
            onPressed: () {
              Navigator.of(context).push(
                MaterialPageRoute(builder: (_) => const ChatScreen()),
              );
            },
            icon: const Icon(
              Icons.chat_bubble_outline,
              color: Colors.black,
              size: 40,
            ),
          ),
          GestureDetector(
            onTap: () {
              Navigator.of(context).push(
                MaterialPageRoute(builder: (_) => ProfileScreen(uid: "")),
              );
            },
            child: CircleAvatar(
              radius: 20,
              backgroundImage: null,
            ),
          ),
        ],
      ),
    );
  }
}
