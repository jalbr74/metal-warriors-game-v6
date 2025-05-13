using MetalWarriors.Objects.Characters.Pilot.States;
using MetalWarriors.Utils;
using MetalWarriorsTests.Utils;
using NSubstitute;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Pilot.States;

public class BasePilotStateTest
{
    protected readonly ISnesController Controller = Substitute.For<ISnesController>();
    protected readonly PilotCharacterImplForTesting PilotCharacter = new();

    protected BasePilotStateTest(ITestOutputHelper testOutputHelper)
    {
        PilotCharacter.Controller = Controller;
        PilotCharacter.Console = new TestOutputConsolePrinter(testOutputHelper);
    }
}
