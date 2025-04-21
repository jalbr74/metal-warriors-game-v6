using Godot;
using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriors.Objects.Characters.Nitro.States;
using MetalWarriors.Utils;
using MetalWarriorsTests.Utils;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Nitro;

public class NitroStateTests
{
    private readonly ISnesController _controller = Substitute.For<ISnesController>();
    private readonly TestOutputConsolePrinter _consolePrinter;
    
    private readonly NitroCharacterImplForTesting _nitroCharacter = new();
    private readonly StateMachine _stateMachine;

    public NitroStateTests(ITestOutputHelper testOutputHelper)
    {
        _consolePrinter = new TestOutputConsolePrinter(testOutputHelper);
        
        _stateMachine = new StateMachine(new Dictionary<string, State>
        {
            {"idle", new NitroIdleState(_controller, _nitroCharacter, _consolePrinter)},
            {"walking", new NitroWalkingState(_controller, _nitroCharacter, _consolePrinter)},
            {"launching", new NitroLaunchingState(_controller, _nitroCharacter, _consolePrinter)},
            {"falling", new NitroFallingState(_controller, _nitroCharacter, _consolePrinter)},
            {"flying", new NitroFlyingState(_controller, _nitroCharacter, _consolePrinter)},
        }, "idle", _consolePrinter);
    }
    
