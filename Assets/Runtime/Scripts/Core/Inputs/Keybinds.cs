using UnityEngine;
using static Dark.SettingsManager;

public class Keybinds
{
    // Gameplay:
    public static readonly EnumParam<KeyCode> MoveUp = new(nameof(MoveUp), KeyCode.W);
    public static readonly EnumParam<KeyCode> MoveDown = new(nameof(MoveDown), KeyCode.S);
    public static readonly EnumParam<KeyCode> MoveRight = new(nameof(MoveRight), KeyCode.A);
    public static readonly EnumParam<KeyCode> MoveLeft = new(nameof(MoveLeft), KeyCode.D);

    // Chat settings:
    public static readonly EnumParam<KeyCode> ExtendChat = new(nameof(ExtendChat), KeyCode.Tilde);
    public static readonly EnumParam<KeyCode> ExtendConsole = new(nameof(ExtendConsole), KeyCode.BackQuote);
}