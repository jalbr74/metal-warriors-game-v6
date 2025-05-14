using System;

namespace MetalWarriors.Objects.Characters.ParkedNitro.States;

public class ParkedNitroEnteringState(IParkedNitroCharacter parkedNitro) : BaseParkedNitroState(parkedNitro)
{
    public override void Enter()
    {
        // parkedNitro.Console.Print("Entering ParkedNitroEnteringState");
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
