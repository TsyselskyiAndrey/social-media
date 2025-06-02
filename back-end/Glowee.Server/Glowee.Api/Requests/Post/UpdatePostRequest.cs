namespace Glowee.Api.Requests.Post;

public class UpdatePostRequest
{
    public long Id { get; set; }
    public string Caption { get; set; }
    public List<string>? Tags { get; set; }
    public List<IFormFile> PostMedias { get; set; }
    public IFormFile? Thumbnail { get; set; }
}