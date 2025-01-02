using FluentAssertions;
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
        nitro.Velocity.Y.Should().Be(global::Nitro.MaxRisingVelocity);
    
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_decelerate_when_jetting_is_stopped()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithOnFloor(false)
            .WithVelocity(new Vector2(0, global::Nitro.MaxRisingVelocity))
            .Build();
    
        // Act
        nitro._PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.Should().Be(0);
        nitro.Velocity.Y.Should().BeLessThan(0);
        nitro.Velocity.Y.Should().BeGreaterThan(global::Nitro.MaxRisingVelocity);
    
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_accelerate_when_jetting_is_started_again()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithOnFloor(false)
            .WithActionPressed("Button_B")
            .WithVelocity(new Vector2(0, global::Nitro.MaxFallingVelocity))
            .Build();
    
        // Act
        nitro._PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.Should().Be(0);
        nitro.Velocity.Y.Should().BeGreaterThan(0);
        nitro.Velocity.Y.Should().BeLessThan(global::Nitro.MaxFallingVelocity);
    
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
            .WithVelocity(new Vector2(0, global::Nitro.MaxFallingVelocity + 10))
            .Build();
    
        // Act
        nitro._PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.Should().Be(0);
        nitro.Velocity.Y.Should().Be(global::Nitro.MaxFallingVelocity);
    
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_not_accelerate_too_fast()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithOnFloor(false)
            .WithActionPressed("Button_B")
            .WithVelocity(new Vector2(0, global::Nitro.MaxRisingVelocity + 10))
            .Build();
    
        // Act
        nitro._PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.Should().Be(0);
        nitro.Velocity.Y.Should().Be(global::Nitro.MaxRisingVelocity);
    
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }
}
