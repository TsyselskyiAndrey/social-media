namespace Glowee.Application.Features.Post.GeneralDto;

public class PostMediaDto
{
    public long Id { get; set; }
    public string MediaUrl { get; set; } = string.Empty;
    public string PostMediaType { get; set; }
    public string? ThumbnailUrl { get; set; }
    public TimeSpan? Duration { get; set; }
    public string Format { get; set; } = string.Empty;
    public long Size { get; set; }
    public bool? IsUploaded { get; set; }
    public int? Position { get; set; }
}