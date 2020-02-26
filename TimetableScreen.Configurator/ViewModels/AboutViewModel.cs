using Prism.Mvvm;
using Prism.Regions;
using System.Linq;
using System.Reflection;

namespace TimetableScreen.Configurator.ViewModels
{
    class AboutViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive { get => true; }
        public string Name { get; }
        public string Version { get; }
        public string Copyright { get; }
        public string Author { get; }
        public string Email { get; }
        public string Phone { get; }

        public AboutViewModel()
        {
            var assembly = Assembly.GetExecutingAssembly();

            Name = ((AssemblyProductAttribute)assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false).First()).Product;
            Version = assembly.GetName().Version.ToString();
            Copyright = @"©  2020";
            Author = "Лавренов Михаил Владимирович";
            Email = "mvlavrenov@mail.ru";
            Phone = "8-924-213-79-11";
        }
    }
}
