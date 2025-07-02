using MetalWarriors.Objects.Characters.Pilot;
using MetalWarriors.Utils;
using MetalWarriorsTests.Utils;
using NSubstitute;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Pilot.States;

public class BasePilotStateTest
{
    protected readonly IPilotCharacter PilotCharacter = Substitute.For<IPilotCharacter>();
    protected readonly ISnesController Controller = Substitute.For<ISnesController>();

    protected BasePilotStateTest(ITestOutputHelper testOutputHelper)
    {
        PilotCharacter.Controller.Returns(Controller);
        PilotCharacter.Console.Returns(new TestOutputConsolePrinter(testOutputHelper));
    }
}