    [Fact]
    public void Nitro_should_rise_constantly_when_launching()
    {
        // Arrange
        _nitroCharacter.OnFloor = true;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.Velocity = Vector2.Zero;
        
        _controller.IsButtonBPressed.Returns(true);
        
        // Act
        _stateMachine.SetCurrentState("idle");
        _stateMachine.PhysicsProcess(0.1f);
    
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxRisingVelocity));
        _nitroCharacter.CurrentAnimation.ShouldBe("launching");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }

    [Fact]
    public void Nitro_should_keep_rising_constantly_when_launching()
    {
        // Arrange
        _nitroCharacter.OnFloor = false;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.CurrentAnimation = "launching";
        // _nitroCharacter.State = NitroState.Launching;
        _nitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxRisingVelocity);
        
        _controller.IsButtonBPressed.Returns(true);
    
        // Act
        _stateMachine.SetCurrentState("launching");
        _stateMachine.PhysicsProcess(0.1f);
    
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        // _nitroCharacter.State.ShouldBe(NitroState.Launching);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxRisingVelocity));
        _nitroCharacter.CurrentAnimation.ShouldBe("launching");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(0); // The animation should have already been played in the Launching Entered state
    }
    
    [Fact]
    public void Nitro_should_change_from_launching_to_flying_after_the_launching_animation_is_finished()
    {
        _nitroCharacter.OnFloor = false;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxRisingVelocity);
        _nitroCharacter.IsLaunchingAnimationComplete = true;
        
        _controller.IsButtonBPressed.Returns(true);
        
        // Act
        _stateMachine.SetCurrentState("launching");
        _stateMachine.PhysicsProcess(0.1f);
    
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxRisingVelocity));
        _nitroCharacter.CurrentAnimation.ShouldBe("flying");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_decelerate_when_jetting_is_stopped()
    {
        // Arrange
        _nitroCharacter.OnFloor = false;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxRisingVelocity);
        
        // _controller
        
        // Act
        _stateMachine.SetCurrentState("flying");
        _stateMachine.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxRisingVelocity + BaseNitroState.FallingForce));
        _nitroCharacter.CurrentAnimation.ShouldBe("falling");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    // [Fact]
    public void Nitro_should_accelerate_when_jetting_is_started_again()
    {
        // Arrange
        _nitroCharacter.OnFloor = false;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxFallingVelocity);
        
        _controller.IsButtonBPressed.Returns(true);
        
        // Act
        _stateMachine.SetCurrentState("flying");
        _stateMachine.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxFallingVelocity - BaseNitroState.BoostingForce));
        _nitroCharacter.CurrentAnimation.ShouldBe("flying");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_fall_after_reaching_the_apex_after_thrust_is_cut()
    {
        // Arrange
        _nitroCharacter.OnFloor = false;
        _nitroCharacter.Direction = NitroDirection.Right;
        // _nitroCharacter.State = NitroState.Flying;
        _nitroCharacter.Velocity = Vector2.Zero;
        
        // _controller
        
        // Act
        _stateMachine.SetCurrentState("flying");
        _stateMachine.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        // _nitroCharacter.State.ShouldBe(NitroState.Falling);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.FallingForce));
        _nitroCharacter.CurrentAnimation.ShouldBe("falling");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_not_fall_too_fast()
    {
        // Arrange
        _nitroCharacter.OnFloor = false;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxFallingVelocity + 10);
        _nitroCharacter.PlayAnimation("falling");
        
        // _controller
        
        // Act
        _stateMachine.SetCurrentState("falling");
        _stateMachine.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxFallingVelocity));
        _nitroCharacter.CurrentAnimation.ShouldBe("falling");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_not_accelerate_too_fast()
    {
        // Arrange
        _nitroCharacter.OnFloor = false;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxRisingVelocity + 10);
        _nitroCharacter.PlayAnimation("flying");
        
        _controller.IsButtonBPressed.Returns(true);
        
        // Act
        _stateMachine.SetCurrentState("flying");
        _stateMachine.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxRisingVelocity));
        _nitroCharacter.CurrentAnimation.ShouldBe("flying");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_not_exceed_max_rising_velocity()
    {
        // Arrange
        _nitroCharacter.OnFloor = false;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxRisingVelocity - 10);
        _nitroCharacter.PlayAnimation("flying");
        
        _controller.IsButtonBPressed.Returns(true);
        
        // Act
        _stateMachine.SetCurrentState("flying");
        _stateMachine.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.MaxRisingVelocity));
        _nitroCharacter.CurrentAnimation.ShouldBe("flying");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_not_go_farther_down_if_already_on_the_floor()
    {
        // Arrange
        _nitroCharacter.OnFloor = true;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.Velocity = Vector2.Zero;
        _nitroCharacter.PlayAnimation("idle");
        
        // _controller
        
        // Act
        _stateMachine.SetCurrentState("idle");
        _stateMachine.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        _nitroCharacter.CurrentAnimation.ShouldBe("idle");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_move_left_when_left_D_Pad_is_pressed()
    {
        // Arrange
        _nitroCharacter.OnFloor = true;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.Velocity = Vector2.Zero;
        
        _controller.IsDPadLeftPressed.Returns(true);
        
        // Act
        _stateMachine.SetCurrentState("idle");
        _stateMachine.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Left);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(-BaseNitroState.MovementSpeed, 0));
        _nitroCharacter.CurrentAnimation.ShouldBe("walking");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_stop_moving_when_left_D_Pad_is_not_pressed()
    {
        // Arrange
        _nitroCharacter.OnFloor = true;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.Velocity = new Vector2(-BaseNitroState.MovementSpeed, 0);
        
        _controller.IsDPadLeftPressed.Returns(false);
    
        // Act
        _stateMachine.SetCurrentState("walking");
        _stateMachine.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        _nitroCharacter.CurrentAnimation.ShouldBe("idle");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_move_right_when_right_D_Pad_is_pressed()
    {
        // Arrange
        _nitroCharacter.OnFloor = true;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.Velocity = Vector2.Zero;
        
        _controller.IsDPadRightPressed.Returns(true);
        
        // Act
        _stateMachine.SetCurrentState("idle");
        _stateMachine.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(BaseNitroState.MovementSpeed, 0));
        _nitroCharacter.CurrentAnimation.ShouldBe("walking");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_stop_moving_when_right_D_Pad_is_not_pressed()
    {
        // Arrange
        _nitroCharacter.OnFloor = true;
        _nitroCharacter.Direction = NitroDirection.Right;
        // _nitroCharacter.State = NitroState.Walking;
        _nitroCharacter.Velocity = new Vector2(BaseNitroState.MovementSpeed, 0);
        
        // _controller
        
        // Act
        _stateMachine.SetCurrentState("walking");
        _stateMachine.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        // _nitroCharacter.State.ShouldBe(NitroState.Idle);
        _nitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        _nitroCharacter.CurrentAnimation.ShouldBe("idle");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_switches_to_flying_after_launching_while_DPad_is_pressed()
    {
        // Arrange
        _nitroCharacter.OnFloor = false;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.Velocity = new Vector2(BaseNitroState.MovementSpeed, BaseNitroState.MaxRisingVelocity);
        _nitroCharacter.IsLaunchingAnimationComplete = true;
        
        _controller.IsDPadRightPressed.Returns(true);
        _controller.IsButtonBPressed.Returns(true);
        
        // Act
        _stateMachine.SetCurrentState("launching");
        _stateMachine.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(BaseNitroState.MovementSpeed, BaseNitroState.MaxRisingVelocity));
        _nitroCharacter.CurrentAnimation.ShouldBe("flying");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_fall_if_idle_and_no_buttons_pressed()
    {
        // Arrange
        _nitroCharacter.OnFloor = false;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.Velocity = Vector2.Zero;
        
        // Controller not being used
        // _controller
        
        // Act
        _stateMachine.SetCurrentState("idle");
        _stateMachine.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(0, BaseNitroState.FallingForce));
        _nitroCharacter.CurrentAnimation.ShouldBe("falling");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_stop_falling_if_on_the_floor()
    {
        // Arrange
        _nitroCharacter.OnFloor = true;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.Velocity = new Vector2(0, BaseNitroState.MaxFallingVelocity);
        
        // Controller not being used
        // _controller
        
        // Act
        _stateMachine.SetCurrentState("falling");
        _stateMachine.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        _nitroCharacter.CurrentAnimation.ShouldBe("idle");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_walking_animation_should_continually_play()
    {
        // Arrange
        _nitroCharacter.OnFloor = true;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.Velocity = Vector2.Zero;
        
        // Controller not being used
        _controller.IsDPadRightPressed.Returns(true);
        
        // Act
        _stateMachine.SetCurrentState("idle");
        _stateMachine.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(BaseNitroState.MovementSpeed, 0));
        _nitroCharacter.CurrentAnimation.ShouldBe("walking");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
        _nitroCharacter.AnimationWasPaused.ShouldBe(false);
    }
}
