using Godot;
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
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = Vector2.Zero;
        NitroCharacter.AnimationOffset = new Vector2(100, 100);
        NitroCharacter.GunOffset = new Vector2(100, 100);
        
        Controller.IsButtonBPressed.Returns(true);
        StateMachine.SetCurrentState(typeof(NitroIdleState));
        
        // Act
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
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.CurrentAnimation = "launching";
        NitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxRisingVelocity);
        
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
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.FacingLeft;
        NitroCharacter.CurrentAnimation = "walking";
        NitroCharacter.Velocity = new Vector2(BaseNitroState.MovementSpeed, 0);
        
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
        StateMachine.SetCurrentState(typeof(NitroLandingState));
        Controller.IsDPadLeftPressed.Returns(true);
        Controller.IsButtonBPressed.Returns(true);
        
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.FacingLeft;
        NitroCharacter.CurrentAnimation = "landing";
        NitroCharacter.Velocity = new Vector2(BaseNitroState.MovementSpeed, 0);
    
        // Act
        StateMachine.PhysicsProcess(0.1f);
    
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingLeft);
        NitroCharacter.Velocity.ShouldBe(new Vector2(-BaseNitroState.MovementSpeed, BaseNitroState.MaxRisingVelocity));
        NitroCharacter.CurrentAnimation.ShouldBe("launching");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1); // The animation should have already been played in the Launching Entered state
        NitroCharacter.AnimationWasPaused.ShouldBe(false);
    }
}
