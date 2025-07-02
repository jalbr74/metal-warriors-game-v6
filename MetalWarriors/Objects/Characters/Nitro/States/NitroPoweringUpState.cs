using System;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroPoweringUpState(INitroCharacter nitro) : BaseNitroState(nitro)
{
    public override void Enter()
    {
        nitro.Console.Print($"Entering state: {GetType().Name}");
        
        nitro.PlayAnimation("powering-up");
    }
    
    public override Type? ProcessOrPass(double delta)
    {
        return null;
    }
}
