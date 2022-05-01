using OutlayManagerPortable.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace OutlayManagerPortable.Models
{
    public class TransactionTypeModelView: TransactionType
    {
        public TransactionTypeModelView(TransactionType transactionType)
        {
            this.Id = transactionType.Id;
            this.Code = transactionType.Code;
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
            if(obj is TransactionTypeModelView transactionTypeModelView)
            {
                return transactionTypeModelView.Id == this.Id;
            }

            return false;
        }
    }
}
