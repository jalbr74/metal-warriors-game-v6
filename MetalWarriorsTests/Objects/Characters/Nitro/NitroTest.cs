﻿using FluentAssertions;
using Godot;
using Godot.Utils;
using Moq;
using NUnit.Framework;

namespace MetalWarriorsTests.Objects.Characters.Nitro;

public class NitroTest
{
    [Test]
    public void Nitro_should_rise_constantly_when_jetting()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithActionPressed("Button_B")
            .WithOnFloor(true)
            .Build();
    
        // Act
        nitro._PhysicsProcess(0.1f);
    
        // Assert
        nitro.Velocity.X.Should().Be(0);
        nitro.Velocity.Y.Should().Be(NitroDefaults.MaxRisingVelocity);
    
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_decelerate_when_jetting_is_stopped()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithOnFloor(false)
            .WithVelocity(new Vector2(0, NitroDefaults.MaxRisingVelocity))
            .Build();
    
        // Act
        nitro._PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.Should().Be(0);
        nitro.Velocity.Y.Should().BeLessThan(0);
        nitro.Velocity.Y.Should().BeGreaterThan(NitroDefaults.MaxRisingVelocity);
    
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_accelerate_when_jetting_is_started_again()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithOnFloor(false)
            .WithActionPressed("Button_B")
            .WithVelocity(new Vector2(0, NitroDefaults.MaxFallingVelocity))
            .Build();
    
        // Act
        nitro._PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.Should().Be(0);
        nitro.Velocity.Y.Should().BeGreaterThan(0);
        nitro.Velocity.Y.Should().BeLessThan(NitroDefaults.MaxFallingVelocity);
    
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_fall_when_no_longer_decelerating()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithOnFloor(false)
            .WithVelocity(new Vector2(0, 0))
            .Build();
    
        // Act
        nitro._PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.Should().Be(0);
        nitro.Velocity.Y.Should().BeGreaterThan(0);
    
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_not_fall_too_fast()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithOnFloor(false)
            .WithVelocity(new Vector2(0, NitroDefaults.MaxFallingVelocity + 10))
            .Build();
    
        // Act
        nitro._PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.Should().Be(0);
        nitro.Velocity.Y.Should().Be(NitroDefaults.MaxFallingVelocity);
    
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_not_accelerate_too_fast()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithOnFloor(false)
            .WithActionPressed("Button_B")
            .WithVelocity(new Vector2(0, NitroDefaults.MaxRisingVelocity + 10))
            .Build();
    
        // Act
        nitro._PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.Should().Be(0);
        nitro.Velocity.Y.Should().Be(NitroDefaults.MaxRisingVelocity);
    
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_not_exceed_max_rising_velocity()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithOnFloor(false)
            .WithActionPressed("Button_B")
            .WithVelocity(new Vector2(0, NitroDefaults.MaxRisingVelocity - 10))
            .Build();
    
        // Act
        nitro._PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.Should().Be(0);
        nitro.Velocity.Y.Should().Be(NitroDefaults.MaxRisingVelocity);
    
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_not_go_farther_down_if_already_on_the_floor()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithOnFloor(true)
            .WithVelocity(new Vector2(0, 0))
            .Build();

        // Act
        nitro._PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.Should().Be(0);
        nitro.Velocity.Y.Should().Be(0, because: "he can't go down more if he's on the floor");
    
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_move_left_when_left_D_Pad_is_pressed()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithOnFloor(true)
            .WithVelocity(new Vector2(0, 0))
            .WithAnimations(new AnimatedSprite2D())
            .WithActionPressed("D_Pad_Left")
            .Build();
    
        // Act
        nitro._PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.Should().Be(-NitroDefaults.MovementSpeed);
        nitro.Velocity.Y.Should().Be(0, because: "he's on the floor");
        nitro.NitroAnimations.Scale.X.Should().BeNegative(because: "he's moving left");
    
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_stop_moving_when_left_D_Pad_is_not_pressed()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithOnFloor(true)
            .WithVelocity(new Vector2(-NitroDefaults.MovementSpeed, 0))
            .Build();
    
        // Act
        nitro._PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.Should().Be(0, because: "the left D_Pad is not being pressed");
        nitro.Velocity.Y.Should().Be(0, because: "he's on the floor");
    
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_move_right_when_right_D_Pad_is_pressed()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithOnFloor(true)
            .WithVelocity(new Vector2(0, 0))
            .WithAnimations(new AnimatedSprite2D())
            .WithActionPressed("D_Pad_Right")
            .Build();
    
        // Act
        nitro._PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.Should().Be(NitroDefaults.MovementSpeed);
        nitro.Velocity.Y.Should().Be(0, because: "he's on the floor");
        nitro.NitroAnimations.Scale.X.Should().BePositive(because: "he's moving right");
    
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_stop_moving_when_right_D_Pad_is_not_pressed()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithOnFloor(true)
            .WithVelocity(new Vector2(NitroDefaults.MovementSpeed, 0))
            .Build();
    
        // Act
        nitro._PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.Should().Be(0, because: "the right D_Pad is not being pressed");
        nitro.Velocity.Y.Should().Be(0, because: "he's on the floor");
    
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }
}
