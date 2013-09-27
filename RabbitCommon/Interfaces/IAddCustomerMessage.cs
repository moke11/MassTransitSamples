namespace RabbitCommon.Interfaces
{
    public interface IAddCustomerMessage : IMessageBase
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string PublishedDateTime { get; set; }
    }
}
