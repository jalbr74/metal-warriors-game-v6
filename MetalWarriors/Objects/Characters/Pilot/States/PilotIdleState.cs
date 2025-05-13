using System;

namespace MetalWarriors.Objects.Characters.Pilot.States;

public class PilotIdleState(IPilotCharacter pilot) : BasePilotState(pilot)
{
    public override void Enter()
    {
        pilot.Console.Print("Entering Idle State");
    }

    public override bool ShouldTransitionToAnotherState(out Type otherState)
    {
        otherState = null;
        return false;
    }

    public override void PhysicsProcess(double delta)
    {
    }
}
