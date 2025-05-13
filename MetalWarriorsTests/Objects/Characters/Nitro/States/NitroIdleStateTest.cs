using Godot;
using MetalWarriors.Objects.Characters;
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
        NitroCharacter.Initialize(
            onFloor: true,
            direction: CharacterDirection.FacingRight,
            velocity: Vector2.Zero,
            animationOffset: Vector2.Zero,
            gunOffset: Vector2.Zero,
            currentAnimation: "idle",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        // Act
        NitroCharacter.StateMachine.SetCurrentState(typeof(NitroIdleState));
        NitroCharacter._PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(CharacterDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        NitroCharacter.CurrentAnimation.ShouldBe("idle");
    }
    
    [Fact]
    public void Nitro_should_stop_moving_when_left_D_Pad_is_not_pressed()
    {
        // Arrange
        NitroCharacter.Initialize(
            onFloor: true,
            direction: CharacterDirection.FacingRight,
            velocity: new Vector2(-BaseNitroState.MovementSpeed, 0),
            animationOffset: Vector2.Zero,
            gunOffset: Vector2.Zero,
            currentAnimation: "flying",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        Controller.IsDPadLeftPressed.Returns(false);
    
        // Act
        NitroCharacter.StateMachine.SetCurrentState(typeof(NitroWalkingState));
        NitroCharacter._PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(CharacterDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        NitroCharacter.CurrentAnimation.ShouldBe("idle");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_stop_moving_when_right_D_Pad_is_not_pressed()
    {
        // Arrange
        NitroCharacter.Initialize(
            onFloor: true,
            direction: CharacterDirection.FacingRight,
            velocity: new Vector2(BaseNitroState.MovementSpeed, 0),
            animationOffset: Vector2.Zero,
            gunOffset: Vector2.Zero,
            currentAnimation: "flying",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        // Act
        NitroCharacter.StateMachine.SetCurrentState(typeof(NitroWalkingState));
        NitroCharacter._PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(CharacterDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        NitroCharacter.CurrentAnimation.ShouldBe("idle");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_become_idle_when_landing_animation_is_finished()
    {
        // Arrange
        NitroCharacter.Initialize(
            onFloor: true,
            direction: CharacterDirection.FacingRight,
            velocity: Vector2.Zero,
            animationOffset: new Vector2(100, 100),
            gunOffset: new Vector2(100, 100),
            currentAnimation: "landing",
            currentAnimationFrame: 0,
            isAnimationFinished: true
        );
        
        // Act
        NitroCharacter.StateMachine.SetCurrentState(typeof(NitroLandingState));
        NitroCharacter._PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(CharacterDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        NitroCharacter.CurrentAnimation.ShouldBe("idle");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
        NitroCharacter.AnimationOffset.ShouldBe(Vector2.Zero);
        NitroCharacter.GunOffset.ShouldBe(NitroIdleState.GunOffset);
    }
}
