using Prism.Mvvm;
using System;
using System.Windows.Media;
using System.Xml.Serialization;

namespace TimetableScreen.Configurator.Models
{
    [Serializable]
    public class ColorsPair : BindableBase
    {
        string hexColor1 = "#808080";
        string hexColor2 = "#808080";
        Color color1;
        Color color2;
        SolidColorBrush brush1;
        SolidColorBrush brush2;
        LinearGradientBrush gradient;

        public string HexColor1
        {
            get => hexColor1;
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length != 7 || value[0] != '#')
                    return;
                    
                SetProperty(ref hexColor1, value);

                var color= (Color)ColorConverter.ConvertFromString(value);
                Brush1 = new SolidColorBrush(color);

                Gradient=new LinearGradientBrush()
            }
        }
        public string HexColor2 { get => hexColor2; set => SetProperty(ref hexColor2, value); }

        [XmlIgnore] public SolidColorBrush Brush1 { get => brush1; set => SetProperty(ref brush1, value); }
        [XmlIgnore] public SolidColorBrush Brush2 { get => brush2; set => SetProperty(ref brush2, value); }

        [XmlIgnore] public LinearGradientBrush Gradient { get => gradient; set => SetProperty(ref gradient, value); }








        //public Color GetColor(string hex)
        //{
        //    Color color;

        //    if (string.IsNullOrEmpty(hex) || hex.Length != 7 || hex[0] != '#')
        //        color = Colors.Transparent;
        //    else
        //        color = (Color)ColorConverter.ConvertFromString(hex);

        //    return new SolidColorBrush;
        //}


    }
}
