using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;

namespace Common.Messages
{
    public class DeleteCustomerMessage : IDeleteCustomerMessage
    {
        public int CustomerId { get; set; }
    }
}
