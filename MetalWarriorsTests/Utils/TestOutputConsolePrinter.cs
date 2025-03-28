using MetalWarriors.Utils;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Utils;

public class TestOutputConsolePrinter(ITestOutputHelper testOutputHelper) : IConsolePrinter
{
    public void Print(string message)
    {
        testOutputHelper.WriteLine(message);
    }
}
