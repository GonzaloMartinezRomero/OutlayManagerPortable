using OutlayManagerPortableMaui.Models.Dto;

namespace OutlayManagerPortableMaui.Models
{
    internal static class TransactionMasterValues
    {
        private static readonly Dictionary<int,TransactionType> transactionTypes = new Dictionary<int, TransactionType>()
        {
            {1,new TransactionType(){ Id=1, Code="INCOMING" } },
            {2,new TransactionType(){ Id=2, Code="SPENDING" } } 
        };

        private static readonly Dictionary<int,TransactionCode> transactionCodes = new Dictionary<int, TransactionCode>()
        {
            {2,new TransactionCode(){ Id=2, Code="ALQUILER" }},
            {4,new TransactionCode(){ Id=4, Code="GAS" }},
            {5,new TransactionCode(){ Id=5, Code="MOVIL" }},
            {6,new TransactionCode(){ Id=6, Code="OCIO" }},
            {7,new TransactionCode(){ Id=7, Code="TRANSPORTE" }},
            {8,new TransactionCode(){ Id=8, Code="COMIDA" }},
            {9,new TransactionCode(){ Id=9, Code="AGUA" }},
            {10,new TransactionCode(){ Id=10, Code="LUZ" }},
            {12,new TransactionCode(){ Id=12, Code="NOMINA" }},
            {13,new TransactionCode(){ Id=13, Code="PELADO" }},
            {15,new TransactionCode(){ Id=15, Code="GYM" }},
            {20,new TransactionCode(){ Id=20, Code="OTRO" }},
            {21,new TransactionCode(){ Id=21, Code="RENTA" }},
            {22,new TransactionCode(){ Id=22, Code="COMPRA_ONLINE" }},
            {23,new TransactionCode(){ Id=23, Code="AJUSTE" }},
            {24,new TransactionCode(){ Id=24, Code="FARMACIA" }},
            {25,new TransactionCode(){ Id=25, Code="ROPA" }},
            {26,new TransactionCode(){ Id=26, Code="HACIENDA" }},
            {27,new TransactionCode(){ Id=27, Code="FERRETERIA" }},
            {28,new TransactionCode(){ Id=28, Code="INVERSION" }},
        };

        public static Dictionary<int,TransactionType> TransactionTypes { get { return transactionTypes; } }

        public static Dictionary<int, TransactionCode> TransactionCodes { get { return transactionCodes; } }
    }
}
