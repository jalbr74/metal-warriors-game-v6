using Godot;
using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriors.Utils;
using MetalWarriorsTests.Utils;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Nitro;

public class NitroCharacterHandlerTest(ITestOutputHelper testOutputHelper)
{
    private readonly TestOutputConsolePrinter _consolePrinter = new(testOutputHelper);
    
    private readonly ISnesController _controller = Substitute.For<ISnesController>();
    private readonly NitroCharacterImplForTesting _nitroCharacter = new();
    
    [Fact]
    public void Nitro_should_rise_constantly_when_launching()
    {
        // Arrange
        _nitroCharacter.OnFloor = true;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.State = NitroState.Idle;
        _nitroCharacter.Velocity = Vector2.Zero;
        
        _controller.IsButtonBPressed.Returns(true);
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
    
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.State.ShouldBe(NitroState.Launching);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(0, NitroCharacterHandler.MaxRisingVelocity));
        _nitroCharacter.CurrentAnimation.ShouldBe("launching");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }

    [Fact]
    public void Nitro_should_keep_rising_constantly_when_launching()
    {
        // Arrange
        _nitroCharacter.OnFloor = false;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.State = NitroState.Launching;
        _nitroCharacter.Velocity = new Vector2(0, NitroCharacterHandler.MaxRisingVelocity);
        
        _controller.IsButtonBPressed.Returns(true);
    
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
    
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.State.ShouldBe(NitroState.Launching);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(0, NitroCharacterHandler.MaxRisingVelocity));
        _nitroCharacter.CurrentAnimation.ShouldBe("launching");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_change_from_launching_to_flying_after_the_launching_animation_is_finished()
    {
        _nitroCharacter.OnFloor = false;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.State = NitroState.Launching;
        _nitroCharacter.Velocity = new Vector2(0, NitroCharacterHandler.MaxRisingVelocity);
        
        _controller.IsButtonBPressed.Returns(true);
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.LaunchingAnimationFinished();
        sut.PhysicsProcess(0.1f);
    
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.State.ShouldBe(NitroState.Flying);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(0, NitroCharacterHandler.MaxRisingVelocity));
        _nitroCharacter.CurrentAnimation.ShouldBe("flying");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_decelerate_when_jetting_is_stopped()
    {
        // Arrange
        _nitroCharacter.OnFloor = false;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.State = NitroState.Flying;
        _nitroCharacter.Velocity = new Vector2(0, NitroCharacterHandler.MaxRisingVelocity);
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.State.ShouldBe(NitroState.Falling);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(0, NitroCharacterHandler.MaxRisingVelocity + NitroCharacterHandler.FallingForce));
        _nitroCharacter.CurrentAnimation.ShouldBe("falling");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_accelerate_when_jetting_is_started_again()
    {
        // Arrange
        _nitroCharacter.OnFloor = false;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.State = NitroState.Flying;
        _nitroCharacter.Velocity = new Vector2(0, NitroCharacterHandler.MaxFallingVelocity);
        
        _controller.IsButtonBPressed.Returns(true);
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.State.ShouldBe(NitroState.Flying);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(0, NitroCharacterHandler.MaxFallingVelocity - NitroCharacterHandler.BoostingForce));
        _nitroCharacter.CurrentAnimation.ShouldBe("flying");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_fall_after_reaching_the_apex_after_thrust_is_cut()
    {
        // Arrange
        _nitroCharacter.OnFloor = false;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.State = NitroState.Flying;
        _nitroCharacter.Velocity = Vector2.Zero;
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.State.ShouldBe(NitroState.Falling);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(0, NitroCharacterHandler.FallingForce));
        _nitroCharacter.CurrentAnimation.ShouldBe("falling");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_not_fall_too_fast()
    {
        // Arrange
        _nitroCharacter.OnFloor = false;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.State = NitroState.Falling;
        _nitroCharacter.Velocity = new Vector2(0, NitroCharacterHandler.MaxFallingVelocity + 10);
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.State.ShouldBe(NitroState.Falling);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(0, NitroCharacterHandler.MaxFallingVelocity));
        _nitroCharacter.CurrentAnimation.ShouldBe("falling");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_not_accelerate_too_fast()
    {
        // Arrange
        _nitroCharacter.OnFloor = false;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.State = NitroState.Flying;
        _nitroCharacter.Velocity = new Vector2(0, NitroCharacterHandler.MaxRisingVelocity + 10);
        
        _controller.IsButtonBPressed.Returns(true);
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.State.ShouldBe(NitroState.Flying);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(0, NitroCharacterHandler.MaxRisingVelocity));
        _nitroCharacter.CurrentAnimation.ShouldBe("flying");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_not_exceed_max_rising_velocity()
    {
        // Arrange
        _nitroCharacter.OnFloor = false;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.State = NitroState.Flying;
        _nitroCharacter.Velocity = new Vector2(0, NitroCharacterHandler.MaxRisingVelocity - 10);
        
        _controller.IsButtonBPressed.Returns(true);
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.State.ShouldBe(NitroState.Flying);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(0, NitroCharacterHandler.MaxRisingVelocity));
        _nitroCharacter.CurrentAnimation.ShouldBe("flying");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_not_go_farther_down_if_already_on_the_floor()
    {
        // Arrange
        _nitroCharacter.OnFloor = true;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.State = NitroState.Idle;
        _nitroCharacter.Velocity = Vector2.Zero;
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.State.ShouldBe(NitroState.Idle);
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
        _nitroCharacter.State = NitroState.Idle;
        _nitroCharacter.Velocity = Vector2.Zero;
        
        _controller.IsDPadLeftPressed.Returns(true);
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Left);
        _nitroCharacter.State.ShouldBe(NitroState.Walking);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(-NitroCharacterHandler.MovementSpeed, 0));
        _nitroCharacter.CurrentAnimation.ShouldBe("walking");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_stop_moving_when_left_D_Pad_is_not_pressed()
    {
        // Arrange
        _nitroCharacter.OnFloor = true;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.State = NitroState.Walking;
        _nitroCharacter.Velocity = new Vector2(-NitroCharacterHandler.MovementSpeed, 0);
        
        _controller.IsDPadLeftPressed.Returns(false);
    
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.State.ShouldBe(NitroState.Idle);
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
        _nitroCharacter.State = NitroState.Idle;
        _nitroCharacter.Velocity = Vector2.Zero;
        
        _controller.IsDPadRightPressed.Returns(true);
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.State.ShouldBe(NitroState.Walking);
        _nitroCharacter.Velocity.ShouldBe(new Vector2(NitroCharacterHandler.MovementSpeed, 0));
        _nitroCharacter.CurrentAnimation.ShouldBe("walking");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_stop_moving_when_right_D_Pad_is_not_pressed()
    {
        // Arrange
        _nitroCharacter.OnFloor = true;
        _nitroCharacter.Direction = NitroDirection.Right;
        _nitroCharacter.State = NitroState.Walking;
        _nitroCharacter.Velocity = new Vector2(NitroCharacterHandler.MovementSpeed, 0);
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        _nitroCharacter.Direction.ShouldBe(NitroDirection.Right);
        _nitroCharacter.State.ShouldBe(NitroState.Idle);
        _nitroCharacter.Velocity.ShouldBe(Vector2.Zero);
        _nitroCharacter.CurrentAnimation.ShouldBe("idle");
        _nitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
}
