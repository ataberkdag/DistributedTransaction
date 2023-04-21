using Core.Application.Common;
using Core.Application.Exceptions;
using FluentAssertions;
using Moq;
using Payment.Application;
using Payment.Application.Features.Commands;
using Payment.Domain.Interfaces;

namespace Payment.Test.Features
{
    public class DoPaymentTests
    {
        [Fact]
        public async Task DoPayment_Success()
        {
            // Arrange
            var command = new DoPayment.Command()
            {
                CorrelationId = Guid.NewGuid(),
                TotalAmount = 180,
                UserId = Guid.NewGuid()
            };

            var paymentUow = new Mock<IPaymentUnitOfWork>();
            paymentUow.Setup(x => x.PaymentTransactions.Add(It.IsAny<Domain.Entities.PaymentTransaction>()));
            paymentUow.Setup(x => x.SaveChangesAsync());

            var doPayment = new DoPayment.CommandHandler(paymentUow.Object);

            // Act
            var result = await doPayment.Handle(command, new CancellationToken());

            // Assert
            result.Should().BeEquivalentTo(BaseResult.Success());
        }
    }
}
