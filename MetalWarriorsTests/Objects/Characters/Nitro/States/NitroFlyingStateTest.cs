using Godot;
using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriors.Objects.Characters.Nitro.States;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Nitro.States;

public class NitroFlyingStateTest(ITestOutputHelper testOutputHelper) : BaseNitroStateTest(testOutputHelper)
{
    [Fact]
    public void Nitro_should_change_from_launching_to_flying_after_the_launching_animation_is_finished()
    {
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxRisingVelocity);
        NitroCharacter.IsAnimationFinished = true;
        
        Controller.IsButtonBPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroLaunchingState));
        StateMachine.PhysicsProcess(0.1f);
    
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxRisingVelocity));
        NitroCharacter.CurrentAnimation.ShouldBe("flying");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_accelerate_when_jetting_is_started_again()
    {
        // Arrange
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxFallingVelocity);
        NitroCharacter.CurrentAnimation = "falling";
        
        Controller.IsButtonBPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroFallingState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxFallingVelocity - BaseNitroState.BoostingForce));
        NitroCharacter.CurrentAnimation.ShouldBe("flying");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_not_accelerate_too_fast()
    {
        // Arrange
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxRisingVelocity + 10);
        NitroCharacter.CurrentAnimation = "flying";
        
        Controller.IsButtonBPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroFlyingState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxRisingVelocity));
        NitroCharacter.CurrentAnimation.ShouldBe("flying");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(0);
    }
    
    [Fact]
    public void Nitro_should_not_exceed_max_rising_velocity()
    {
        // Arrange
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxRisingVelocity - 10);
        NitroCharacter.CurrentAnimation = "flying";
        
        Controller.IsButtonBPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroFlyingState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxRisingVelocity));
        NitroCharacter.CurrentAnimation.ShouldBe("flying");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(0);
    }
    
    [Fact]
    public void Nitro_switches_to_flying_after_launching_while_DPad_is_pressed()
    {
        // Arrange
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = new Vector2(BaseNitroState.MovementSpeed, BaseNitroState.MaxRisingVelocity);
        NitroCharacter.IsAnimationFinished = true;
        
        Controller.IsDPadRightPressed.Returns(true);
        Controller.IsButtonBPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroLaunchingState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(BaseNitroState.MovementSpeed, BaseNitroState.MaxRisingVelocity));
        NitroCharacter.CurrentAnimation.ShouldBe("flying");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
        NitroCharacter.AnimationWasPaused.ShouldBe(false);
    }
    
    [Fact]
    public void Nitro_changes_direction_when_DPad_is_pressed_the_other_direction()
    {
        // Arrange
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.FacingLeft;
        NitroCharacter.Velocity = new Vector2(-BaseNitroState.MovementSpeed, BaseNitroState.MaxRisingVelocity);
        NitroCharacter.CurrentAnimation = "flying";
        
        StateMachine.SetCurrentState(typeof(NitroFlyingState));
        Controller.IsDPadRightPressed.Returns(true);
        Controller.IsButtonBPressed.Returns(true);
        
        // Act
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(BaseNitroState.MovementSpeed, BaseNitroState.MaxRisingVelocity));
        NitroCharacter.CurrentAnimation.ShouldBe("flying");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(0);
        NitroCharacter.AnimationWasPaused.ShouldBe(false);
    }
    
    [Fact]
    public void Nitro_transitions_to_flying_when_B_button_is_pressed()
    {
        // Arrange
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxFallingVelocity);
        NitroCharacter.CurrentAnimation = "falling";
        
        StateMachine.SetCurrentState(typeof(NitroFallingState));
        Controller.IsButtonBPressed.Returns(true);
        
        // Act
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxFallingVelocity - BaseNitroState.BoostingForce));
        NitroCharacter.CurrentAnimation.ShouldBe("flying");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
        NitroCharacter.AnimationWasPaused.ShouldBe(false);
    }
    
    [Fact]
    public void Nitro_transitions_to_idle_when_on_floor()
    {
        // Arrange
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = Vector2.Zero;
        NitroCharacter.CurrentAnimation = "flying";
        
        StateMachine.SetCurrentState(typeof(NitroFlyingState));
        Controller.IsButtonBPressed.Returns(true);
        
        // Act
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, -BaseNitroState.BoostingForce));
        NitroCharacter.CurrentAnimation.ShouldBe("flying");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(0);
        NitroCharacter.AnimationWasPaused.ShouldBe(false);
    }
}
