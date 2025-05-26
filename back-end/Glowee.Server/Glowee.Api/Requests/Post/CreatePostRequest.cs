namespace Glowee.Api.Requests.Post;


public class CreatePostRequest
{
    public string Caption { get; set; }

    public List<string> Tags { get; set; } = new List<string>();

    public List<IFormFile>? PostMedias { get; set; }

    public IFormFile? Thumbnail { get; set; }
}