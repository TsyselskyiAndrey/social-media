using Glowee.Domain.Common;
using Glowee.Domain.Entities.RequestTypes;
using Glowee.Domain.Entities.Users;

namespace Glowee.Domain.Entities.Requests
{
    public class Request : BaseEntity<RequestId>
    {
        public UserId SenderId { get; set; }
        public User Sender { get; set; }
        public UserId RecipientId { get; set; }
        public User Recipient { get; set; }
        public RequestTypeId RequestTypeId { get; set; }
        public RequestType RequestType { get; set; }
        public bool? IsAccepted { get; set; }

    }
}
