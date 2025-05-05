using Godot;
using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriors.Objects.Characters.Nitro.States;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Nitro.States;

public class NitroFallingStateTest(ITestOutputHelper testOutputHelper) : BaseNitroStateTest(testOutputHelper)
{
    [Fact]
    public void Nitro_should_decelerate_when_jetting_is_stopped()
    {
        // Arrange
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxRisingVelocity);
        
        // _controller
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroFlyingState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxRisingVelocity + BaseNitroState.FallingForce));
        NitroCharacter.CurrentAnimation.ShouldBe("falling");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_fall_after_reaching_the_apex_after_thrust_is_cut()
    {
        // Arrange
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = Vector2.Zero;
        
        // _controller
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroFlyingState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.FallingForce));
        NitroCharacter.CurrentAnimation.ShouldBe("falling");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_not_fall_too_fast()
    {
        // Arrange
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxFallingVelocity + 10);
        NitroCharacter.PlayAnimation("falling");
        
        // _controller
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroFallingState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxFallingVelocity));
        NitroCharacter.CurrentAnimation.ShouldBe("falling");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_fall_if_idle_and_no_buttons_pressed()
    {
        // Arrange
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = Vector2.Zero;
        
        // Controller not being used
        // _controller
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroIdleState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.FallingForce));
        NitroCharacter.CurrentAnimation.ShouldBe("falling");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_fall_if_walking_and_not_on_the_floor()
    {
        // Arrange
        StateMachine.SetCurrentState(typeof(NitroWalkingState));
        Controller.IsDPadLeftPressed.Returns(true);
        
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.FacingLeft;
        NitroCharacter.CurrentAnimation = "walking";
        NitroCharacter.Velocity = new Vector2(-BaseNitroState.MovementSpeed, 0);
        
        // Act
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingLeft);
        NitroCharacter.Velocity.ShouldBe(new Vector2(-BaseNitroState.MovementSpeed, BaseNitroState.FallingForce));
        NitroCharacter.CurrentAnimation.ShouldBe("falling");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_transition_from_landing_to_falling_when_he_goes_off_a_cliff()
    {
        // Arrange
        StateMachine.SetCurrentState(typeof(NitroLandingState));
        Controller.IsDPadLeftPressed.Returns(true);
        
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.FacingLeft;
        NitroCharacter.CurrentAnimation = "landing";
        NitroCharacter.Velocity = new Vector2(BaseNitroState.MovementSpeed, 0);
    
        // Act
        StateMachine.PhysicsProcess(0.1f);
    
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingLeft);
        NitroCharacter.Velocity.ShouldBe(new Vector2(-BaseNitroState.MovementSpeed, BaseNitroState.FallingForce));
        NitroCharacter.CurrentAnimation.ShouldBe("falling");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1); // The animation should have already been played in the Launching Entered state
        NitroCharacter.AnimationWasPaused.ShouldBe(false);
    }
    
    [Fact]
    public void Nitro_gun_position_is_correct()
    {
        // Arrange
        StateMachine.SetCurrentState(typeof(NitroIdleState));
        
        NitroCharacter.CurrentAnimationFrame = 0;
        NitroCharacter.GunPosition = Vector2.Zero;
        NitroCharacter.OnFloor = false;

        // Act
        StateMachine.SetCurrentState(typeof(NitroWalkingState));
        StateMachine.PhysicsProcess(0.1f);

        // Assert
        NitroCharacter.GunPosition.ShouldBe(NitroFallingState.GunPositionAtFrame0);
    }
}
