using Godot;
using MetalWarriors.Objects.Characters.Pilot.States;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Pilot;

public partial class Pilot : CharacterBody2D, IPilotCharacter
{
    public ISnesController Controller { get; set; }
    public IConsolePrinter Console { get; set; }
    public string CurrentAnimation { get; }
    public int CurrentAnimationFrame { get; }
    public bool IsAnimationFinished { get; set; }
    public CharacterDirection Direction { get; set; }
    public bool OnFloor { get; }
    
    private StateMachine _stateMachine;

    public override void _Ready()
    {
        _stateMachine = new StateMachine([
            new PilotIdleState(this)
        ], typeof(PilotIdleState));
    }

    public void PlayAnimation(string animation)
    {
        throw new System.NotImplementedException();
    }

    public void PauseAnimation()
    {
        throw new System.NotImplementedException();
    }
}
