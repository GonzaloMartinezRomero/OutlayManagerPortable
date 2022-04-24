using OutlayManagerPortable.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OutlayManagerPortable.Services.Abstract
{
    public interface ITransactionOutlayService
    {
        IEnumerable<string> Codes();
        IEnumerable<string> Operations();
        IEnumerable<TransactionOutlay> LoadTransactions(DateTime date);

    }
}
