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
        var consolePrinter = new TestOutputConsolePrinter(testOutputHelper);
        
        StateMachine = new StateMachine(new Dictionary<string, State>
        {
            {"idle", new NitroIdleState(Controller, NitroCharacter, consolePrinter)},
            {"walking", new NitroWalkingState(Controller, NitroCharacter, consolePrinter)},
            {"launching", new NitroLaunchingState(Controller, NitroCharacter, consolePrinter)},
            {"falling", new NitroFallingState(Controller, NitroCharacter, consolePrinter)},
            {"flying", new NitroFlyingState(Controller, NitroCharacter, consolePrinter)},
        }, "idle", consolePrinter);
    }
}
