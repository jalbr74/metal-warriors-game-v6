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

    public override void _Ready()
    {
        _nitroPackedScene = (PackedScene)ResourceLoader.Load("res://Objects/Characters/Nitro/Nitro.tscn");
        _parkedNitroPackedScene = (PackedScene)ResourceLoader.Load("res://Objects/Characters/ParkedNitro/ParkedNitro.tscn");
        _pilotPackedScene = (PackedScene)ResourceLoader.Load("res://Objects/Characters/Pilot/Pilot.tscn");
        
        _cameraMover = GetNode<RemoteTransform2D>("CameraMover");
        
        GD.Print("HumanPlayer ready, creating a Nitro instance.");
        
        _playerAvatar = _nitroPackedScene.Instantiate<Nitro>();
        
        ((Nitro)_playerAvatar).Controller = Controller;
        
        AddChild(_playerAvatar);
    }
    
    public override void _Process(double delta)
    {
        if (Controller.IsSelectPressed)
        {
            GD.Print("Select button pressed.");

            if (_playerAvatar is Nitro nitro)
            {
                GD.Print("Creating a ParkedNitro instance.");
                
                var parkedNitro = _parkedNitroPackedScene.Instantiate<ParkedNitro>();
                parkedNitro.Position = nitro.Position;
                AddChild(parkedNitro);
                
                var pilot = _pilotPackedScene.Instantiate<Pilot>();
                pilot.Position = nitro.Position;
                AddChild(pilot);

                _playerAvatar = pilot;
                
                nitro.QueueFree();
            }
        }
        
        // Update the camera position based on the Nitro instance
        _cameraMover.Position = _playerAvatar.Position;
    }
}
