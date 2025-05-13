using Godot;
using MetalWarriors.Objects.Characters;
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
        NitroCharacter.SetInitialState(
            onFloor: false,
            direction: CharacterDirection.FacingRight,
            velocity: new Vector2(0, BaseNitroState.MaxRisingVelocity),
            animationOffset: new Vector2(100, 100),
            gunOffset: new Vector2(100, 100),
            currentAnimation: "flying",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroFlyingState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(CharacterDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxRisingVelocity + BaseNitroState.FallingForce));
        NitroCharacter.CurrentAnimation.ShouldBe("falling");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
        NitroCharacter.GunOffset.ShouldBe(NitroFallingState.GunOffset);
        NitroCharacter.AnimationOffset.ShouldBe(NitroFallingState.AnimationOffset);
    }
    
    [Fact]
    public void Nitro_should_fall_after_reaching_the_apex_after_thrust_is_cut()
    {
        // Arrange
        NitroCharacter.SetInitialState(
            onFloor: false,
            direction: CharacterDirection.FacingRight,
            velocity: Vector2.Zero,
            animationOffset: Vector2.Zero,
            gunOffset: Vector2.Zero,
            currentAnimation: "flying",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroFlyingState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(CharacterDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.FallingForce));
        NitroCharacter.CurrentAnimation.ShouldBe("falling");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_not_fall_too_fast()
    {
        // Arrange
        NitroCharacter.SetInitialState(
            onFloor: false,
            direction: CharacterDirection.FacingRight,
            velocity: new Vector2(0, BaseNitroState.MaxFallingVelocity + 10),
            animationOffset: Vector2.Zero,
            gunOffset: Vector2.Zero,
            currentAnimation: "falling",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroFallingState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(CharacterDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxFallingVelocity));
        NitroCharacter.CurrentAnimation.ShouldBe("falling");
    }
    
    [Fact]
    public void Nitro_should_fall_if_idle_and_no_buttons_pressed()
    {
        // Arrange
        NitroCharacter.SetInitialState(
            onFloor: false,
            direction: CharacterDirection.FacingRight,
            velocity: Vector2.Zero,
            animationOffset: Vector2.Zero,
            gunOffset: Vector2.Zero,
            currentAnimation: "idle",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroIdleState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(CharacterDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.FallingForce));
        NitroCharacter.CurrentAnimation.ShouldBe("falling");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_fall_if_walking_and_not_on_the_floor()
    {
        // Arrange
        NitroCharacter.SetInitialState(
            onFloor: false,
            direction: CharacterDirection.FacingLeft,
            velocity: new Vector2(-BaseNitroState.MovementSpeed, 0),
            animationOffset: Vector2.Zero,
            gunOffset: Vector2.Zero,
            currentAnimation: "walking",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        Controller.IsDPadLeftPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroWalkingState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(CharacterDirection.FacingLeft);
        NitroCharacter.Velocity.ShouldBe(new Vector2(-BaseNitroState.MovementSpeed, BaseNitroState.FallingForce));
        NitroCharacter.CurrentAnimation.ShouldBe("falling");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_transition_from_landing_to_falling_when_he_goes_off_a_cliff()
    {
        // Arrange
        NitroCharacter.SetInitialState(
            onFloor: false,
            direction: CharacterDirection.FacingLeft,
            velocity: new Vector2(BaseNitroState.MovementSpeed, 0),
            animationOffset: Vector2.Zero,
            gunOffset: Vector2.Zero,
            currentAnimation: "landing",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        Controller.IsDPadLeftPressed.Returns(true);
    
        // Act
        StateMachine.SetCurrentState(typeof(NitroLandingState));
        StateMachine.PhysicsProcess(0.1f);
    
        // Assert
        NitroCharacter.Direction.ShouldBe(CharacterDirection.FacingLeft);
        NitroCharacter.Velocity.ShouldBe(new Vector2(-BaseNitroState.MovementSpeed, BaseNitroState.FallingForce));
        NitroCharacter.CurrentAnimation.ShouldBe("falling");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1); // The animation should have already been played in the Launching Entered state
        NitroCharacter.AnimationWasPaused.ShouldBe(false);
    }
    
    [Fact]
    public void Nitro_gun_position_is_correct()
    {
        // Arrange
        NitroCharacter.SetInitialState(
            onFloor: false,
            direction: CharacterDirection.FacingLeft,
            velocity: new Vector2(BaseNitroState.MovementSpeed, 0),
            animationOffset: Vector2.Zero,
            gunOffset: Vector2.Zero,
            currentAnimation: "landing",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroIdleState));
        StateMachine.PhysicsProcess(0.1f);

        // Assert
        NitroCharacter.GunOffset.ShouldBe(NitroFallingState.GunOffset);
    }
}
