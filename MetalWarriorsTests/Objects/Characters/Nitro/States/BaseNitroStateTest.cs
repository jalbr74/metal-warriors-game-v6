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
        
        StateMachine = new StateMachine([
            new NitroIdleState(NitroCharacter),
            new NitroWalkingState(NitroCharacter),
            new NitroLaunchingState(NitroCharacter),
            new NitroFallingState(NitroCharacter),
            new NitroFlyingState(NitroCharacter),
            new NitroLandingState(NitroCharacter),
        ], typeof(NitroIdleState));
    }
}
