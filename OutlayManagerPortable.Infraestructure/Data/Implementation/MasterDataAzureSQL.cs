using Dapper;
using Microsoft.Extensions.Configuration;
using OutlayManagerPortable.DTO;
using OutlayManagerPortable.Infraestructure.Data.Abstraction;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace OutlayManagerPortable.Infraestructure.Data.Implementation
{
    internal class MasterDataAzureSQL : IMasterData
    {
        private const string CONNECTION_STRING_KEY = "MasterDB";
        private readonly string CONNECTION_STRING;

        private MasterDataAzureSQL() { }

        public MasterDataAzureSQL(IConfiguration configuration)
        {
            CONNECTION_STRING = configuration.GetConnectionString(CONNECTION_STRING_KEY) ?? throw new Exception($"{CONNECTION_STRING_KEY} not configured!");
        }

        public async Task<IEnumerable<TransactionCode>> TransactionsCodes()
        {
            const string queryTransactionCodes = "SELECT Id,Code FROM [dbo].[TransactionCode]";

            using var connection = new SqlConnection(CONNECTION_STRING);
            connection.Open();

            IEnumerable<TransactionCode> transactionsCode = await connection.QueryAsync<TransactionCode>(queryTransactionCodes);

            return transactionsCode;
        }

        public async Task<IEnumerable<TransactionType>> TransactionsTypes()
        {
            const string queryTransactionTypes = "SELECT Id,Code FROM [dbo].[TransactionType]";

            using var connection = new SqlConnection(CONNECTION_STRING);
            connection.Open();

            IEnumerable<TransactionType> transactionsTypes = await connection.QueryAsync<TransactionType>(queryTransactionTypes);

            return transactionsTypes;
        }
    }
}
