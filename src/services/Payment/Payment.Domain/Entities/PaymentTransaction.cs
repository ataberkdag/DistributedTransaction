using Core.Domain.Base;
using Payment.Domain.Events;

namespace Payment.Domain.Entities
{
    public class PaymentTransaction : BaseRootEntity
    {
        public Guid CorrelationId { get; private set; }
        public Guid UserId { get; private set; }
        public decimal Amount { get; private set; }
        public string StatusCode { get; private set; }
        public string Description { get; private set; }
        public bool Succeeded { get; private set; }

        protected PaymentTransaction()
        {

        }

        private PaymentTransaction(Guid correlationId, Guid userId, decimal amount)
        {
            CorrelationId= correlationId;
            UserId= userId;
            Amount= amount;
        }

        public static PaymentTransaction Create(Guid correlationId, Guid userId, decimal amount)
        {
            return new PaymentTransaction(correlationId, userId, amount);
        }

        public void SucceededPayment()
        {
            Succeeded = true;
            StatusCode = "0000";
            Description = "Succeeded";

            this.AddDomainEvent(new PaymentSucceeded(CorrelationId, UserId));
        }

        public void FailedPayment(string statusCode, string description)
        {
            StatusCode = statusCode;
            Description = description;

            this.AddDomainEvent(new PaymentFailed(CorrelationId, UserId));
        }
    }
}
