using MetalWarriors.Objects.Characters.Pilot.States;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Pilot.States;

public class PilotWalkingStateTest(ITestOutputHelper output) : BasePilotStateTest(output)
{
    [Fact]
    public void TestPilotWalkingState()
    {
        // Arrange

        // Act
        var passedToState = new PilotWalkingState(PilotCharacter).ProcessOrPass(0.1f);

        // Assert
        passedToState.ShouldBe(typeof(PilotIdleState));
    }
}
