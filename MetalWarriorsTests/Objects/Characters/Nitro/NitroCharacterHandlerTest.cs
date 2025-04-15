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
    private readonly INitroCharacter _nitroCharacter = Substitute.For<INitroCharacter>();
    
    [Fact]
    public void Nitro_should_rise_constantly_when_launching()
    {
        // Arrange
        _controller.IsButtonBPressed.Returns(true);

        
        _nitroCharacter.State.Returns(NitroState.Idle);
        _nitroCharacter.IsOnFloor().Returns(true);
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
    
        // Assert
        VerifyMutatorsWereOnlyCalledOnce();
        
        _nitroCharacter.Received().Direction = NitroDirection.Right;
        _nitroCharacter.Received().State = NitroState.Launching;
        _nitroCharacter.Received().Velocity = new Vector2(0, NitroCharacterHandler.MaxRisingVelocity);
        _nitroCharacter.Received().PlayAnimation("launching");
    }

    [Fact]
    public void Nitro_should_keep_rising_constantly_when_launching()
    {
        _controller.IsButtonBPressed.Returns(true);
        
        _nitroCharacter.State.Returns(NitroState.Launching);
        _nitroCharacter.IsOnFloor().Returns(false);
        _nitroCharacter.Velocity.Returns(new Vector2(0, NitroCharacterHandler.MaxRisingVelocity));

        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
    
        // Assert
        
        // Assert mutations to Direction
        _nitroCharacter.DidNotReceive().Direction = Arg.Any<NitroDirection>(); // No direction was given

        // Assert mutations to State
        _nitroCharacter.DidNotReceive().State = Arg.Any<NitroState>();

        // Assert mutations to Velocity 
        _nitroCharacter.Received(1).Velocity = Arg.Any<Vector2>(); // The Velocity was only set once
        _nitroCharacter.Received().Velocity = new Vector2(0, NitroCharacterHandler.MaxRisingVelocity);

        // Assert calls to PlayAnimation
        _nitroCharacter.Received(1).PlayAnimation(Arg.Any<string>()); // The animation was only changed once
        _nitroCharacter.Received().PlayAnimation("launching");
    }

    [Fact]
    public void Nitro_should_change_from_launching_to_flying_after_the_launching_animation_is_finished()
    {
        _controller.IsButtonBPressed.Returns(true);
        
        _nitroCharacter.State.Returns(NitroState.Launching);
        _nitroCharacter.IsOnFloor().Returns(false);
        _nitroCharacter.Velocity.Returns(new Vector2(0, NitroCharacterHandler.MaxRisingVelocity));
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.LaunchingAnimationFinished();
        sut.PhysicsProcess(0.1f);
    
        // Assert
        
        // Assert mutations to Direction
        _nitroCharacter.DidNotReceive().Direction = Arg.Any<NitroDirection>(); // No direction was given

        // Assert mutations to State
        _nitroCharacter.Received(1).State = Arg.Any<NitroState>();
        _nitroCharacter.Received().State = NitroState.Flying;

        // Assert mutations to Velocity 
        _nitroCharacter.Received(1).Velocity = Arg.Any<Vector2>(); // The Velocity was only set once
        _nitroCharacter.Received().Velocity = new Vector2(0, NitroCharacterHandler.MaxRisingVelocity);

        // Assert calls to PlayAnimation
        _nitroCharacter.Received(1).PlayAnimation(Arg.Any<string>()); // The animation was only changed once
        _nitroCharacter.Received().PlayAnimation("flying");
    }

    [Fact]
    public void Nitro_should_decelerate_when_jetting_is_stopped()
    {
        // Arrange
        _nitroCharacter.State.Returns(NitroState.Flying);
        _nitroCharacter.IsOnFloor().Returns(false);
        _nitroCharacter.Velocity.Returns(new Vector2(0, NitroCharacterHandler.MaxRisingVelocity));
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        
        // Assert mutations to Direction
        _nitroCharacter.DidNotReceive().Direction = Arg.Any<NitroDirection>(); // No direction was given

        // Assert mutations to State
        _nitroCharacter.Received(1).State = Arg.Any<NitroState>();
        _nitroCharacter.Received().State = NitroState.Falling;

        // Assert mutations to Velocity 
        _nitroCharacter.Received(1).Velocity = Arg.Any<Vector2>(); // The Velocity was only set once
        _nitroCharacter.Received().Velocity = new Vector2(0, NitroCharacterHandler.MaxRisingVelocity + NitroCharacterHandler.FallingForce);

        // Assert calls to PlayAnimation
        _nitroCharacter.Received(1).PlayAnimation(Arg.Any<string>()); // The animation was only changed once
        _nitroCharacter.Received().PlayAnimation("falling");
    }
    
    [Fact]
    public void Nitro_should_accelerate_when_jetting_is_started_again()
    {
        // Arrange
        
        _controller.IsButtonBPressed.Returns(true);
        
        _nitroCharacter.State.Returns(NitroState.Flying);
        _nitroCharacter.IsOnFloor().Returns(false);
        _nitroCharacter.Velocity.Returns(new Vector2(0, NitroCharacterHandler.MaxFallingVelocity));
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        
        // Assert mutations to Direction
        _nitroCharacter.DidNotReceive().Direction = Arg.Any<NitroDirection>(); // No direction was given

        // Assert mutations to State
        _nitroCharacter.DidNotReceive().State = Arg.Any<NitroState>();

        // Assert mutations to Velocity 
        _nitroCharacter.Received(1).Velocity = Arg.Any<Vector2>(); // The Velocity was only set once
        _nitroCharacter.Received().Velocity = new Vector2(0, NitroCharacterHandler.MaxFallingVelocity - NitroCharacterHandler.BoostingForce);

        // Assert calls to PlayAnimation
        _nitroCharacter.Received(1).PlayAnimation(Arg.Any<string>()); // The animation was only changed once
        _nitroCharacter.Received().PlayAnimation("flying");
    }
    
    [Fact]
    public void Nitro_should_fall_after_reaching_the_apex_after_thrust_is_cut()
    {
        // Arrange
        _nitroCharacter.State.Returns(NitroState.Flying);
        _nitroCharacter.IsOnFloor().Returns(false);
        _nitroCharacter.Velocity.Returns(new Vector2(0, 0));
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        
        // Assert mutations to Direction
        _nitroCharacter.DidNotReceive().Direction = Arg.Any<NitroDirection>(); // No direction was given

        // Assert mutations to State
        _nitroCharacter.Received(1).State = Arg.Any<NitroState>();
        _nitroCharacter.Received().State = NitroState.Falling;

        // Assert mutations to Velocity 
        _nitroCharacter.Received(1).Velocity = Arg.Any<Vector2>(); // The Velocity was only set once
        _nitroCharacter.Received().Velocity = new Vector2(0, NitroCharacterHandler.FallingForce);

        // Assert calls to PlayAnimation
        _nitroCharacter.Received(1).PlayAnimation(Arg.Any<string>()); // The animation was only changed once
        _nitroCharacter.Received().PlayAnimation("falling");
    }
    
    [Fact]
    public void Nitro_should_not_fall_too_fast()
    {
        // Arrange
        _nitroCharacter.IsOnFloor().Returns(false);
        _nitroCharacter.State.Returns(NitroState.Falling);
        _nitroCharacter.Velocity.Returns(new Vector2(0, NitroCharacterHandler.MaxFallingVelocity + 10));
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        
        // Assert mutations to Direction
        _nitroCharacter.DidNotReceive().Direction = Arg.Any<NitroDirection>(); // No direction was given

        // Assert mutations to State
        _nitroCharacter.Received(1).State = Arg.Any<NitroState>();
        _nitroCharacter.Received().State = NitroState.Falling;

        // Assert mutations to Velocity 
        _nitroCharacter.Received(1).Velocity = Arg.Any<Vector2>(); // The Velocity was only set once
        _nitroCharacter.Received().Velocity = new Vector2(0, NitroCharacterHandler.MaxFallingVelocity);

        // Assert calls to PlayAnimation
        _nitroCharacter.Received(1).PlayAnimation(Arg.Any<string>()); // The animation was only changed once
        _nitroCharacter.Received().PlayAnimation("falling");
    }
    
    [Fact]
    public void Nitro_should_not_accelerate_too_fast()
    {
        // Arrange
        _controller.IsButtonBPressed.Returns(true);
        
        _nitroCharacter.IsOnFloor().Returns(false);
        _nitroCharacter.State.Returns(NitroState.Flying);
        _nitroCharacter.Velocity.Returns(new Vector2(0, NitroCharacterHandler.MaxRisingVelocity + 10)); // Just under max rising velocity
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        
        // Assert mutations to Direction
        _nitroCharacter.DidNotReceive().Direction = Arg.Any<NitroDirection>(); // No direction was given

        // Assert mutations to State
        _nitroCharacter.DidNotReceive().State = Arg.Any<NitroState>();

        // Assert mutations to Velocity 
        _nitroCharacter.Received(1).Velocity = Arg.Any<Vector2>(); // The Velocity was only set once
        _nitroCharacter.Received().Velocity = new Vector2(0, NitroCharacterHandler.MaxRisingVelocity);

        // Assert calls to PlayAnimation
        _nitroCharacter.Received(1).PlayAnimation(Arg.Any<string>()); // The animation was only changed once
        _nitroCharacter.Received().PlayAnimation("flying");
    }
    
    [Fact]
    public void Nitro_should_not_exceed_max_rising_velocity()
    {
        // Arrange
        _controller.IsButtonBPressed.Returns(true);
        
        _nitroCharacter.IsOnFloor().Returns(false);
        _nitroCharacter.State.Returns(NitroState.Flying);
        _nitroCharacter.Velocity.Returns(new Vector2(0, NitroCharacterHandler.MaxRisingVelocity - 10)); // Just over max rising velocity
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        
        // Assert mutations to Direction
        _nitroCharacter.DidNotReceive().Direction = Arg.Any<NitroDirection>(); // No direction was given

        // Assert mutations to State
        _nitroCharacter.DidNotReceive().State = Arg.Any<NitroState>();

        // Assert mutations to Velocity 
        _nitroCharacter.Received(1).Velocity = Arg.Any<Vector2>(); // The Velocity was only set once
        _nitroCharacter.Received().Velocity = new Vector2(0, NitroCharacterHandler.MaxRisingVelocity);

        // Assert calls to PlayAnimation
        _nitroCharacter.Received(1).PlayAnimation(Arg.Any<string>()); // The animation was only changed once
        _nitroCharacter.Received().PlayAnimation("flying");
    }
    
    [Fact]
    public void Nitro_should_not_go_farther_down_if_already_on_the_floor()
    {
        // Arrange
        _nitroCharacter.IsOnFloor().Returns(true);
        _nitroCharacter.State.Returns(NitroState.Idle);
        _nitroCharacter.Velocity.Returns(new Vector2(0, 0)); // Just over max rising velocity
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert

        // Assert mutations to Direction
        _nitroCharacter.DidNotReceive().Direction = Arg.Any<NitroDirection>(); // No direction was given

        // Assert mutations to State
        _nitroCharacter.DidNotReceive().State = Arg.Any<NitroState>();

        // Assert mutations to Velocity 
        _nitroCharacter.Received(1).Velocity = Arg.Any<Vector2>(); // The Velocity was only set once
        _nitroCharacter.Received().Velocity = new Vector2(0, 0);

        // Assert calls to PlayAnimation
        _nitroCharacter.Received(1).PlayAnimation(Arg.Any<string>()); // The animation was only changed once
        _nitroCharacter.Received().PlayAnimation("idle");
    }
    
    [Fact]
    public void Nitro_should_move_left_when_left_D_Pad_is_pressed()
    {
        // Arrange
        _controller.IsDPadLeftPressed.Returns(true);
        
        _nitroCharacter.IsOnFloor().Returns(true);
        _nitroCharacter.State.Returns(NitroState.Idle);
        _nitroCharacter.Velocity.Returns(new Vector2(0, 0)); // Just over max rising velocity
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        
        // Assert mutations to Direction
        _nitroCharacter.Received(1).Direction = Arg.Any<NitroDirection>();
        _nitroCharacter.Received().Direction = NitroDirection.Left;

        // Assert mutations to State
        _nitroCharacter.DidNotReceive().State = Arg.Any<NitroState>();

        // Assert mutations to Velocity 
        _nitroCharacter.Received(1).Velocity = Arg.Any<Vector2>(); // The Velocity was only set once
        _nitroCharacter.Received().Velocity = new Vector2(-NitroCharacterHandler.MovementSpeed, 0);

        // Assert calls to PlayAnimation
        _nitroCharacter.Received(1).PlayAnimation(Arg.Any<string>()); // The animation was only changed once
        _nitroCharacter.Received().PlayAnimation("walking");
    }
    
    [Fact]
    public void Nitro_should_stop_moving_when_left_D_Pad_is_not_pressed()
    {
        // Arrange
        _controller.IsDPadLeftPressed.Returns(false);
        
        _nitroCharacter.IsOnFloor().Returns(true);
        _nitroCharacter.State.Returns(NitroState.Walking);
        _nitroCharacter.Velocity.Returns(new Vector2(-NitroCharacterHandler.MovementSpeed, 0)); // Just over max rising velocity

        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        
        // Assert mutations to Direction
        _nitroCharacter.DidNotReceive().Direction = Arg.Any<NitroDirection>();

        // Assert mutations to State
        _nitroCharacter.Received(1).State = Arg.Any<NitroState>();
        _nitroCharacter.Received().State = NitroState.Idle;

        // Assert mutations to Velocity 
        _nitroCharacter.Received(1).Velocity = Arg.Any<Vector2>(); // The Velocity was only set once
        _nitroCharacter.Received().Velocity = new Vector2(0, 0);

        // Assert calls to PlayAnimation
        _nitroCharacter.Received(1).PlayAnimation(Arg.Any<string>()); // The animation was only changed once
        _nitroCharacter.Received().PlayAnimation("idle");
    }
    
    [Fact]
    public void Nitro_should_move_right_when_right_D_Pad_is_pressed()
    {
        // Arrange
        _controller.IsDPadRightPressed.Returns(true);
        
        _nitroCharacter.IsOnFloor().Returns(true);
        _nitroCharacter.State.Returns(NitroState.Idle);
        _nitroCharacter.Velocity.Returns(new Vector2(0, 0));
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        
        // Assert mutations to Direction
        _nitroCharacter.Received(1).Direction = Arg.Any<NitroDirection>();
        _nitroCharacter.Received().Direction = NitroDirection.Right;

        // Assert mutations to State
        _nitroCharacter.Received(1).State = Arg.Any<NitroState>();
        _nitroCharacter.Received().State = NitroState.Walking;

        // Assert mutations to Velocity 
        _nitroCharacter.Received(1).Velocity = Arg.Any<Vector2>(); // The Velocity was only set once
        _nitroCharacter.Received().Velocity = new Vector2(NitroCharacterHandler.MovementSpeed, 0);

        // Assert calls to PlayAnimation
        _nitroCharacter.Received(1).PlayAnimation(Arg.Any<string>()); // The animation was only changed once
        _nitroCharacter.Received().PlayAnimation("walking");
    }
    
    [Fact]
    public void Nitro_should_stop_moving_when_right_D_Pad_is_not_pressed()
    {
        // Arrange
        _nitroCharacter.IsOnFloor().Returns(true);
        _nitroCharacter.State.Returns(NitroState.Walking);
        _nitroCharacter.Velocity.Returns(new Vector2(NitroCharacterHandler.MovementSpeed, 0));
        
        // Act
        var sut = new NitroCharacterHandler(_controller, _nitroCharacter, _consolePrinter);
        sut.PhysicsProcess(0.1f);
        
        // Assert
        
        // Assert mutations to Direction
        _nitroCharacter.DidNotReceive().Direction = Arg.Any<NitroDirection>();

        // Assert mutations to State
        _nitroCharacter.Received(1).State = Arg.Any<NitroState>();
        _nitroCharacter.Received().State = NitroState.Idle;

        // Assert mutations to Velocity 
        _nitroCharacter.Received(1).Velocity = Arg.Any<Vector2>(); // The Velocity was only set once
        _nitroCharacter.Received().Velocity = new Vector2(0, 0);

        // Assert calls to PlayAnimation
        _nitroCharacter.Received(1).PlayAnimation(Arg.Any<string>()); // The animation was only changed once
        _nitroCharacter.Received().PlayAnimation("idle");
    }
    
    private void VerifyMutatorsWereOnlyCalledOnce()
    {
        _nitroCharacter.Received(1).Direction = Arg.Any<NitroDirection>();
        _nitroCharacter.Received(1).State = Arg.Any<NitroState>();
        _nitroCharacter.Received(1).Velocity = Arg.Any<Vector2>();
        _nitroCharacter.Received(1).PlayAnimation(Arg.Any<string>());
    }
}
