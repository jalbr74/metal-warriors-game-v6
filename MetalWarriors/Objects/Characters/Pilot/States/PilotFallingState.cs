using System;

namespace MetalWarriors.Objects.Characters.Pilot.States;

public class PilotFallingState(IPilotCharacter pilot) : BasePilotState(pilot)
{
    public override void Enter()
    {
    }

    public override Type? ProcessOrPass(double delta)
    {
        return null;
    }
}
