using System;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroPoweringDownState(INitroCharacter nitro) : BaseNitroState(nitro)
{
    public override void Enter()
    {
        nitro.Console.Print($"Entering state: {GetType().Name}");
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
