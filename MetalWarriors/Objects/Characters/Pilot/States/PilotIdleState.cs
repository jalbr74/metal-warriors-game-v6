using System;
using Godot;

namespace MetalWarriors.Objects.Characters.Pilot.States;

public class PilotIdleState(IPilotCharacter pilot) : BasePilotState(pilot)
{
    public override void Enter()
    {
        pilot.Console.Print($"Entering state: {GetType().Name}");
        
        pilot.PlayAnimation("idle");
    }

    public override bool ShouldTransitionToAnotherState(out Type otherState)
    {
        if (pilot.Controller.IsDPadLeftPressed || pilot.Controller.IsDPadRightPressed)
        {
            otherState = typeof(PilotWalkingState);
            return true;
        }
        
        otherState = null;
        return false;
    }

    public override void PhysicsProcess(double delta)
    {
        pilot.Velocity = Vector2.Zero;
    }
}
