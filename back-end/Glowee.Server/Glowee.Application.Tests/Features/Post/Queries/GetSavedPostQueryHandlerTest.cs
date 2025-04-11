// using Glowee.Application.Contracts.Persistence;
// using Glowee.Application.Features.Post.Queries.SavedPosts;
// using Glowee.Application.Tests.Mocks;
// using Glowee.Domain.Entities.Users;
// using Moq;
// using Shouldly;
//
// namespace Glowee.Application.Tests.Features.Post.Queries;
//
// public class GetSavedPostQueryHandlerTest
// {
//     private readonly Mock<IPostRepository> _postRepository;
//     private readonly Mock<ISavedPostRepository> _savedPostRepository;
//     private readonly Mock<ITagRepository> _tagRepository;
//     private readonly Mock<ILikeRepository> _likeRepository;
//     private readonly Mock<IUninterestingPostRepository> _uninterestingPostRepository;
//     private readonly Mock<IPostMediaRepository> _postMediaRepository;
//     private readonly Mock<IUserRepository> _userRepository;
//
//     public GetSavedPostQueryHandlerTest()
//     {
//         _postRepository = MockPostRepository.GetMockPostRepository();
//         _savedPostRepository = MockSavedPostRepository.GetMockSavedPostRepository();
//         _tagRepository = MockTagRepository.GetMockTagRepository();
//         _likeRepository = MockLikeRepository.GetMockRepository();
//         _uninterestingPostRepository = MockUninterestingPostRepository.GetMockUninterestingPostRepository();
//         _postMediaRepository = MockPostMediaRepository.GetMockPostMediaRepository();
//         _userRepository = MockUserRepository.GetMockUsersRepository();
//     }
//
//     [Theory]
//     [MemberData(nameof(GetSavedPostQueryData))]
//     public async Task GetSavedPostQueryHandler_Test(GetSavedPostsQuery query, List<PostDto> expected)
//     {
//         var handler = new GetSavedPostsQueryHandler(
//             _postRepository.Object,
//             _savedPostRepository.Object,
//             _tagRepository.Object,
//             _likeRepository.Object,
//             _uninterestingPostRepository.Object,
//             _postMediaRepository.Object,
//             _userRepository.Object
//         );
//
//         var result = await handler.Handle(query, CancellationToken.None);
//
//         result.ShouldBe(expected);
//     }
//
//     public static IEnumerable<object[]> GetSavedPostQueryData()
//     {
//         yield return
//         [
//             new GetSavedPostsQuery(new UserId(2)),
//             new List<PostDto>
//             {
//                 new PostDto
//                 {
//                     Id = 2,
//                     UserId = 2,
//                     Caption = ".NET 8 News",
//                     PostType = "Video",
//                     Tags = new List<string>(),
//                     IsLiked = true,
//                     IsSaved = true
//                 }
//             }
//         ];
//
//         yield return
//         [
//             new GetSavedPostsQuery(new UserId(3)),
//             new List<PostDto>
//             {
//                 new PostDto
//                 {
//                     Id = 3,
//                     UserId = 3,
//                     Caption = "EF Core Guide",
//                     PostType = "GIF",
//                     Tags = new List<string>(),
//                     IsLiked = false,
//                     IsSaved = true
//                 }
//             }
//         ];
//     }
// }