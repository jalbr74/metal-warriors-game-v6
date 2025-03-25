using Godot;

namespace MetalWarriors.Utils;

public class SnesController : ISnesController
{
    public bool IsDPadLeftPressed => Input.IsActionPressed("D_Pad_Left");
    public bool IsDPadRightPressed => Input.IsActionPressed("D_Pad_Right");
    public bool IsButtonBPressed => Input.IsActionPressed("Button_B");
}
