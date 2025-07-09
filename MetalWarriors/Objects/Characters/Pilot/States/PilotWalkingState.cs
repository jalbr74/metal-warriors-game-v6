using System;
using Godot;

namespace MetalWarriors.Objects.Characters.Pilot.States;

public class PilotWalkingState(IPilotCharacter pilot) : BasePilotState(pilot)
{
    public override void Enter()
    {
        pilot.PlayAnimation("walking");
    }
    
    public override Type ProcessOrPass(double delta)
    {
        if (!pilot.Controller.IsDPadLeftPressed && !pilot.Controller.IsDPadRightPressed)
        {
            return typeof(PilotIdleState);
        }
        
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
        
        return null;
    }
}
