using Glowee.Application.Exceptions;
using Glowee.Application.Features.Post.GeneralDto;

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
        yield return [ 1, 3];
        yield return [ 4, 1];
        yield return [ 5, 1];
    }
}