using Glowee.Application.Features.Post.Commands.Post.CreatePost;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Glowee.Application.Features.Post.Commands.Post.UpdatePost;

public class EditPostCommand :  IRequest
{
    public long Id { get; set; }
    public string Caption { get; set; }
    public List<string>? Tags { get; set; }
    public List<IFormFile> PostMedias { get; set; }
    public IFormFile? Thumbnail { get; set; }
}