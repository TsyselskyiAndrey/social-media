using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Glowee.Api.Requests;


public class CreatePostRequest
{
    public string Caption { get; set; }

    public List<string> Tags { get; set; } = new List<string>();
    
    public List<IFormFile>? PostMedias { get; set; }

    public IFormFile? Thumbnail { get; set; }
}