using Glowee.Domain.Common;
using Glowee.Domain.Entities.Chats;
using Glowee.Domain.Entities.Users;

namespace Glowee.Domain.Entities.UserChats
{
    public class UserChat : BaseEntity<UserChatId>
    {
        public UserId UserId { get; set; }
        public User User { get; set; }
        public ChatId ChatId { get; set; }
        public Chat Chat { get; set; }
        public bool? IsAdminOrModerator { get; set; }
    }
}
