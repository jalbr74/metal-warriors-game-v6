﻿using Godot;

namespace MetalWarriors.Utils;

public class SnesController : ISnesController
{
    public bool IsDPadLeftPressed => Input.IsActionPressed("D_Pad_Left");
    public bool IsDPadRightPressed => Input.IsActionPressed("D_Pad_Right");
    public bool IsButtonBPressed => Input.IsActionPressed("Button_B");
    public bool IsSelectPressed => Input.IsActionPressed("Select");
}

public class NullSnesController : ISnesController
{
    public bool IsDPadLeftPressed => false;
    public bool IsDPadRightPressed => false;
    public bool IsButtonBPressed => false;
    public bool IsSelectPressed => false;
}
