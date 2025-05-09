﻿using Godot;
using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriors.Objects.Characters.Nitro.States;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Nitro.States;

public class NitroLaunchingStateTest(ITestOutputHelper testOutputHelper) : BaseNitroStateTest(testOutputHelper)
{
    [Fact]
    public void Nitro_should_rise_constantly_when_launching()
    {
        // Arrange
        NitroCharacter.SetInitialState(
            onFloor: true,
            direction: NitroDirection.FacingRight,
            velocity: Vector2.Zero,
            animationOffset: new Vector2(100, 100),
            gunOffset: new Vector2(100, 100),
            currentAnimation: "flying",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        Controller.IsButtonBPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroIdleState));
        StateMachine.PhysicsProcess(0.1f);
    
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxRisingVelocity));
        NitroCharacter.CurrentAnimation.ShouldBe("launching");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
        NitroCharacter.AnimationOffset.ShouldBe(NitroLaunchingState.AnimationOffset);
        NitroCharacter.GunOffset.ShouldBe(NitroLaunchingState.GunOffset + NitroLaunchingState.AnimationOffset);
    }
    
    [Fact]
    public void Nitro_should_keep_rising_constantly_when_launching()
    {
        // Arrange
        NitroCharacter.SetInitialState(
            onFloor: false,
            direction: NitroDirection.FacingRight,
            velocity: new Vector2(0, BaseNitroState.MaxRisingVelocity),
            animationOffset: Vector2.Zero,
            gunOffset: Vector2.Zero,
            currentAnimation: "launching",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        Controller.IsButtonBPressed.Returns(true);
    
        // Act
        StateMachine.SetCurrentState(typeof(NitroLaunchingState));
        StateMachine.PhysicsProcess(0.1f);
    
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxRisingVelocity));
        NitroCharacter.CurrentAnimation.ShouldBe("launching");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(0); // The animation should have already been played in the Launching Entered state
    }
    
    [Fact]
    public void Nitro_should_transition_from_walking_to_launching_when_B_button_is_pressed()
    {
        // Arrange
        NitroCharacter.SetInitialState(
            onFloor: true,
            direction: NitroDirection.FacingLeft,
            velocity: new Vector2(BaseNitroState.MovementSpeed, 0),
            animationOffset: Vector2.Zero,
            gunOffset: Vector2.Zero,
            currentAnimation: "walking",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        // Set up the Controller
        Controller.IsDPadLeftPressed.Returns(true);
        Controller.IsButtonBPressed.Returns(true);
    
        // Act
        StateMachine.SetCurrentState(typeof(NitroWalkingState));
        StateMachine.PhysicsProcess(0.1f);
    
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingLeft);
        NitroCharacter.Velocity.ShouldBe(new Vector2(-BaseNitroState.MovementSpeed, BaseNitroState.MaxRisingVelocity));
        NitroCharacter.CurrentAnimation.ShouldBe("launching");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1); // The animation should have already been played in the Launching Entered state
        NitroCharacter.AnimationWasPaused.ShouldBe(false);
    }
    
    [Fact]
    public void Nitro_should_transition_from_landing_to_launching_when_B_button_is_pressed()
    {
        // Arrange
        NitroCharacter.SetInitialState(
            onFloor: true,
            direction: NitroDirection.FacingLeft,
            velocity: new Vector2(BaseNitroState.MovementSpeed, 0),
            animationOffset: Vector2.Zero,
            gunOffset: Vector2.Zero,
            currentAnimation: "landing",
            currentAnimationFrame: 0,
            isAnimationFinished: false
        );
        
        Controller.IsDPadLeftPressed.Returns(true);
        Controller.IsButtonBPressed.Returns(true);
    
        // Act
        StateMachine.SetCurrentState(typeof(NitroLandingState));
        StateMachine.PhysicsProcess(0.1f);
    
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingLeft);
        NitroCharacter.Velocity.ShouldBe(new Vector2(-BaseNitroState.MovementSpeed, BaseNitroState.MaxRisingVelocity));
        NitroCharacter.CurrentAnimation.ShouldBe("launching");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1); // The animation should have already been played in the Launching Entered state
        NitroCharacter.AnimationWasPaused.ShouldBe(false);
    }
}
