using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class Request : BaseEntity<long>
    {
        public long SenderId { get; set; }
        public User Sender { get; set; }
        public long RecipientId { get; set; }
        public User Recipient { get; set; }
        public int RequestTypeId { get; set; }
        public RequestType RequestType { get; set; }
        public bool? IsAccepted { get; set; }

    }
}
