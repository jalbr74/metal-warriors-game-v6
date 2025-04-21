using Godot;
using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriors.Objects.Characters.Nitro.States;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Nitro.States;

public class NitroLaunchingStateTest : BaseNitroStateTest
{
    public NitroLaunchingStateTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    [Fact]
    public void Nitro_should_rise_constantly_when_launching()
    {
        // Arrange
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.Right;
        NitroCharacter.Velocity = Vector2.Zero;
        
        Controller.IsButtonBPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState("idle");
        StateMachine.PhysicsProcess(0.1f);
    
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxRisingVelocity));
        NitroCharacter.CurrentAnimation.ShouldBe("launching");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_keep_rising_constantly_when_launching()
    {
        // Arrange
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.Right;
        NitroCharacter.CurrentAnimation = "launching";
        // _nitroCharacter.State = NitroState.Launching;
        NitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxRisingVelocity);
        
        Controller.IsButtonBPressed.Returns(true);
    
        // Act
        StateMachine.SetCurrentState("launching");
        StateMachine.PhysicsProcess(0.1f);
    
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        // _nitroCharacter.State.ShouldBe(NitroState.Launching);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxRisingVelocity));
        NitroCharacter.CurrentAnimation.ShouldBe("launching");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(0); // The animation should have already been played in the Launching Entered state
    }
    
    
    
    
    
    
    
    [Fact]
    public void Nitro_should_decelerate_when_jetting_is_stopped()
    {
        // Arrange
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.Right;
        NitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxRisingVelocity);
        
        // _controller
        
        // Act
        StateMachine.SetCurrentState("flying");
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxRisingVelocity + BaseNitroState.FallingForce));
        NitroCharacter.CurrentAnimation.ShouldBe("falling");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    // [Fact]
    public void Nitro_should_accelerate_when_jetting_is_started_again()
    {
        // Arrange
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.Right;
        NitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxFallingVelocity);
        
        Controller.IsButtonBPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState("flying");
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxFallingVelocity - BaseNitroState.BoostingForce));
        NitroCharacter.CurrentAnimation.ShouldBe("flying");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_fall_after_reaching_the_apex_after_thrust_is_cut()
    {
        // Arrange
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.Right;
        // _nitroCharacter.State = NitroState.Flying;
        NitroCharacter.Velocity = Vector2.Zero;
        
        // _controller
        
        // Act
        StateMachine.SetCurrentState("flying");
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        // _nitroCharacter.State.ShouldBe(NitroState.Falling);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.FallingForce));
        NitroCharacter.CurrentAnimation.ShouldBe("falling");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_not_fall_too_fast()
    {
        // Arrange
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.Right;
        NitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxFallingVelocity + 10);
        NitroCharacter.PlayAnimation("falling");
        
        // _controller
        
        // Act
        StateMachine.SetCurrentState("falling");
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxFallingVelocity));
        NitroCharacter.CurrentAnimation.ShouldBe("falling");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_not_accelerate_too_fast()
    {
        // Arrange
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.Right;
        NitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxRisingVelocity + 10);
        NitroCharacter.PlayAnimation("flying");
        
        Controller.IsButtonBPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState("flying");
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxRisingVelocity));
        NitroCharacter.CurrentAnimation.ShouldBe("flying");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_not_exceed_max_rising_velocity()
    {
        // Arrange
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.Right;
        NitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxRisingVelocity - 10);
        NitroCharacter.PlayAnimation("flying");
        
        Controller.IsButtonBPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState("flying");
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxRisingVelocity));
        NitroCharacter.CurrentAnimation.ShouldBe("flying");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_not_go_farther_down_if_already_on_the_floor()
    {
        // Arrange
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.Right;
        NitroCharacter.Velocity = Vector2.Zero;
        NitroCharacter.PlayAnimation("idle");
        
        // _controller
        
        // Act
        StateMachine.SetCurrentState("idle");
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        NitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        NitroCharacter.CurrentAnimation.ShouldBe("idle");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_move_left_when_left_D_Pad_is_pressed()
    {
        // Arrange
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.Right;
        NitroCharacter.Velocity = Vector2.Zero;
        
        Controller.IsDPadLeftPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState("idle");
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.Left);
        NitroCharacter.Velocity.ShouldBe(new Vector2(-BaseNitroState.MovementSpeed, 0));
        NitroCharacter.CurrentAnimation.ShouldBe("walking");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_stop_moving_when_left_D_Pad_is_not_pressed()
    {
        // Arrange
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.Right;
        NitroCharacter.Velocity = new Vector2(-BaseNitroState.MovementSpeed, 0);
        
        Controller.IsDPadLeftPressed.Returns(false);
    
        // Act
        StateMachine.SetCurrentState("walking");
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        NitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        NitroCharacter.CurrentAnimation.ShouldBe("idle");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_move_right_when_right_D_Pad_is_pressed()
    {
        // Arrange
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.Right;
        NitroCharacter.Velocity = Vector2.Zero;
        
        Controller.IsDPadRightPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState("idle");
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        NitroCharacter.Velocity.ShouldBe(new Vector2(BaseNitroState.MovementSpeed, 0));
        NitroCharacter.CurrentAnimation.ShouldBe("walking");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_stop_moving_when_right_D_Pad_is_not_pressed()
    {
        // Arrange
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.Right;
        // _nitroCharacter.State = NitroState.Walking;
        NitroCharacter.Velocity = new Vector2(BaseNitroState.MovementSpeed, 0);
        
        // _controller
        
        // Act
        StateMachine.SetCurrentState("walking");
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        // _nitroCharacter.State.ShouldBe(NitroState.Idle);
        NitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        NitroCharacter.CurrentAnimation.ShouldBe("idle");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_switches_to_flying_after_launching_while_DPad_is_pressed()
    {
        // Arrange
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.Right;
        NitroCharacter.Velocity = new Vector2(BaseNitroState.MovementSpeed, BaseNitroState.MaxRisingVelocity);
        NitroCharacter.IsLaunchingAnimationComplete = true;
        
        Controller.IsDPadRightPressed.Returns(true);
        Controller.IsButtonBPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState("launching");
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        NitroCharacter.Velocity.ShouldBe(new Vector2(BaseNitroState.MovementSpeed, BaseNitroState.MaxRisingVelocity));
        NitroCharacter.CurrentAnimation.ShouldBe("flying");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_fall_if_idle_and_no_buttons_pressed()
    {
        // Arrange
        NitroCharacter.OnFloor = false;
        NitroCharacter.Direction = NitroDirection.Right;
        NitroCharacter.Velocity = Vector2.Zero;
        
        // Controller not being used
        // _controller
        
        // Act
        StateMachine.SetCurrentState("idle");
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        NitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.FallingForce));
        NitroCharacter.CurrentAnimation.ShouldBe("falling");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_stop_falling_if_on_the_floor()
    {
        // Arrange
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.Right;
        NitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxFallingVelocity);
        
        // Controller not being used
        // _controller
        
        // Act
        StateMachine.SetCurrentState("falling");
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        NitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        NitroCharacter.CurrentAnimation.ShouldBe("idle");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_walking_animation_should_continually_play()
    {
        // Arrange
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.Right;
        NitroCharacter.Velocity = Vector2.Zero;
        
        // Controller not being used
        Controller.IsDPadRightPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState("idle");
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        NitroCharacter.Velocity.ShouldBe(new Vector2(BaseNitroState.MovementSpeed, 0));
        NitroCharacter.CurrentAnimation.ShouldBe("walking");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
        NitroCharacter.AnimationWasPaused.ShouldBe(false);
    }
}
