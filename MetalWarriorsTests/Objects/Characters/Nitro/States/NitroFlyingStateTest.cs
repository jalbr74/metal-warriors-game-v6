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
        NitroCharacter.SetInitialState(
            onFloor: false,
            direction: NitroDirection.FacingRight,
            velocity: new Vector2(0, BaseNitroState.MaxRisingVelocity),
            animationOffset: new Vector2(100, 100),
            gunOffset: new Vector2(100, 100),
            currentAnimation: "flying",
            currentAnimationFrame: 0,
            isAnimationFinished: true
        );
        
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
        NitroCharacter.SetInitialState(
            onFloor: false,
            direction: NitroDirection.FacingRight,
            velocity: new Vector2(0, BaseNitroState.MaxFallingVelocity),
            animationOffset: new Vector2(100, 100),
            gunOffset: new Vector2(100, 100),
            currentAnimation: "falling",
            currentAnimationFrame: 0,
            isAnimationFinished: true
        );
        
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
        NitroCharacter.SetInitialState(
            onFloor: false,
            direction: NitroDirection.FacingRight,
            velocity: new Vector2(0, BaseNitroState.MaxRisingVelocity + 10),
            animationOffset: new Vector2(100, 100),
            gunOffset: new Vector2(100, 100),
            currentAnimation: "flying",
            currentAnimationFrame: 0,
            isAnimationFinished: true
        );
        
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
        NitroCharacter.SetInitialState(
            onFloor: false,
            direction: NitroDirection.FacingRight,
            velocity: new Vector2(0, BaseNitroState.MaxRisingVelocity - 10),
            animationOffset: new Vector2(100, 100),
            gunOffset: new Vector2(100, 100),
            currentAnimation: "flying",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
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
        NitroCharacter.SetInitialState(
            onFloor: false,
            direction: NitroDirection.FacingRight,
            velocity: new Vector2(BaseNitroState.MovementSpeed, BaseNitroState.MaxRisingVelocity),
            animationOffset: Vector2.Zero,
            gunOffset: Vector2.Zero,
            currentAnimation: "flying",
            currentAnimationFrame: 0,
            isAnimationFinished: true
        );
        
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
        NitroCharacter.SetInitialState(
            onFloor: false,
            direction: NitroDirection.FacingLeft,
            velocity: new Vector2(-BaseNitroState.MovementSpeed, BaseNitroState.MaxRisingVelocity),
            animationOffset: Vector2.Zero,
            gunOffset: Vector2.Zero,
            currentAnimation: "flying",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        Controller.IsDPadRightPressed.Returns(true);
        Controller.IsButtonBPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroFlyingState));
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
        NitroCharacter.SetInitialState(
            onFloor: false,
            direction: NitroDirection.FacingRight,
            velocity: new Vector2(0, BaseNitroState.MaxFallingVelocity),
            animationOffset: Vector2.Zero,
            gunOffset: Vector2.Zero,
            currentAnimation: "falling",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        Controller.IsButtonBPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroFallingState));
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
        NitroCharacter.SetInitialState(
            onFloor: true,
            direction: NitroDirection.FacingRight,
            velocity: Vector2.Zero,
            animationOffset: Vector2.Zero,
            gunOffset: Vector2.Zero,
            currentAnimation: "flying",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        Controller.IsButtonBPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroFlyingState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, -BaseNitroState.BoostingForce));
        NitroCharacter.CurrentAnimation.ShouldBe("flying");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(0);
        NitroCharacter.AnimationWasPaused.ShouldBe(false);
    }
    
    [Fact]
    public void Nitro_gun_position_is_correct()
    {
        // Arrange
        NitroCharacter.SetInitialState(
            onFloor: false,
            direction: NitroDirection.FacingRight,
            velocity: Vector2.Zero,
            animationOffset: Vector2.Zero,
            gunOffset: Vector2.Zero,
            currentAnimation: "flying",
            currentAnimationFrame: 0,
            isAnimationFinished: true
        );
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroLaunchingState));
        StateMachine.PhysicsProcess(0.1f);

        // Assert
        NitroCharacter.GunOffset.ShouldBe(NitroFlyingState.GunOffset + NitroFlyingState.AnimationOffset);
        NitroCharacter.AnimationOffset.ShouldBe(NitroFlyingState.AnimationOffset);
    }
}
