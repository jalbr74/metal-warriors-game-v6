using System;

namespace MetalWarriors.Objects.Characters.ParkedNitro.States;

public class ParkedNitroExitingState(IParkedNitroCharacter parkedNitro) : BaseParkedNitroState(parkedNitro)
{
    public override void Enter()
    {
        parkedNitro.Console.Print($"Entering state: {GetType().Name}");
        
        parkedNitro.PlayAnimation("exiting");
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
