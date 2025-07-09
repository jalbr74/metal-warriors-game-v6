using System;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroPoweringUpState(INitroCharacter nitro) : BaseNitroState(nitro)
{
    public override void Enter()
    {
        nitro.PlayAnimation("powering-up");
    }
    
    public override Type? ProcessOrPass(double delta)
    {
        return null;
    }
}
