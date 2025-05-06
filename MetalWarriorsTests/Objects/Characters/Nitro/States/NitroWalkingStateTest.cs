using System.Numerics;
using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriors.Objects.Characters.Nitro.States;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Abstractions;
using Vector2 = Godot.Vector2;

namespace MetalWarriorsTests.Objects.Characters.Nitro.States;

public class NitroWalkingStateTest(ITestOutputHelper testOutputHelper) : BaseNitroStateTest(testOutputHelper)
{
    [Fact]
    public void Nitro_should_move_left_when_left_D_Pad_is_pressed()
    {
        // Arrange
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = Vector2.Zero;
        
        Controller.IsDPadLeftPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroIdleState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingLeft);
        NitroCharacter.Velocity.ShouldBe(new Vector2(-BaseNitroState.MovementSpeed, 0));
        NitroCharacter.CurrentAnimation.ShouldBe("walking");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_should_move_right_when_right_D_Pad_is_pressed()
    {
        // Arrange
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = Vector2.Zero;
        
        Controller.IsDPadRightPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroIdleState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(BaseNitroState.MovementSpeed, 0));
        NitroCharacter.CurrentAnimation.ShouldBe("walking");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
    }
    
    [Fact]
    public void Nitro_walking_animation_should_continually_play()
    {
        // Arrange
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = Vector2.Zero;
        
        // Controller not being used
        Controller.IsDPadRightPressed.Returns(true);
        
        // Act
        StateMachine.SetCurrentState(typeof(NitroIdleState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.Direction.ShouldBe(NitroDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(new Vector2(BaseNitroState.MovementSpeed, 0));
        NitroCharacter.CurrentAnimation.ShouldBe("walking");
        NitroCharacter.PlayedAnimations.Count.ShouldBe(1);
        NitroCharacter.AnimationWasPaused.ShouldBe(false);
    }
    
    [Fact]
    public void Nitro_current_animation_should_reset_when_transitioning_to_walking()
    {
        // Arrange
        NitroCharacter.OnFloor = true;
        NitroCharacter.Direction = NitroDirection.FacingRight;
        NitroCharacter.Velocity = Vector2.Zero;
        NitroCharacter.AnimationOffset = new Vector2(100, 100);
        
        StateMachine.SetCurrentState(typeof(NitroIdleState));
        Controller.IsDPadRightPressed.Returns(true);
        
        // Act
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.AnimationOffset.ShouldBe(Vector2.Zero);
    }
    
    public static IEnumerable<object[]> GunOffsetData => new List<object[]>
    {
        new object[] { 0, NitroWalkingState.GunOffsetAtFrame0 },
        new object[] { 1, NitroWalkingState.GunOffsetAtFrame1 },
        new object[] { 2, NitroWalkingState.GunOffsetAtFrame2 },
        new object[] { 3, NitroWalkingState.GunOffsetAtFrame3 },
        new object[] { 4, NitroWalkingState.GunOffsetAtFrame4 },
        new object[] { 5, NitroWalkingState.GunOffsetAtFrame5 },
        new object[] { 6, NitroWalkingState.GunOffsetAtFrame6 },
        new object[] { 7, NitroWalkingState.GunOffsetAtFrame7 }
    };

    [Theory, MemberData(nameof(GunOffsetData))]
    public void Nitro_gun_position_is_correct_for_a_given_animation_frame(int frame, Vector2 expectedPosition)
    {
        // Arrange
        NitroCharacter.CurrentAnimationFrame = frame;
        NitroCharacter.GunOffset = Vector2.Zero;
        NitroCharacter.OnFloor = true;
        Controller.IsDPadRightPressed.Returns(true);
        NitroCharacter.GunOffset = new Vector2(100, 100);

        // Act
        StateMachine.SetCurrentState(typeof(NitroWalkingState));
        StateMachine.PhysicsProcess(0.1f);
        
        // Assert
        NitroCharacter.GunOffset.ShouldBe(expectedPosition);
    }
}
