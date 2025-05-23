using MediatR;
using Microsoft.AspNetCore.Http;

namespace Glowee.Application.Features.Post.Commands.Post.CreatePost;

public class CreatePostCommand : IRequest
{
    public string Caption { get; set; }
    public List<string> Tags { get; set; }
    public List<IFormFile> PostMedias { get; set; }
    public IFormFile? Thumbnail { get; set; }
}