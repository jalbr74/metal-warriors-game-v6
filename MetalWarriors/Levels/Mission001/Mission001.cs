using Godot;
using System;
using MetalWarriors.Utils;

public partial class Mission001 : Node2D
{
    public MetalWarriors.Objects.Characters.Nitro.Nitro Nitro { get; set; }
    public Label VelocityLabel { get; set; }
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Nitro = GetNode<MetalWarriors.Objects.Characters.Nitro.Nitro>("Nitro");
        VelocityLabel = GetNode<Label>("DebugText/VelocityLabel");
        
        if (OS.GetName() == "Android")
        {
            ControllerUtils.RemapControllerForAndroid();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        VelocityLabel.Text = $"Velocity: {Nitro.Velocity}";
    }
}
