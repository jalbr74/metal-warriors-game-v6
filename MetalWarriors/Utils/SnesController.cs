using Godot;

namespace MetalWarriors.Utils;

public class SnesController : ISnesController
{
    public bool IsDPadLeftPressed => Input.IsActionPressed("D_Pad_Left");
    public bool IsDPadRightPressed => Input.IsActionPressed("D_Pad_Right");
    public bool IsButtonBPressed => Input.IsActionPressed("Button_B");
    public bool IsSelectPressed => Input.IsActionPressed("Select");
    public bool WasSelectPressed => Input.IsActionJustPressed("Select");
}

public class NullSnesController : ISnesController
{
    private NullSnesController() { }
    public static NullSnesController Instance { get; } = new();
    
    public bool IsDPadLeftPressed => false;
    public bool IsDPadRightPressed => false;
    public bool IsButtonBPressed => false;
    public bool IsSelectPressed => false;
    public bool WasSelectPressed => false;
}
