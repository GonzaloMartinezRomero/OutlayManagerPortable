using OutlayManagerPortable.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlayManagerPortable.Infraestructure.Data.Abstraction
{
    public interface IMasterData
    {
        Task<IEnumerable<TransactionCode>> TransactionsCodes();

        Task<IEnumerable<TransactionType>> TransactionsTypes();
    }
}
