using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IAddCustomerMessage : IMessageBase
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string PublishedDateTime { get; set; }
    }
}
