using System;
using Godot;

namespace MetalWarriors.Objects.Characters.Pilot.States;

public class PilotIdleState(IPilotCharacter pilot) : BasePilotState(pilot)
{
    public override void Enter()
    {
        pilot.PlayAnimation("idle");
    }
    
    public override Type ProcessOrPass(double delta)
    {
        if (pilot.Controller.IsDPadLeftPressed || pilot.Controller.IsDPadRightPressed)
        {
            return typeof(PilotWalkingState);
        }

        if (!pilot.OnFloor)
        {
            return typeof(PilotFallingState);
        }
        
        pilot.Velocity = Vector2.Zero;
        
        return null;
    }
}
