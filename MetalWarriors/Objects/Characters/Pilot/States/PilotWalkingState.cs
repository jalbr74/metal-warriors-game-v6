using System;
using Godot;

namespace MetalWarriors.Objects.Characters.Pilot.States;

public class PilotWalkingState(IPilotCharacter pilot) : BasePilotState(pilot)
{
    public override void Enter()
    {
        pilot.Console.Print($"Entering state: {GetType().Name}");
        
        pilot.PlayAnimation("walking");
    }

    public override bool ShouldTransitionToAnotherState(out Type otherState)
    {
        if (!pilot.Controller.IsDPadLeftPressed && !pilot.Controller.IsDPadRightPressed)
        {
            otherState = typeof(PilotIdleState);
            return true;
        }
        
        otherState = null;
        return false;
    }

    public override void PhysicsProcess(double delta)
    {
        if (pilot.Controller.IsDPadLeftPressed)
        {
            pilot.Direction = CharacterDirection.FacingLeft;
            pilot.Velocity = new Vector2(-MovementSpeed, pilot.Velocity.Y);
        }
        else if (pilot.Controller.IsDPadRightPressed)
        {
            pilot.Direction = CharacterDirection.FacingRight;
            pilot.Velocity = new Vector2(MovementSpeed, pilot.Velocity.Y);
        }
    }
}
