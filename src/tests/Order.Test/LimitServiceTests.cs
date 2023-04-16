using Core.Application.Exceptions;
using Core.Application.Services;
using Moq;
using Order.Application.Models;
using Order.Application.Models.Contracts;
using Order.Infrastructure.Services;

namespace Order.Test
{
    public class LimitServiceTests
    {
        [Fact]
        public async Task CheckLimit_Success()
        {
            // Arrange - Prepare Resource
            var checkLimitResult = new CheckLimitResult
            {
                IsLimitExceeded = false,
                Succeeded = true,
                StatusCode = "200"
            };

            var httpCallerMock = new Mock<IHttpCaller>();
            httpCallerMock.Setup(x => x.Get<CheckLimitResult>(It.IsAny<Action<HttpCallOptions>>())).ReturnsAsync(checkLimitResult);

            var limitServiceConfigMock = new Mock<LimitServiceConfig>();

            var limitService = new LimitService(limitServiceConfigMock.Object, httpCallerMock.Object);

            // Act - Run Flow
            var result = await limitService.CheckLimit(new CheckLimitRequest { UserId = Guid.NewGuid() });

            // Assert - Check
            Assert.IsType<CheckLimitResult>(result);
        }

        [Fact]
        public async Task CheckLimit_Failure_LimitExceeded()
        {
            // Arrange - Prepare Resource
            var checkLimitResult = new CheckLimitResult
            {
                IsLimitExceeded = true,
                Succeeded = true,
                StatusCode = "200"
            };

            var httpCallerMock = new Mock<IHttpCaller>();
            httpCallerMock.Setup(x => x.Get<CheckLimitResult>(It.IsAny<Action<HttpCallOptions>>())).ReturnsAsync(checkLimitResult);

            var limitServiceConfigMock = new Mock<LimitServiceConfig>();

            var limitService = new LimitService(limitServiceConfigMock.Object, httpCallerMock.Object);

            var ex = await Assert.ThrowsAsync<BusinessException>(() => limitService.CheckLimit(new CheckLimitRequest { UserId = Guid.NewGuid() }));

            Assert.Equal("LimitExceeded", ex.Message);
        }

        [Fact]
        public async Task CheckLimit_Failure_CallFailed()
        {
            // Arrange - Prepare Resource
            var checkLimitResult = new CheckLimitResult
            {
                IsLimitExceeded = false,
                Succeeded = false,
                StatusCode = "400"
            };

            var httpCallerMock = new Mock<IHttpCaller>();
            httpCallerMock.Setup(x => x.Get<CheckLimitResult>(It.IsAny<Action<HttpCallOptions>>())).ReturnsAsync(checkLimitResult);

            var limitServiceConfigMock = new Mock<LimitServiceConfig>();

            var limitService = new LimitService(limitServiceConfigMock.Object, httpCallerMock.Object);

            var ex = await Assert.ThrowsAsync<BusinessException>(() => limitService.CheckLimit(new CheckLimitRequest { UserId = Guid.NewGuid() }));

            Assert.Equal("LimitServiceIntegrationError", ex.Message);
        }
    }
}
