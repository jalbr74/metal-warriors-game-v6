﻿using Godot;
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
        NitroCharacter.SetInitialState(
            onFloor: true,
            direction: NitroDirection.FacingRight,
            velocity: new Vector2(BaseNitroState.MovementSpeed, BaseNitroState.MaxFallingVelocity),
            animationOffset: Vector2.Zero,
            gunOffset: Vector2.Zero,
            currentAnimation: "falling",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        Controller.IsDPadRightPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroFallingState));
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
        NitroCharacter.SetInitialState(
            onFloor: true,
            direction: NitroDirection.FacingRight,
            velocity: new Vector2(0, BaseNitroState.MaxFallingVelocity),
            animationOffset: new Vector2(100, 100),
            gunOffset: new Vector2(100, 100),
            currentAnimation: "falling",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroFallingState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        NitroCharacter.CurrentAnimation.ShouldBe("landing");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
        NitroCharacter.AnimationOffset.ShouldBe(NitroLandingState.AnimationOffset);
        NitroCharacter.GunOffset.ShouldBe(NitroLandingState.GunOffset + NitroLandingState.AnimationOffset);
    }
}
