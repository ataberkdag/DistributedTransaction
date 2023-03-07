namespace Order.Application
{
    public enum BusinessExceptionCodes
    {
        Succeeded = 0,
        UserInactive = 1000,
        OrderNotFound = 1001,
        OrderProcessed = 1002,
        IntegrationEventError = 8888
    }
}
