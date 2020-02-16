using System;
using System.Collections.Generic;
using System.Text;

namespace TimetableScreen.Configurator.Infrastructure
{
    public enum Operation : byte
    {
        SendToServer = 1,
        RecieveFromServer = 2,
        TestConnection=3
    }
}
