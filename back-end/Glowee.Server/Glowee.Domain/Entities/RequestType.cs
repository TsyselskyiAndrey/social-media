using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class RequestType : BaseEntity<int>
    {
        public string Name { get; set; } = String.Empty;
        public ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}
