using System.Collections.Generic;
using System.Linq;
using Godot;
using MetalWarriors.Objects.Characters.Pilot.States;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Pilot;

public partial class Pilot : CharacterBody2D, IPilotCharacter
{
    public ISnesController Controller { get; set; } = NullSnesController.Instance;
    public IConsolePrinter Console { get; set; } = new ConsolePrinter();
    public AnimatedSprite2D Animations { get; set; }
    public Area2D ParkedMechDetector { get; set; }
    
    public string CurrentAnimation { get; }
    public int CurrentAnimationFrame { get; }
    public bool IsAnimationFinished { get; set; }
    public bool OnFloor { get; }
    
    private List<Node2D> _detectedCollidableMechs = new ();
    
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
        ParkedMechDetector = GetNode<Area2D>("ParkedMechDetector");
    }
    
    public override void _PhysicsProcess(double delta)
    {
        _stateMachine.PhysicsProcess(delta);
        MoveAndSlide();
    }

    public void PlayAnimation(string animation)
    {
        Animations.Play(animation);
    }

    public void PauseAnimation()
    {
    }

    public void CollidableMechEntered(Node2D collidableMech)
    {
        GD.Print("CollidableMechEntered");
        
        _detectedCollidableMechs.Add(collidableMech);
    }
    
    public void CollidableMechExited(Node2D collidableMech)
    {
        GD.Print("CollidableMechExited");
        
        _detectedCollidableMechs.Remove(collidableMech);
    }

    public Node2D GetDetectedMech()
    {
        return _detectedCollidableMechs.FirstOrDefault();
    }
}
