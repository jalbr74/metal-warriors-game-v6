using System;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroPoweringDownState(INitroCharacter nitro) : BaseNitroState(nitro)
{
    public override void Enter()
    {
    }
    
    public override Type? ProcessOrPass(double delta)
    {
        return null;
    }
}
