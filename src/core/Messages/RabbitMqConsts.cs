namespace Messages
{
    public static class RabbitMqConsts
    {
        public const string OrderPlacedQueueName = "OrderPlaced";
        public const string StockDecreasedQueueName = "StockDecrease";
        public const string StockFailedQueueName = "StockFailed";

        public const string OrderApplicationName = "Order";
        public const string StockApplicationName = "Stock";
    }
}
