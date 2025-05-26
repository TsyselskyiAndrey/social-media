namespace Glowee.Api.Requests.Comment
{
    public class CreateCommentRequest
    {
        public string Content { get; set; }
        public long PostId { get; set; }
        public long? ParentCommentId { get; set; }
    }
}
