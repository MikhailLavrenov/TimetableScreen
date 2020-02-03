using Prism.Commands;
using Prism.Mvvm;
using System.Threading.Tasks;
using TimetableScreen.Infrastructure;
using TimetableScreen.Models;

namespace TimetableScreen.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        string clientText;
        string serverText;
        NetworkTransport client;
        NetworkTransport server;
        Task task;


        public string ClientText { get => clientText; set => SetProperty(ref clientText, value); }
        public string ServerText { get => serverText; set => SetProperty(ref serverText, value); }

        public DelegateCommand ClientSendCommand { get; }

        public ShellViewModel()
        {
            client = new NetworkTransport();
            server = new NetworkTransport();

            server.DataRecieved += DataRecievedHandler;
            task = Task.Run(() => server.Recieve());


            ClientSendCommand = new DelegateCommand(ClientSendExecute);

        }

        private void ClientSendExecute()
        {
            client.Send(clientText.GetBytes());
        }
        private void DataRecievedHandler(object obj, ResponseEventArgs args)
        {
            serverText = args.Buffer.ToString();
        }
    }
}
