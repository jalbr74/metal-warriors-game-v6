using System;
using Godot;
using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriors.Objects.Characters.Pilot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.HumanPlayer;

public partial class HumanPlayer : Node2D, IHumanPlayer
{
    public ISnesController Controller { get; set; } = new SnesController();
    private RemoteTransform2D _cameraMover;
    private Node2D _activeAvatar;

    private PackedScene _nitroPackedScene;
    private PackedScene _pilotPackedScene;
    
    public override void _Ready()
    {
        _nitroPackedScene = (PackedScene)ResourceLoader.Load(IResourceConstants.NitroScene);
        _pilotPackedScene = (PackedScene)ResourceLoader.Load(IResourceConstants.PilotScene);
        
        _cameraMover = GetNode<RemoteTransform2D>("CameraMover");
        
        GD.Print("HumanPlayer ready, creating a Nitro instance.");
        
        _activeAvatar = _nitroPackedScene.Instantiate<Nitro>();
        
        ((Nitro)_activeAvatar).Controller = Controller;
        
        AddChild(_activeAvatar);
    }
    
    public override void _Process(double delta)
    {
        if (Controller.WasSelectPressed)
        {
            switch (_activeAvatar)
            {
                case Nitro nitro:
                {
                    nitro.PowerDown();
                    
                    var newPilot = _pilotPackedScene.Instantiate<Pilot>();
                    newPilot.Position = nitro.Position;
                    newPilot.Controller = Controller;
                    AddChild(newPilot);

                    _activeAvatar = newPilot;

                    break;
                }
                case Pilot pilot:
                {
                    var detectedMech = pilot.GetDetectedMech();
                    
                    if (detectedMech is Nitro parkedNitro)
                    {
                        parkedNitro.PowerUp(Controller);
                        _activeAvatar = parkedNitro;
                        
                        pilot.QueueFree();
                    }
                    
                    break;
                }
            }
        }
        
        // Update the camera position based on the Nitro instance
        _cameraMover.Position = _activeAvatar.Position;
    }
}
