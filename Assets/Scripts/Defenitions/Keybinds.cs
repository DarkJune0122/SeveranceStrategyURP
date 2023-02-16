using System;
using UnityEngine;

[Serializable]
public class Keybinds
{
    public static Keybinds Global { get; private set; } = new();

    public KeyCode MoveUp = KeyCode.W;
    public KeyCode MoveDown = KeyCode.S;
    public KeyCode MoveLeft = KeyCode.A;
    public KeyCode MoveRight = KeyCode.D;

    public KeyCode SubmitShape = KeyCode.Z;
    public KeyCode FillAndSubmit = KeyCode.X;
    public KeyCode RotateBounds = KeyCode.C;
    public KeyCode PassTurn = KeyCode.B;

    public KeyCode ExtendConsole = KeyCode.BackQuote;


    public void LoadKeybinds() { }
    // Добавьте код загрузки данных биндов ыклавиш с диска

    public void SaveKeybinds() { }
    // Добавьте код сохранения данных биндов клавиш на диске
}