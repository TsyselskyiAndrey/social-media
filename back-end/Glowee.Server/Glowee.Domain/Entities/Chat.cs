using Glowee.Domain.Common;
using System;

namespace Glowee.Domain.Entities
{
    public class Chat : BaseEntity<long>
    {
        public string? Name { get; set; }
        public string? LogoUrl { get; set; }
        public bool IsGroup { get; set; }
        public ICollection<UserChat> UsersChats { get; set; } = new List<UserChat>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
