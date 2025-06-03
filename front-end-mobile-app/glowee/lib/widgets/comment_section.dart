import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'package:glowee/bloc/comment_bloc/comment_bloc.dart';
import 'package:glowee/bloc/comment_bloc/comment_events.dart';
import 'package:glowee/bloc/comment_bloc/comment_states.dart';
import 'package:glowee/model/comment.dart';
import 'package:glowee/util/error_dialog.dart';

class CommentSection extends StatefulWidget {
  final int postId;
  const CommentSection({super.key, required this.postId});

  @override
  State<CommentSection> createState() => _CommentSectionState();
}

class _CommentSectionState extends State<CommentSection> {
  final commentController = TextEditingController();
  List<Comment>? _comments;

  int? replyingToCommentId;

  @override
  void initState() {
    context
        .read<CommentBloc>()
        .add(GetCommentsBtnClicked(postId: widget.postId));
    super.initState();
  }

  @override
  void dispose() {
    commentController.dispose();
    super.dispose();
  }

  void toggleLike(Comment comment) {
    setState(() {
      comment.isLiked = !comment.isLiked;
      comment.likes += comment.isLiked ? 1 : -1;
    });
  }

  Widget buildComment(Comment comment, {int indentLevel = 0}) {
    return Padding(
      padding: EdgeInsets.only(left: 20.0 * indentLevel),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          ListTile(
            leading: ClipOval(
              child: comment.author.profileImageUrl != null
                  ? Image.network(comment.author.profileImageUrl!,
                      width: 35, height: 35, fit: BoxFit.cover)
                  : Container(
                      width: 35,
                      height: 35,
                      color: Colors.grey.shade300,
                      child: Icon(Icons.person, size: 20),
                    ),
            ),
            title: Text(
              comment.author.userName,
              style: TextStyle(fontWeight: FontWeight.bold),
            ),
            subtitle: Text(comment.content),
            trailing: Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                GestureDetector(
                  onTap: () {
                    toggleLike(comment);
                    context
                        .read<CommentBloc>()
                        .add(LikeCommentBtnClicked(commentId: comment.id));
                  },
                  child: Icon(
                    comment.isLiked ? Icons.favorite : Icons.favorite_border,
                    color: comment.isLiked ? Colors.red : Colors.grey,
                    size: 20,
                  ),
                ),
                SizedBox(width: 4),
                Text('${comment.likes}'),
                SizedBox(width: 10),
                if (indentLevel == 0)
                  TextButton(
                    onPressed: () {
                      setState(() {
                        replyingToCommentId = comment.id;
                      });
                    },
                    child: Text('Reply'),
                  ),
              ],
            ),
          ),
          ...comment.childComments
              .map((reply) => buildComment(reply, indentLevel: indentLevel + 1))
              .toList(),
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return BlocConsumer<CommentBloc, CommentState>(
      listener: (context, state) {
        if (state is CommentsLoaded) {
          _comments = state.comments;
        } else if (state is CommentStepSuccess) {
          context
              .read<CommentBloc>()
              .add(GetCommentsBtnClicked(postId: widget.postId));
        }
      },
      builder: (context, state) {
        if (state is CommentLoading) {
          return Center(child: CircularProgressIndicator());
        }

        if (_comments == null || _comments!.isEmpty) {
          return ClipRRect(
            borderRadius: BorderRadius.vertical(top: Radius.circular(25)),
            child: Container(
              color: Colors.white,
              height: 400,
              child: Column(
                children: [
                  Container(
                    margin: EdgeInsets.symmetric(vertical: 8),
                    width: 100,
                    height: 3,
                    color: Colors.black,
                  ),
                  Expanded(
                    child: Center(
                      child: Text(
                        'No comments',
                        style: TextStyle(fontSize: 16, color: Colors.grey),
                      ),
                    ),
                  ),
                  Divider(height: 1),
                  _buildInputField(),
                ],
              ),
            ),
          );
        }

        return ClipRRect(
          borderRadius: BorderRadius.vertical(top: Radius.circular(25)),
          child: Container(
            color: Colors.white,
            height: 400,
            child: Column(
              children: [
                Container(
                  margin: EdgeInsets.symmetric(vertical: 8),
                  width: 100,
                  height: 3,
                  color: Colors.black,
                ),
                Expanded(
                  child: ListView(
                    children: _comments!.map((c) => buildComment(c)).toList(),
                  ),
                ),
                Divider(height: 1),
                _buildInputField(),
              ],
            ),
          ),
        );
      },
    );
  }

  Widget _buildInputField() {
    return Container(
      padding: EdgeInsets.symmetric(horizontal: 10),
      color: Colors.white,
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          if (replyingToCommentId != null)
            Container(
              padding: EdgeInsets.symmetric(horizontal: 10, vertical: 5),
              color: Colors.grey[200],
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  Expanded(
                    child: Text(
                      'Replying to comment ID: $replyingToCommentId',
                      style:
                          TextStyle(fontSize: 12, fontStyle: FontStyle.italic),
                    ),
                  ),
                  IconButton(
                    icon: Icon(Icons.close),
                    onPressed: () {
                      setState(() {
                        replyingToCommentId = null;
                      });
                    },
                  ),
                ],
              ),
            ),
          Row(
            children: [
              Expanded(
                child: TextField(
                  controller: commentController,
                  decoration: InputDecoration(
                    hintText: replyingToCommentId != null
                        ? 'Reply to comment...'
                        : 'Add comment..',
                    border: InputBorder.none,
                  ),
                ),
              ),
              IconButton(
                icon: Icon(Icons.send),
                onPressed: () async {
                  if (commentController.text.isNotEmpty) {
                    context.read<CommentBloc>().add(
                          CreateCommentBtnClicked(
                            content: commentController.text,
                            postId: widget.postId,
                            parentCommentId: replyingToCommentId,
                          ),
                        );
                    commentController.clear();
                    setState(() {
                      replyingToCommentId = null;
                    });
                  } else {
                    await showErrorDialog(context, ["Message can't be empty"]);
                  }
                },
              ),
            ],
          ),
        ],
      ),
    );
  }
}
