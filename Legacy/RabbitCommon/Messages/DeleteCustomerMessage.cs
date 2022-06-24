using RabbitCommon.Interfaces;

namespace RabbitCommon.Messages
{
    public class DeleteCustomerMessage : IDeleteCustomerMessage
    {
        public int CustomerId { get; set; }
    }
}
