using System;
using System.Collections.Generic;
using System.Text;

namespace TimetableScreen.Configurator.Infrastructure
{
    public enum Operation : byte
    {
        serverMustRecieve = 1,
        serverMustSend = 2,
    }
}
