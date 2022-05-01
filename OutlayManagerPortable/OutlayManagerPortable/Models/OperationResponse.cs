using System;
using System.Collections.Generic;
using System.Text;

namespace OutlayManagerPortable.Models
{
    public enum OperationStatus
    {
        OK,ERROR
    }

    public class OperationResponse
    {
        public OperationStatus OperationStatus { get; set; }
        public string Message { get; set; }
    }
}
