using MetalWarriorsTests.Utils;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.ParkedNitro.States;

public class BaseParkedNitroStateTest
{
    protected readonly ParkedNitroCharacterImplForTesting ParkedNitroCharacter = new();

    protected BaseParkedNitroStateTest(ITestOutputHelper testOutputHelper)
    {
        ParkedNitroCharacter.Console = new TestOutputConsolePrinter(testOutputHelper);
    }
}
