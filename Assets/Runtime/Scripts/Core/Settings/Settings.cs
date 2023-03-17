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

        // Camera
        public static readonly BooleanParam SmoothCameraZoom = new(nameof(SmoothCameraZoom), true);
        public static readonly BooleanParam InvertZoom = new(nameof(InvertZoom), false);
        public static readonly BooleanParam InvertXSlide = new(nameof(InvertXSlide), false);
        public static readonly BooleanParam InvertYSlide = new(nameof(InvertYSlide), false);
        public static readonly BooleanParam InvertXRotation = new(nameof(InvertXRotation), false);
        public static readonly BooleanParam InvertYRotation = new(nameof(InvertYRotation), true);

        // Gameplay:
        public static readonly IntRangeParam FoV = new(nameof(FoV), 90, min: 30, max: 120);

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
