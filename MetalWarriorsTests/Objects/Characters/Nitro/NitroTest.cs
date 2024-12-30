using FluentAssertions;
using Godot;
using Moq;
using NUnit.Framework;

namespace MetalWarriorsTests.Objects.Characters.Nitro;

public class NitroTest
{
    [Test]
    public void Nitro_should_respect_gravity_when_not_on_the_floor() 
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithGravity(new Vector2(0, 1000))
            .WithOnFloor(false)
            .Build();
        
        // Act
        nitro._PhysicsProcess(0.01f);
        
        // Assert
        nitro.Velocity.X.Should().Be(0);
        nitro.Velocity.Y.Should().BeGreaterThan(0);
        
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_ignore_gravity_when_on_the_floor() 
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithGravity(new Vector2(0, 1000))
            .WithOnFloor(true)
            .Build();

        // Act
        nitro._PhysicsProcess(0.01f);
        
        // Assert
        nitro.Velocity.Should().Be(new Vector2(0, 0));
        
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }
    
    [Test]
    public void Nitro_should_jump_when_Button_A_is_pressed_and_is_on_the_floor()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithGravity(new Vector2(0, 1000))
            .WithButtonPressed("Button_A")
            .WithOnFloor(true)
            .Build();
        
        // Act
        nitro._PhysicsProcess(0.01f);

        // Assert
        nitro.Velocity.X.Should().Be(0);
        nitro.Velocity.Y.Should().BeLessThan(0);
        
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_keep_falling_when_Button_A_is_pressed_and_is_not_on_the_floor()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithGravity(new Vector2(0, 1000))
            .WithButtonPressed("Button_A")
            .WithOnFloor(false)
            .Build();

        // Act
        nitro._PhysicsProcess(0.01f);

        // Assert
        nitro.Velocity.X.Should().Be(0);
        nitro.Velocity.Y.Should().BeGreaterThan(0);
        
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_face_and_move_left_when_the_DPad_is_pressed_left()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithMainAnimatedSprite(new AnimatedSprite2D())
            .WithGravity(new Vector2(0, 1000))
            .WithButtonPressedAndHeld("D_Pad_Left")
            .WithOnFloor(true)
            .Build();

        // Act
        nitro._PhysicsProcess(0.01f);

        // Assert
        nitro.Velocity.X.Should().BeLessThan(0);
        nitro.Velocity.Y.Should().Be(0);
        nitro.MainAnimatedSprite.Scale.X.Should().Be(-1);
        
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_face_and_move_right_when_the_DPad_is_pressed_right()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithMainAnimatedSprite(new AnimatedSprite2D())
            .WithGravity(new Vector2(0, 1000))
            .WithButtonPressedAndHeld("D_Pad_Right")
            .WithOnFloor(true)
            .Build();
        
        // Act
        nitro._PhysicsProcess(0.01f);

        // Assert
        nitro.Velocity.X.Should().BeGreaterThan(0);
        nitro.Velocity.Y.Should().Be(0);
        nitro.MainAnimatedSprite.Scale.X.Should().Be(1);
        
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_slow_down_and_stop_when_the_DPad_is_released()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithGravity(new Vector2(0, 1000))
            .WithVelocity(new Vector2(-100, 0))
            .WithOnFloor(true)
            .Build();
        
        // Act
        nitro._PhysicsProcess(0.01f);

        // Assert
        nitro.Velocity.X.Should().BeGreaterThan(-100);
        nitro.Velocity.X.Should().BeLessThanOrEqualTo(0);
        nitro.Velocity.Y.Should().Be(0);

        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }
}
