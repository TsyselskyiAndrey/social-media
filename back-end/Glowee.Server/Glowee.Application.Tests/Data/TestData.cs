using Glowee.Application.Exceptions;
using Glowee.Application.Features.Post.Commands.Comment.CreateComment;
using Glowee.Application.Features.Post.Commands.Comment.DeleteComment;
using Glowee.Application.Features.Post.Commands.Comment.EditComment;
using Glowee.Domain.Entities.Comments;
using UnauthorizedAccessException = Glowee.Application.Exceptions.UnauthorizedAccessException;

namespace Glowee.Application.Tests.Data;

public static class TestData
{
    public static IEnumerable<object[]> GetLikeCommandTestData()
    {
        yield return [1, 1, false];
        yield return [1, 1, true];
        yield return [2, 2, false];
        yield return [300, 1, typeof(NotFoundException)];
        yield return [6, 1, false]; 
        yield return [8, 3, false];
        yield return [1, 999, typeof(NotFoundException)];
        yield return [1, 5, true];
    }
    
    public static IEnumerable<object[]> GetSavedPostsTestData()
    {
        yield return [1, 1, false];
        yield return [2, 1, false];
        yield return [1, 1, true];
        yield return [3, 1, false];
        yield return [7, 1, true];
        yield return [10, 4, true];
        yield return [9999999, 1, typeof(NotFoundException)];
        yield return [1, 9494, typeof(NotFoundException)];
        yield return [74555, 16161, typeof(NotFoundException)];
    }

    public static IEnumerable<object[]> GetUsersSavedPostsTestData()
    {
        yield return [ 1, 3 ];
        yield return [ 4, 1 ];
        yield return [ 5, 1 ];
    }

    public static IEnumerable<object[]> GetPostCommentTestData()
    {
        yield return [1, 2, 1];
        yield return [2, 2, 0];
    }

    public static IEnumerable<object[]> CrateValidCommentsTestData()
    {
        yield return [new CreateCommentCommand()
        {
            Content = "Gulu gulu",
            PostId = 5,
        }];
        yield return [new CreateCommentCommand()
        {
            Content = "45456",
            PostId = 2,
        }];
        yield return [new CreateCommentCommand()
        {
            Content = "4555dkddlm",
            PostId = 3,
        }];
    }
    
    public static IEnumerable<object[]> CreateInvalidCommentsTestData()
    {
        yield return new object[]
        {
            new CreateCommentCommand
            {
                Content = "Gulu gulu",
                PostId = 789789
            },
            typeof(NotFoundException)
        };
    
        yield return new object[]
        {
            new CreateCommentCommand
            {
                Content = "",
                PostId = 2
            },
            typeof(BadRequestException)
        };
    
        yield return new object[]
        {
            new CreateCommentCommand
            {
                Content = null!,
                PostId = 3
            },
            typeof(BadRequestException)
        };
    
        yield return new object[]
        {
            new CreateCommentCommand
            {
                Content = new string('a', 2200),
                PostId = 3
            },
            typeof(BadRequestException)
        };
    }
    
    public static IEnumerable<object[]> CreateValidDeleteCommentsTestData()
    {
        yield return new object[]
        {
            new DeleteCommentCommand(new CommentId(1))
        };
    }

    public static IEnumerable<object[]> CreateInvalidDeleteCommentsTestData()
    {
        yield return new object[]
        {
            new DeleteCommentCommand(new CommentId(9999)),
            typeof(NotFoundException)
        };

        yield return new object[]
        {
            new DeleteCommentCommand(new CommentId(3)),
            typeof(Glowee.Application.Exceptions.UnauthorizedAccessException)
        };
    }

    public static IEnumerable<object[]> CreateValidEditCommentsTestData()
    {
        yield return new object[]
        {
            new EditCommentCommand
            {
                CommentId = 3,
                Content = "Updated content"
            }
        };
    }

    public static IEnumerable<object[]> CreateInvalidEditCommentsTestData()
    {
        yield return new object[]
        {
            new EditCommentCommand
            {
                CommentId = 9999,
                Content = "Updated content"
            },
            typeof(NotFoundException)
        };

        yield return new object[]
        {
            new EditCommentCommand
            {
                CommentId = 1,
                Content = ""
            },
            typeof(BadRequestException)
        };

        yield return new object[]
        {
            new EditCommentCommand
            {
                CommentId = 1,
                Content = new string('x', 2200)
            },
            typeof(BadRequestException)
        };
    }

}