using System;

namespace MetalWarriors.Objects.Characters.Pilot.States;

public class PilotFallingState(IPilotCharacter pilot) : BasePilotState(pilot)
{
    public override void Enter()
    {
        pilot.Console.Print($"Entering state: {GetType().Name}");
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
