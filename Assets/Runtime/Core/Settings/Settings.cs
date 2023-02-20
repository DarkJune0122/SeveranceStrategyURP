using System.Collections.Generic;
using static Dark.SettingsManager;
using System.Linq;

namespace SeveranceStrategy
{
    public static partial class Settings
    {
        static Settings()
        {
            List<string> list = new();
            list.AddRange(typeof(Settings).GetFields().Select(field => ((AbstractParam)field.GetValue(null)).ToString()));
            list.LogEnumerable("Current settings:");
        }

        // Music & Sounds:
        public static readonly BooleanParam SFx = new(nameof(SFx), true);

        public static readonly BooleanParam UIAnimations = new(nameof(UIAnimations), true);
        public static readonly BooleanParam Animations = new(nameof(Animations), true);

        public static readonly BooleanParam InvertX = new(nameof(InvertX), false);
        public static readonly BooleanParam InvertY = new(nameof(InvertY), true);
        public static readonly BooleanParam InvertZoom = new(nameof(InvertZoom), true);

        // Gameplay:
        public static readonly IntRangeParam FoV = new(nameof(FoV), 90, min: 60, max: 120);

        // Advanced UI:
        public static readonly BooleanParam ShowSaveFilePath = new(nameof(ShowSaveFilePath), false);
    }

    public enum GraphicsLevel : byte
    {
        Lowest = 0,
        Low = 1,
        Normal = 2,
        High = 3,
        Best = 4
    }
}
