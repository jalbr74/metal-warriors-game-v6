using System;
using Godot;
using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriors.Objects.Characters.ParkedNitro;
using MetalWarriors.Objects.Characters.Pilot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.HumanPlayer;

public partial class HumanPlayer : Node2D, IHumanPlayer
{
    public ISnesController Controller { get; set; } = new SnesController();
    private RemoteTransform2D _cameraMover;
    private Node2D _playerAvatar;

    private PackedScene _nitroPackedScene;
    private PackedScene _parkedNitroPackedScene;
    private PackedScene _pilotPackedScene;
    
    private NullSnesController _nullSnesController = new ();

    public override void _Ready()
    {
        _nitroPackedScene = (PackedScene)ResourceLoader.Load(IResourceConstants.NitroScene);
        _parkedNitroPackedScene = (PackedScene)ResourceLoader.Load(IResourceConstants.ParkedNitroScene);
        _pilotPackedScene = (PackedScene)ResourceLoader.Load(IResourceConstants.PilotScene);
        
        _cameraMover = GetNode<RemoteTransform2D>("CameraMover");
        
        GD.Print("HumanPlayer ready, creating a Nitro instance.");
        
        _playerAvatar = _nitroPackedScene.Instantiate<Nitro>();
        
        ((Nitro)_playerAvatar).Controller = Controller;
        
        AddChild(_playerAvatar);
    }
    
    public override void _Process(double delta)
    {
        if (Controller.WasSelectPressed)
        {
            switch (_playerAvatar)
            {
                case Nitro nitro:
                {
                    var parkedNitro = _parkedNitroPackedScene.Instantiate<ParkedNitro>();
                    parkedNitro.Position = nitro.Position;
                    AddChild(parkedNitro);
                
                    var newPilot = _pilotPackedScene.Instantiate<Pilot>();
                    newPilot.Position = nitro.Position;
                    newPilot.Controller = Controller;
                    AddChild(newPilot);

                    _playerAvatar = newPilot;

                    nitro.Controller = _nullSnesController;
                    nitro.QueueFree();
                    break;
                }
                case Pilot pilot:
                {
                    var detectedMech = pilot.GetDetectedMech();
                    
                    if (detectedMech is ParkedNitro parkedNitro)
                    {
                        parkedNitro.ReceivePilot(pilot, nitro => _playerAvatar = nitro);
                    }
                    
                    break;
                }
            }
        }
        
        // Update the camera position based on the Nitro instance
        _cameraMover.Position = _playerAvatar.Position;
    }
}
