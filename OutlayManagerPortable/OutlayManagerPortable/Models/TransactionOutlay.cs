using System;
using System.Collections.Generic;
using System.Text;

namespace OutlayManagerPortable.Models
{
    public sealed class TransactionOutlay
    {
        public int ID { get; set; }
        public double Amount { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }        
        public string Description { get; set; }
    }
}
