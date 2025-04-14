namespace Glowee.Application.Features.Post.GeneralDto;

public class PostDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string? Caption { get; set; }
    public string PostType { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
    public int Likes { get; set; }
    public int Views { get; set; }
    public bool IsLiked { get; set; }
    public bool IsSaved { get; set; }
    public bool IsUninteresting { get; set; }
    public List<PostMediaDto> PostMediaDtos { get; set; } = new();
}