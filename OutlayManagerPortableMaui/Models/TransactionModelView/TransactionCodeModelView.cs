using OutlayManagerPortableMaui.Models.Dto;

namespace OutlayManagerPortableMaui.Models.TransactionModelView
{
    public class TransactionCodeModelView : TransactionCode
    {
        public TransactionCodeModelView(TransactionCode transactionCode)
        {
            Id = transactionCode.Id;
            Code = transactionCode.Code;
        }

        public override string ToString()
        {
            return Code;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is TransactionCodeModelView transactionCodeModelView)
            {
                return transactionCodeModelView.Id == Id;
            }

            return false;
        }
    }
}
