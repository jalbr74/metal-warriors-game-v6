using Godot;
using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriors.Objects.Characters.Nitro.States;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Nitro.States;

public class NitroIdleStateTest(ITestOutputHelper testOutputHelper) : BaseNitroStateTest(testOutputHelper)
{
    [Fact]
    public void Nitro_should_not_go_farther_down_if_already_on_the_floor()
    {
        // Arrange
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = Vector2.Zero;
        NitroCharacter.PlayAnimation("idle");
        
        // _controller
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroIdleState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        NitroCharacter.CurrentAnimation.ShouldBe("idle");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_stop_moving_when_left_D_Pad_is_not_pressed()
    {
        // Arrange
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = new Vector2(-BaseNitroState.MovementSpeed, 0);
        
        Controller.IsDPadLeftPressed.Returns(false);
    
        // Act
        StateMachine.SetCurrentState(typeof(NitroWalkingState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        NitroCharacter.CurrentAnimation.ShouldBe("idle");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_stop_moving_when_right_D_Pad_is_not_pressed()
    {
        // Arrange
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = new Vector2(BaseNitroState.MovementSpeed, 0);
        
        // _controller
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroWalkingState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        NitroCharacter.CurrentAnimation.ShouldBe("idle");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_become_idle_when_landing_animation_is_finished()
    {
        // Arrange
        StateMachine.SetCurrentState(typeof(NitroLandingState));
        
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = Vector2.Zero;
        NitroCharacter.CurrentAnimation = "landing";
        NitroCharacter.IsAnimationFinished = true;
        
        NitroCharacter.CurrentAnimationOffset = new Vector2(100, 100);
        NitroCharacter.GunPosition = new Vector2(100, 100);
        
        // Act
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        NitroCharacter.CurrentAnimation.ShouldBe("idle");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
        NitroCharacter.CurrentAnimationOffset.ShouldBe(Vector2.Zero);
        NitroCharacter.GunPosition.ShouldBe(NitroIdleState.GunPosition);
    }
}
