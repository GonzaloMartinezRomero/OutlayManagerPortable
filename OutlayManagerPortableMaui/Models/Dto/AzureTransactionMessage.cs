namespace OutlayManagerPortableMaui.Models.Dto
{
    public class AzureTransactionMessage: TransactionMessage
    {
        public AzureTransactionMessage(TransactionMessage transactionMessage)
        {
            this.Id = transactionMessage.Id;
            this.Date = transactionMessage.Date;    
            this.CodeID = transactionMessage.CodeID;    
            this.TypeID = transactionMessage.TypeID;    
            this.Description = transactionMessage.Description;
            this.Amount = transactionMessage.Amount;    
        }

        public string PopReceipt { get; set; }
    }
}
