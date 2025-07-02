using MetalWarriors.Objects.Characters.Nitro.States;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Nitro.States;

public class NitroFallingStateTest(ITestOutputHelper testOutputHelper) : BaseNitroStateTest(testOutputHelper)
{
    [Fact]
    public void TestTransitionToFlyingStateWhenButtonIsPressed()
    {
        // Arrange
        Controller.IsButtonBPressed.Returns(true);
        
        // Act
        var passedToState = new NitroFallingState(NitroCharacter).ProcessOrPass(0.1f);

        // Assert
        passedToState.ShouldBe(typeof(NitroFlyingState));
    }
    
    [Fact]
    public void TestTransitionToLandingStateWhenOnFloor()
    {
        // Arrange
        NitroCharacter.OnFloor.Returns(true);
        
        // Act
        var passedToState = new NitroFallingState(NitroCharacter).ProcessOrPass(0.1f);

        // Assert
        passedToState.ShouldBe(typeof(NitroLandingState));
    }
}
