namespace Order.Application
{
    public enum BusinessExceptionCodes
    {
        Succeeded = 0,
        LimitServiceIntegrationError = 1000,
        LimitExceeded = 1001,
        OrderNotFound = 2000,
        OrderProcessed = 2001,
        IntegrationEventError = 8888
    }
}
