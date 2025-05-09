using Godot;
using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.HumanPlayer;

public partial class HumanPlayer : Node2D, IHumanPlayer
{
    public ISnesController Controller { get; set; } = new SnesController();
    private RemoteTransform2D _cameraMover;
    private Node2D _playerAvatar;

    public override void _Ready()
    {
        _cameraMover = GetNode<RemoteTransform2D>("CameraMover");
        
        GD.Print("HumanPlayer ready, creating a Nitro instance.");
        
        var packedScene = (PackedScene)ResourceLoader.Load("res://Objects/Characters/Nitro/Nitro.tscn");
        _playerAvatar = packedScene.Instantiate<Nitro>();
        
        ((Nitro)_playerAvatar).Controller = Controller;
        
        AddChild(_playerAvatar);
    }
    
    public override void _Process(double delta)
    {
        if (Controller.IsSelectPressed)
        {
            GD.Print("Select button pressed.");
        }
        
        // Update the camera position based on the Nitro instance
        _cameraMover.Position = _playerAvatar.Position;
    }
}
