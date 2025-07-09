using Godot;
using MetalWarriors.Objects.Characters;
using MetalWarriors.Objects.Characters.Pilot.States;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Pilot.States;

public class PilotIdleStateTest(ITestOutputHelper output) : BasePilotStateTest(output)
{
    [Fact]
    public void TestTransitionToWalkingStateWhenButtonIsPressed()
    {
        // Arrange
        PilotCharacter.OnFloor.Returns(true);
        PilotCharacter.Velocity.Returns(new Vector2(100, 0));
        
        // Act
        var passedToState = new PilotIdleState(PilotCharacter).ProcessOrPass(0.1f);

        // Assert
        passedToState.ShouldBeNull();
        PilotCharacter.Velocity.ShouldBe(Vector2.Zero);
    }
    
    // [Fact]
    // public void TestTransitionToFallingStateWhenNotOnFloor()
    // {
    //     // Arrange
    //     PilotCharacter.OnFloor.Returns(false);
    //     
    //     // Act
    //     var passedToState = new PilotIdleState(PilotCharacter).ProcessOrPass(0.1f);
    //
    //     // Assert
    //     passedToState.ShouldBeNull();
    //     
    // }
}
