using System.Text.Json.Serialization;

namespace Core.Domain.Base
{
    public abstract class BaseRootEntity : BaseEntity
    {
        [JsonIgnore]
        private List<IDomainEvent> _events;

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _events?.AsReadOnly();
        protected BaseRootEntity()
        {

        }

        public void AddDomainEvent(IDomainEvent @event)
        {
            if (_events is null)
                _events = new List<IDomainEvent>();

            _events.Add(@event);
        }

        public void RemoveDomainEvent(IDomainEvent @event)
        {
            _events?.Remove(@event);
        }

        public void ClearDomainEvents()
        {
            _events?.Clear();
        }

    }
}
