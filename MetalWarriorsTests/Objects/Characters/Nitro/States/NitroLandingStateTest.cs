using Godot;
using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriors.Objects.Characters.Nitro.States;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Nitro.States;

public class NitroLandingStateTest(ITestOutputHelper testOutputHelper) : BaseNitroStateTest(testOutputHelper)
{
    [Fact]
    public void Nitro_should_transition_from_falling_to_landing_when_contact_is_made_with_the_floor_but_keep_moving_right()
    {
        // Arrange
        StateMachine.SetCurrentState(typeof(NitroFallingState));
        Controller.IsDPadRightPressed.Returns(true);
        
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = new Vector2(BaseNitroState.MovementSpeed, BaseNitroState.MaxFallingVelocity);
        NitroCharacter.CurrentAnimation = "falling";
        
        // Act
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(BaseNitroState.MovementSpeed, 0));
        NitroCharacter.CurrentAnimation.ShouldBe("landing");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
        NitroCharacter.AnimationWasPaused.ShouldBe(false);
    }
        
    [Fact]
    public void Nitro_should_stop_falling_if_on_the_floor()
    {
        // Arrange
        StateMachine.SetCurrentState(typeof(NitroFallingState));
        
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxFallingVelocity);
        NitroCharacter.CurrentAnimation = "falling";
        
        // Controller not being used
        
        // Act
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        NitroCharacter.CurrentAnimation.ShouldBe("landing");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
}
