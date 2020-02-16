using System;
using System.Collections.Generic;
using System.Text;

namespace TimetableScreen.Configurator.Infrastructure
{
    public class NetworkTransmissionEventArgs : EventArgs
    {
        public Operation Procedure { get; set; }
        public Type ObjectType { get; set; }
        public object Object { get; set; }
        public int? RequestId { get; set; }
    }
}
