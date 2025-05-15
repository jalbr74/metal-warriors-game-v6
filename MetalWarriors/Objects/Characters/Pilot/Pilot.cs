using Godot;
using MetalWarriors.Objects.Characters.Pilot.States;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Pilot;

public partial class Pilot : CharacterBody2D, IPilotCharacter
{
    public ISnesController Controller { get; set; } = new NullSnesController();
    public IConsolePrinter Console { get; set; } = new ConsolePrinter();
    public AnimatedSprite2D Animations { get; set; }
    
    public string CurrentAnimation { get; }
    public int CurrentAnimationFrame { get; }
    public bool IsAnimationFinished { get; set; }
    public bool OnFloor { get; }
    
    public CharacterDirection Direction
    {
        set => Animations.Scale = new Vector2(value == CharacterDirection.FacingLeft ? -1 : 1, Animations.Scale.Y);
        get => Animations.Scale.X >= 0 ? CharacterDirection.FacingRight : CharacterDirection.FacingLeft;
    }
    
    private StateMachine _stateMachine;

    public override void _Ready()
    {
        _stateMachine = new StateMachine([
            new PilotIdleState(this),
            new PilotWalkingState(this),
            new PilotJettingState(this),
        ], typeof(PilotIdleState));
        
        Animations = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
    
    public override void _PhysicsProcess(double delta)
    {
        _stateMachine.PhysicsProcess(delta);
        MoveAndSlide();
    }

    public void PlayAnimation(string animation)
    {
        
    }

    public void PauseAnimation()
    {
        
    }
}
