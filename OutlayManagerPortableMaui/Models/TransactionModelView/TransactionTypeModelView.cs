using OutlayManagerPortableMaui.Models.Dto;

namespace OutlayManagerPortableMaui.Models.TransactionModelView
{
    public class TransactionTypeModelView : TransactionType
    {
        public TransactionTypeModelView(TransactionType transactionType)
        {
            Id = transactionType.Id;
            Code = transactionType.Code;
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
            if (obj is TransactionTypeModelView transactionTypeModelView)
            {
                return transactionTypeModelView.Id == Id;
            }

            return false;
        }
    }
}
