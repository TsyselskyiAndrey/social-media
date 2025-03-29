using Glowee.Domain.Common;
using Glowee.Domain.Entities.Requests;

namespace Glowee.Domain.Entities.RequestTypes
{
    public class RequestType : BaseEntity<RequestTypeId>
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}
