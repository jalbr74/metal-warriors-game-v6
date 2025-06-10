using System;
using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.ParkedNitro;

public partial class ParkedNitro : CharacterBody2D, IParkedNitroCharacter
{
    public AnimatedSprite2D Animations { get; set; }
    public string CurrentAnimation => Animations.Animation;
    
    public IConsolePrinter Console { get; set; } = new ConsolePrinter();
    public CharacterDirection Direction
    {
        set => Animations.Scale = new Vector2(value == CharacterDirection.FacingLeft ? -1 : 1, Animations.Scale.Y);
        get => Animations.Scale.X >= 0 ? CharacterDirection.FacingRight : CharacterDirection.FacingLeft;
    }
    
    private PackedScene _nitroPackedScene;
    private NullSnesController _nullSnesController = new ();

    public override void _Ready()
    {
        _nitroPackedScene = (PackedScene)ResourceLoader.Load(IResourceConstants.NitroScene);
        
        Animations = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public void PlayAnimation(string animation)
    {
        if (Animations.Animation == animation) return;

        Animations.Play(animation);
    }

    public void ReceivePilot(Pilot.Pilot pilot, Action<Nitro.Nitro> action)
    {
        // TODO: Move this stuff into the ParkedNitroEnteringState so we can play the animation before we free the pilot
        
        var nitro = _nitroPackedScene.Instantiate<Nitro.Nitro>();
        nitro.Position = Position;
        nitro.Controller = pilot.Controller;
        
        GetParent().AddChild(nitro);
        
        pilot.Controller = _nullSnesController;
        pilot.QueueFree();
        
        QueueFree();
        
        action(nitro);
    }
}
