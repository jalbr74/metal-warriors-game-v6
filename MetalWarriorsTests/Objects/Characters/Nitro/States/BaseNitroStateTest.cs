using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriors.Utils;
using MetalWarriorsTests.Utils;
using NSubstitute;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Nitro.States;

public class BaseNitroStateTest
{
    protected readonly INitroCharacter NitroCharacter = Substitute.For<INitroCharacter>();
    protected readonly ISnesController Controller = Substitute.For<ISnesController>();

    protected BaseNitroStateTest(ITestOutputHelper testOutputHelper)
    {
        NitroCharacter.Controller.Returns(Controller);
        NitroCharacter.Console.Returns(new TestOutputConsolePrinter(testOutputHelper));
    }
}
