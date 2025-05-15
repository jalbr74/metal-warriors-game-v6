using Godot;
using MetalWarriors.Objects.Characters;
using MetalWarriors.Objects.Characters.Nitro.States;
using MetalWarriors.Objects.Characters.ParkedNitro.States;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.ParkedNitro.States;

public class ParkedNitroExitingStateTest(ITestOutputHelper testOutputHelper) : BaseParkedNitroStateTest(testOutputHelper)
{
    [Fact]
    public void TestOne()
    {
        // Arrange
        ParkedNitroCharacter.Initialize(
            onFloor: true
        );
        
        // Act
        ParkedNitroCharacter.StateMachine.TransitionToState(typeof(ParkedNitroExitingState));
        ParkedNitroCharacter.StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        ParkedNitroCharacter.Direction.ShouldBe(CharacterDirection.FacingRight);
        ParkedNitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        ParkedNitroCharacter.CurrentAnimation.ShouldBe("exiting");
        ParkedNitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    // TODO: Test other cases for exiting state
}
