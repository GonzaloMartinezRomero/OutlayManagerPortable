using OutlayManagerPortable.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace OutlayManagerPortable.Models
{
    public class TransactionCodeModelView: TransactionCode
    {
        public TransactionCodeModelView(TransactionCode transactionCode)
        {
            this.Id = transactionCode.Id;
            this.Code = transactionCode.Code;
        }

        public override string ToString()
        {
            return base.Code;
        }

        public override int GetHashCode()
        {
            return base.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is TransactionCodeModelView transactionCodeModelView)
            {
                return transactionCodeModelView.Id == this.Id;
            }

            return false;
        }
    }
}
