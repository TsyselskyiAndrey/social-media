namespace Glowee.Domain.Common
{
    public interface IEntity
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
