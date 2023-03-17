using UnityEngine;
using static Dark.SettingsManager;

public class Keybinds
{
    // Gameplay:
    // Camera Movements:
    public static readonly EnumParam<KeyCode> MoveUp = new(nameof(MoveUp), KeyCode.W);
    public static readonly EnumParam<KeyCode> MoveDown = new(nameof(MoveDown), KeyCode.S);
    public static readonly EnumParam<KeyCode> MoveRight = new(nameof(MoveRight), KeyCode.A);
    public static readonly EnumParam<KeyCode> MoveLeft = new(nameof(MoveLeft), KeyCode.D);
    public static readonly EnumParam<KeyCode> CameraRotationKey = new(nameof(CameraRotationKey), KeyCode.Mouse1);
    public static readonly EnumParam<KeyCode> CameraSlideKey = new(nameof(CameraSlideKey), KeyCode.Mouse2);

    // Chat settings:
    public static readonly EnumParam<KeyCode> ExtendChat = new(nameof(ExtendChat), KeyCode.Tilde);
    public static readonly EnumParam<KeyCode> ExtendConsole = new(nameof(ExtendConsole), KeyCode.BackQuote);
}