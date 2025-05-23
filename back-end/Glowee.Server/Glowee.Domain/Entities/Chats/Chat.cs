using Glowee.Domain.Common;
using Glowee.Domain.Entities.Messages;
using Glowee.Domain.Entities.UserChats;

namespace Glowee.Domain.Entities.Chats
{
    public class Chat : BaseEntity<ChatId>
    {
        public string? Name { get; set; }
        public string? LogoPath { get; set; }
        public bool IsGroup { get; set; }
        public ICollection<UserChat> UsersChats { get; set; } = new List<UserChat>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
