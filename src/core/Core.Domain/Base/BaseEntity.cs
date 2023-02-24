namespace Core.Domain.Base
{
    public abstract class BaseEntity
    {
        public long Id { get; private set; }
        public DateTime CreatedOn { get; private set; }

        protected BaseEntity()
        {
            CreatedOn = DateTime.Now;
        }
    }
}
