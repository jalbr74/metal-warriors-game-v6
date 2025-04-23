using Godot;
using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriors.Objects.Characters.Nitro.States;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Nitro.States;

public class NitroFallingStateTest : BaseNitroStateTest
{
    public NitroFallingStateTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
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
}
