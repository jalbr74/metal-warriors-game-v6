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
    public void PilotShouldMoveToTheRightWhenControllerInputIsRight()
    {
        // Arrange
        PilotCharacter.Initialize(
            onFloor: true,
            direction: CharacterDirection.FacingLeft,
            velocity: new Vector2(-BasePilotState.MovementSpeed, 0),
            currentAnimation: "walking",
            currentAnimationFrame: 0,
            isAnimationFinished: true
        );
        
        Controller.IsDPadLeftPressed.Returns(false);
        
        // Act
        PilotCharacter.StateMachine.SetCurrentState(typeof(PilotWalkingState));
        PilotCharacter.StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        PilotCharacter.Direction.ShouldBe(CharacterDirection.FacingLeft);
        PilotCharacter.Velocity.ShouldBe(Vector2.Zero);
        PilotCharacter.CurrentAnimation.ShouldBe("idle");
        PilotCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
}
