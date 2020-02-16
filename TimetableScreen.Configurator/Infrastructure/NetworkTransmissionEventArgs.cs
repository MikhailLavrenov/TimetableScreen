using System;

namespace TimetableScreen.Configurator.Infrastructure
{
    public class NetworkTransmissionEventArgs : EventArgs
    {
        public Operation Operation { get; set; }
        public Type ObjectType { get; set; }
        public object Object { get; set; }
        public int? RequestId { get; set; }

        public NetworkTransmissionEventArgs()
        { }

        public NetworkTransmissionEventArgs(Operation operation, Type objectType)
        {
            Operation = operation;
            ObjectType = objectType;
        }
    }
}
