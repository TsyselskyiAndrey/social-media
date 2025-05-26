namespace Glowee.Api.Requests.Comment
{
    public class EditCommentRequest
    {
        public long CommentId { get; set; }
        public string Content { get; set; }
    }
}
