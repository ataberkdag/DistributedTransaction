using Core.Domain.Base;
using System.Text;

namespace Core.Domain.Entities
{
    public class OutboxMessage : BaseEntity
    {
        public string Type { get; private set; }
        public string Data { get; private set; }
        public string QueueName { get; private set; }

        // ORM
        protected OutboxMessage()
        {

        }

        private OutboxMessage(string type, string data, string queueName)
        {
            Type = type;
            Data = data;
            QueueName = queueName;
        }

        public static OutboxMessage Create(string type, string data, string queueName)
        {
            return new OutboxMessage(type, data, queueName);
        }
    }
}
