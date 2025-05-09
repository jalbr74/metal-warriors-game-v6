using Godot;
using System;
using MetalWarriors.Utils;

public partial class Mission001 : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (OS.GetName() == "Android")
        {
            ControllerUtils.RemapControllerForAndroid();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
