using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.KancolleProgress
{
    public enum Comparator
    {
        Equal,
        GreaterOrEqual
    }

    public class ColorFilter
    {
        private int _level;
        public List<IShipData> Ships { get; set; }

        public string Name { get; set; } = "";

        public string MatchCount => Comparator switch
        {
            Comparator.GreaterOrEqual => $"{Ships.Count(s => s.Level >= Level)}/{Ships.Count}",
            Comparator.Equal => $"{Ships.Count(s => s.Level == Level)}/{Ships.Count}",
            _ => "Unknown filter"
        };

        public int Level
        {
            get => _level;
            set
            {
                _level = value;

                Comparator = Level switch
                {
                    175 => Comparator.Equal,
                    0 => Comparator.Equal,
                    _ => Comparator.GreaterOrEqual
                };
            }
        }

        public Comparator Comparator { get; set; }
        public string Color => LevelColor(Level);

        public static string LevelColor(int level) => (level switch
        {
            175 => Colors.DeepPink,
            _ when level >= 99 => Colors.DeepSkyBlue,
            _ when level >= 90 => Colors.LimeGreen,
            _ when level >= 1 => Colors.Yellow,
            _ => Colors.Red
        }).ToString();
    }
}