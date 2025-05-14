using System;

namespace MetalWarriors.Objects.Characters.ParkedNitro.States;

public class ParkedNitroBeforeEnteringState(IParkedNitroCharacter parkedNitro) : BaseParkedNitroState(parkedNitro)
{
    public override bool ShouldTransitionToAnotherState(out Type otherState)
    {
        otherState = typeof(ParkedNitroExitingState);
        return true;
    }
}
