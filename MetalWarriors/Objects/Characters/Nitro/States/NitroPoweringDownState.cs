using System;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroPoweringDownState(INitroCharacter nitro) : BaseNitroState(nitro)
{
    public override void Enter()
    {
        nitro.IsGunVisible = false;
        nitro.PlayAnimation("powering-down");
    }
    
    public override Type ProcessOrPass(double delta)
    {
        return null;
    }
}
