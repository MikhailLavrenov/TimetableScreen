using System;
using System.Windows.Media;

namespace TimetableScreen.Infrastructure
{
    public class ColorPalette
    {
        public static Color[] colors500 = new Color[]{
            GetColor("#f44336"),
            GetColor("#E91E63"),
            GetColor("#9C27B0"),
            GetColor("#673AB7"),
            GetColor("#3F51B5"),
            GetColor("#2196F3"),
            GetColor("#03A9F4"),
            GetColor("#00BCD4"),
            GetColor("#009688"),
            GetColor("#4CAF50"),
            GetColor("#8BC34A"),
            GetColor("#CDDC39"),
            //GetColor("#FFEB3B"),
            GetColor("#FFC107"),
            GetColor("#FF9800"),
            GetColor("#FF5722"),
            GetColor("#795548"),
            //GetColor("#9E9E9E"),
            GetColor("#607D8B"),
        };

        public static Color[] colors400 = new Color[]{
            GetColor("#ef5350"),
            GetColor("#EC407A"),
            GetColor("#AB47BC"),
            GetColor("#7E57C2"),
            GetColor("#5C6BC0"),
            GetColor("#42A5F5"),
            GetColor("#29B6F6"),
            GetColor("#26C6DA"),
            GetColor("#26A69A"),
            GetColor("#66BB6A"),
            GetColor("#9CCC65"),
            GetColor("#D4E157"),
            //GetColor("#FFEE58"),
            GetColor("#FFCA28"),
            GetColor("#FFA726"),
            GetColor("#FF7043"),
            GetColor("#8D6E63"),
            //GetColor("#BDBDBD"),
            GetColor("#78909C"),
        };

        public static Color[] colors300 = new Color[]{
            GetColor("#e57373"),
            GetColor("#F06292"),
            GetColor("#BA68C8"),
            GetColor("#9575CD"),
            GetColor("#7986CB"),
            GetColor("#64B5F6"),
            GetColor("#4FC3F7"),
            GetColor("#4DD0E1"),
            GetColor("#4DB6AC"),
            GetColor("#81C784"),
            GetColor("#AED581"),
            GetColor("#DCE775"),
            //GetColor("#FFF176"),
            GetColor("#FFD54F"),
            GetColor("#FFB74D"),
            GetColor("#FF8A65"),
            GetColor("#A1887F"),
            //GetColor("#E0E0E0"),
            GetColor("#90A4AE"),
        };

        public static Color[] colors200 = new Color[]{
            GetColor("#ef9a9a"),
            GetColor("#F48FB1"),
            GetColor("#CE93D8"),
            GetColor("#B39DDB"),
            GetColor("#9FA8DA"),
            GetColor("#90CAF9"),
            GetColor("#81D4FA"),
            GetColor("#80DEEA"),
            GetColor("#80CBC4"),
            GetColor("#A5D6A7"),
            GetColor("#C5E1A5"),
            GetColor("#E6EE9C"),
            //GetColor("#FFF59D"),
            GetColor("#FFE082"),
            GetColor("#FFCC80"),
            GetColor("#FFAB91"),
            GetColor("#BCAAA4"),
            //GetColor("#EEEEEE"),
            GetColor("#B0BEC5"),
        };

        public static Color[] colors100 = new Color[]{
            GetColor("#ffebee"),
            GetColor("#FCE4EC"),
            GetColor("#F3E5F5"),
            GetColor("#EDE7F6"),
            GetColor("#E8EAF6"),
            GetColor("#E3F2FD"),
            GetColor("#E1F5FE"),
            GetColor("#E0F7FA"),
            GetColor("#E0F2F1"),
            GetColor("#E8F5E9"),
            GetColor("#F1F8E9"),
            GetColor("#F9FBE7"),
            //GetColor("#FFFDE7"),
            GetColor("#FFF8E1"),
            GetColor("#FFF3E0"),
            GetColor("#FBE9E7"),
            GetColor("#EFEBE9"),
            //GetColor("#FAFAFA"),
            GetColor("#ECEFF1"),
        };

        public static (Color, Color)[] gradientsColors = new[]
        {
            (GetColor("#29B6F6"),GetColor("#5C6BC0")),
            //(GetColor("#26C6DA"),GetColor("#26A69A")),
            //(GetColor("#26C6DA"),GetColor("#00695C")),
            //(GetColor("#"),GetColor("#"))
        };
            


        public static int ColorsCount;
        public static int GradientsCount;

        static ColorPalette()
        {
            ColorsCount = colors400.Length;
            GradientsCount = gradientsColors.Length;

            if (colors100.Length != colors200.Length || colors100.Length != colors300.Length || colors100.Length != colors400.Length || colors100.Length != colors500.Length)
                throw new InvalidOperationException("Наборы цветовой палитры имкют различные размеры.");
        }

        public static Color GetColor(string colorCode)
        {
            return (Color)ColorConverter.ConvertFromString(colorCode);
        }
    }



}
