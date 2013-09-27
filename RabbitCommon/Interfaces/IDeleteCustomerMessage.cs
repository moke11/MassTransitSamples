namespace RabbitCommon.Interfaces
{
    public interface IDeleteCustomerMessage : IMessageBase
    {
        int CustomerId { get; set; }
    }
}
