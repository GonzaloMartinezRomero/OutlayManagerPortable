using OutlayManagerPortable.Models;
using OutlayManagerPortable.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace OutlayManagerPortable.Services.Implementation
{
    public class TransactionOutlayAPIService : ITransactionOutlayService
    {
        public IEnumerable<string> Codes()
        {
            return new string[] { "AGUA", "COMIDA", "OTROS","SALIR" };
        }

        public IEnumerable<TransactionOutlay> LoadTransactions(DateTime date)
        {
            return new List<TransactionOutlay>()
            {
                new TransactionOutlay()
                {
                    ID=1,
                    Amount=15.33,
                    Code="COMIDA",
                    Type="SPENDING",
                    Description="My descripcion"
                }
            };
        }

        public IEnumerable<string> Operations()
        {
            return new string[] { "SPENDING", "INCOMING" };
        }
    }
}
