using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ElectronicObserver.Utility
{
    class ThemeManager
    {
        private static Dictionary<Tuple<Theme, ThemeColors>, Color> colors;

        static ThemeManager()
        {
            colors = new Dictionary<Tuple<Theme, ThemeColors>, Color>();
            colors.Add(Tuple.Create(Theme.Dark, ThemeColors.MainFontColor), SystemColors.Control);
            colors.Add(Tuple.Create(Theme.Dark, ThemeColors.SubFontColor), SystemColors.ControlDark);
            colors.Add(Tuple.Create(Theme.Dark, ThemeColors.BackgroundColor), Color.FromArgb(0x22, 0x22, 0x22));
            colors.Add(Tuple.Create(Theme.Dark, ThemeColors.RedHighlight), Color.IndianRed);
            colors.Add(Tuple.Create(Theme.Dark, ThemeColors.OrangeHighlight), Color.FromArgb(0xE5, 0x68, 0));
            colors.Add(Tuple.Create(Theme.Dark, ThemeColors.YellowHighlight), Color.FromArgb(0xFF, 0xCC, 0x00));
            colors.Add(Tuple.Create(Theme.Dark, ThemeColors.GreenHighlight), Color.DarkGreen);
            colors.Add(Tuple.Create(Theme.Dark, ThemeColors.GrayHighlight), Color.Silver);
            colors.Add(Tuple.Create(Theme.Dark, ThemeColors.PinkHighlight), Color.LightCoral);
            colors.Add(Tuple.Create(Theme.Dark, ThemeColors.MVPHighlight), Color.FromArgb(0xCC, 0xB6, 0x90));
            colors.Add(Tuple.Create(Theme.Dark, ThemeColors.RepairColor), Color.LightBlue);
            colors.Add(Tuple.Create(Theme.Dark, ThemeColors.BlackFontColor), Color.White);
        }

        public static Color GetColor(Theme th, ThemeColors tr)
        {
            return colors[Tuple.Create(th, tr)];
        }
    }

    [Serializable]
    public enum Theme
    {
        Dark = 1 //,
        //Pink = 2
    }

    [Serializable]
    public enum ThemeColors
    {
        MainFontColor,
        SubFontColor,
        BackgroundColor,
        RedHighlight,
        OrangeHighlight,
        YellowHighlight,
        GreenHighlight,
        GrayHighlight,
        PinkHighlight,
        MVPHighlight,
        RepairColor,
        BlackFontColor
    }
}

