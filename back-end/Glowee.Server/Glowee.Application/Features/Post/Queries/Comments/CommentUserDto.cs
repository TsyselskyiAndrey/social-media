namespace Glowee.Application.Features.Post.Queries.Comments;

public class CommentUserDto
{
    public long Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
}