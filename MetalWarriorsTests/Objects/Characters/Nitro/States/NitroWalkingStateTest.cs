using Godot;
using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriors.Objects.Characters.Nitro.States;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

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
    public void Nitro_gun_position_at_frame_0()
    {
        VerifyGunPositionAtFrame(0, BaseNitroState.GunPositionAtFrame0);
        VerifyGunPositionAtFrame(1, BaseNitroState.GunPositionAtFrame1);
        VerifyGunPositionAtFrame(2, BaseNitroState.GunPositionAtFrame2);
        VerifyGunPositionAtFrame(3, BaseNitroState.GunPositionAtFrame3);
        VerifyGunPositionAtFrame(4, BaseNitroState.GunPositionAtFrame4);
        VerifyGunPositionAtFrame(5, BaseNitroState.GunPositionAtFrame5);
        VerifyGunPositionAtFrame(6, BaseNitroState.GunPositionAtFrame6);
        VerifyGunPositionAtFrame(7, BaseNitroState.GunPositionAtFrame7);

        return;
        
        void VerifyGunPositionAtFrame(int frame, Vector2 expectedPosition)
        {
            // Arrange
            NitroCharacter.CurrentAnimationFrame = frame;
            NitroCharacter.GunPosition = Vector2.Zero;
            NitroCharacter.OnFloor = true;
            Controller.IsDPadRightPressed.Returns(true);
        
            // Act
            StateMachine.SetCurrentState(typeof(NitroWalkingState));
            StateMachine.PhysicsProcess(0.1f);
        
            // Assert
            NitroCharacter.GunPosition.ShouldBe(expectedPosition);
        }
    }
}
