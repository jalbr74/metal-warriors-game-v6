using MetalWarriors.Objects.Characters.Nitro.States;
using MetalWarriors.Utils;
using MetalWarriorsTests.Utils;
using NSubstitute;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Nitro.States;

public class BaseNitroStateTest
{
    protected readonly ISnesController Controller = Substitute.For<ISnesController>();
    protected readonly NitroCharacterImplForTesting NitroCharacter = new();
    protected readonly StateMachine StateMachine;

    protected BaseNitroStateTest(ITestOutputHelper testOutputHelper)
    {
        NitroCharacter.Controller = Controller;
        NitroCharacter.Console = new TestOutputConsolePrinter(testOutputHelper);
        
        StateMachine = new StateMachine(new Dictionary<string, State>
        {
            {"idle", new NitroIdleState(NitroCharacter)},
            {"walking", new NitroWalkingState(NitroCharacter)},
            {"launching", new NitroLaunchingState(NitroCharacter)},
            {"falling", new NitroFallingState(NitroCharacter)},
            {"flying", new NitroFlyingState(NitroCharacter)},
            {"landing", new NitroLandingState(NitroCharacter)},
        }, "idle");
    }
}
