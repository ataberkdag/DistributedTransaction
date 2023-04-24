using Core.Application.Common;
using Core.Application.Exceptions;
using Core.Application.Services;
using FluentAssertions;
using Moq;
using Order.Application;
using Order.Application.Features.Commands;
using Order.Domain.Interfaces;
using System.Linq.Expressions;

namespace Order.Test.Features
{
    public class PaymentSucceededFeatureTests
    {
        [Fact]
        public async Task PaymentSucceeded_Success()
        {
            // Arrange
            var order = Domain.Entities.Order.PlaceOrder(Guid.NewGuid(), new List<Domain.Dtos.OrderItemDto>());

            var orderUnitOfWork = new Mock<IOrderUnitOfWork>();
            orderUnitOfWork.Setup(x => x.Orders.FindByQuery(
                It.IsAny<Expression<Func<Domain.Entities.Order, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<Domain.Entities.Order> { order });

            var distributedLock = new Mock<IDistributedLockManager>();
            distributedLock.Setup(x => x.UnLock(It.IsAny<string>())).ReturnsAsync(true);

            var paymentSuccededCommandHandler = new PaymentSucceeded.CommandHandler(orderUnitOfWork.Object, distributedLock.Object);

            var command = new PaymentSucceeded.Command
            {
                CorrelationId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            // Act
            var result = await paymentSuccededCommandHandler.Handle(command, new CancellationToken());

            // Assert
            result.Should().BeEquivalentTo(BaseResult.Success());
        }

        [Fact]
        public async Task PaymentSucceeded_Failure_OrderIsNull()
        {
            // Arrange
            var orderUnitOfWork = new Mock<IOrderUnitOfWork>();
            orderUnitOfWork.Setup(x => x.Orders.FindByQuery(
                It.IsAny<Expression<Func<Domain.Entities.Order, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<Domain.Entities.Order>());

            var distributedLock = new Mock<IDistributedLockManager>();
            distributedLock.Setup(x => x.UnLock(It.IsAny<string>())).ReturnsAsync(true);

            var paymentSuccededCommandHandler = new PaymentSucceeded.CommandHandler(orderUnitOfWork.Object, distributedLock.Object);

            var command = new PaymentSucceeded.Command
            {
                CorrelationId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            // Act
            var ex = await Assert.ThrowsAsync<BusinessException>(() => paymentSuccededCommandHandler.Handle(command, new CancellationToken()));

            // Assert
            Assert.Equal(BusinessExceptionCodes.OrderNotFound.ToString(), ex.Message);
        }

        [Fact]
        public async Task PaymentSucceeded_Failure_OrderProcessed()
        {
            // Arrange
            var order = Domain.Entities.Order.PlaceOrder(Guid.NewGuid(), new List<Domain.Dtos.OrderItemDto>());
            order.CompleteOrder();

            var orderUnitOfWork = new Mock<IOrderUnitOfWork>();
            orderUnitOfWork.Setup(x => x.Orders.FindByQuery(
                It.IsAny<Expression<Func<Domain.Entities.Order, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<Domain.Entities.Order> { order });

            var distributedLock = new Mock<IDistributedLockManager>();
            distributedLock.Setup(x => x.UnLock(It.IsAny<string>())).ReturnsAsync(true);

            var paymentSuccededCommandHandler = new PaymentSucceeded.CommandHandler(orderUnitOfWork.Object, distributedLock.Object);

            var command = new PaymentSucceeded.Command
            {
                CorrelationId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            // Act
            var ex = await Assert.ThrowsAsync<BusinessException>(() => paymentSuccededCommandHandler.Handle(command, new CancellationToken()));

            // Assert
            Assert.Equal(BusinessExceptionCodes.OrderProcessed.ToString(), ex.Message);
        }
    }
}
