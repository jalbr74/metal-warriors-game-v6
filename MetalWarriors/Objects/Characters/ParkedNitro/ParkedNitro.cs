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

    public override void _Ready()
    {
        Animations = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public void PlayAnimation(string animation)
    {
        if (Animations.Animation == animation) return;

        Animations.Play(animation);
    }
}
