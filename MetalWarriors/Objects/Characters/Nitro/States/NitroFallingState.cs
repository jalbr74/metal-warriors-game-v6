using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro.States;

public class NitroFallingState(ISnesController controller, INitroCharacter nitro, IConsolePrinter console) : BaseNitroState(controller, nitro, console)
{
    public override void Enter()
    {
        console.Print("Entering Idle State");
        
        nitro.PlayAnimation("walking");
        nitro.PauseAnimation();
    }
}
