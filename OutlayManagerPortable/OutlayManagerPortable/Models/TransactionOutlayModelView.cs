using System;
using System.Collections.Generic;
using System.Text;

namespace OutlayManagerPortable.Models
{
    public sealed class TransactionOutlayModelView
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Amount { get; set; }
        public TransactionCodeModelView Code { get; set; }
        public TransactionTypeModelView Type { get; set; }
        public string Description { get; set; }
    }
}
