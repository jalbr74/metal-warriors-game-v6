using Godot;
using MetalWarriors.Objects.Characters;
using MetalWarriors.Objects.Characters.Pilot.States;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Pilot.States;

public class PilotWalkingStateTest(ITestOutputHelper output) : BasePilotStateTest(output)
{
    [Fact]
    public void PilotShouldMoveToTheRightWhenControllerInputIsRight()
    {
        // Arrange
        PilotCharacter.Initialize(
            onFloor: true,
            direction: CharacterDirection.FacingRight,
            velocity: Vector2.Zero,
            currentAnimation: "idle",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        Controller.IsDPadLeftPressed.Returns(true);
        
        // Act
        PilotCharacter.StateMachine.SetCurrentState(typeof(PilotIdleState));
        PilotCharacter.StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        PilotCharacter.Direction.ShouldBe(CharacterDirection.FacingLeft);
        PilotCharacter.Velocity.ShouldBe(new Vector2(-BasePilotState.MovementSpeed, 0));
        PilotCharacter.CurrentAnimation.ShouldBe("walking");
        PilotCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
}
