using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimetableScreen.Configurator.Models;

namespace TimetableScreen
{
    public class ShellViewModel:BindableBase
    {
        private NetworkTransport networkTransport;

        public Settings Settings { get; set; }

        public ShellViewModel(Settings settings)
        {
            Settings = settings;

            networkTransport = new NetworkTransport();
            networkTransport.DataRecieved += DataRecievedHandler;

            Task.Run(()=>networkTransport.BeginRecieve());
        }

        private void DataRecievedHandler(object oject, ResponseEventArgs args)
        {

        }


    }
}
