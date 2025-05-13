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

    protected BaseNitroStateTest(ITestOutputHelper testOutputHelper)
    {
        NitroCharacter.Controller = Controller;
        NitroCharacter.Console = new TestOutputConsolePrinter(testOutputHelper);
    }
}
