using Common.Interfaces;

namespace Common.Messages
{
    public class AddCustomerMessage : IAddCustomerMessage
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PublishedDateTime { get; set; }
    }
}
