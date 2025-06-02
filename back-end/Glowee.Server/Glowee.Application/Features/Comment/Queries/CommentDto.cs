namespace Glowee.Application.Features.Comment.Queries;

public class CommentDto
{
    public long Id { get; set; }
    public CommentUserDto Author { get; set; }
    public string Content { get; set; }
    public List<CommentDto> ChildComments { get; set; }
    public bool IsLiked { get; set; }
    public int Likes { get; set; }
}