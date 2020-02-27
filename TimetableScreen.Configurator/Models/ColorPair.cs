using Prism.Mvvm;
using System;
using System.Windows.Media;
using System.Xml.Serialization;

namespace TimetableScreen.Configurator.Models
{
    [Serializable]
    public class ColorPair : BindableBase
    {
        string hexColor1 = "#808080";
        string hexColor2 = "#808080";

        public string HexColor1
        {
            get => hexColor1;
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length != 7 || value[0] != '#')
                    return;

                SetProperty(ref hexColor1, value);

                RaisePropertyChanged(nameof(Brush1));
                RaisePropertyChanged(nameof(Gradient));
            }
        }
        public string HexColor2
        {
            get => hexColor2;
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length != 7 || value[0] != '#')
                    return;

                SetProperty(ref hexColor2, value);

                RaisePropertyChanged(nameof(Brush2));
                RaisePropertyChanged(nameof(Gradient));
            }
        }

        [XmlIgnore] public SolidColorBrush Brush1 { get => new SolidColorBrush(GetColor(HexColor1)); }
        [XmlIgnore] public SolidColorBrush Brush2 { get => new SolidColorBrush(GetColor(HexColor2)); }

        [XmlIgnore] public LinearGradientBrush Gradient { get => new LinearGradientBrush(GetColor(HexColor1), GetColor(HexColor2), 0); }

        public static Color GetColor(string hex)
        {
            return (Color)ColorConverter.ConvertFromString(hex);
        }


    }
}
