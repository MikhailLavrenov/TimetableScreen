using Prism.Mvvm;
using System;
using System.Windows.Media;
using System.Xml.Serialization;

namespace TimetableScreen.Configurator.Models
{
    [Serializable]
    public class ColorPair : BindableBase
    {
        Color color1 = Colors.Gray;
        Color color2 = Colors.Gray;

        public Color Color1
        {
            get => color1;
            set
            {           
                SetProperty(ref color1, value);

                RaisePropertyChanged(nameof(Brush1));
                RaisePropertyChanged(nameof(Gradient));
            }
        }
        public Color Color2
        {
            get => color2;
            set
            {
                SetProperty(ref color2, value);

                RaisePropertyChanged(nameof(Brush2));
                RaisePropertyChanged(nameof(Gradient));
            }
        }

        [XmlIgnore] public SolidColorBrush Brush1 { get => new SolidColorBrush(Color1); }
        [XmlIgnore] public SolidColorBrush Brush2 { get => new SolidColorBrush(Color2); }

        [XmlIgnore] public LinearGradientBrush Gradient { get => new LinearGradientBrush(Color1, Color2, 0); }

    }
}
