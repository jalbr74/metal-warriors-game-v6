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
        nitro.Velocity.Y.Should().Be(global::Nitro.JetVelocity);
    
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }

    [Test]
    public void Nitro_should_decelerate_when_jetting_is_stopped()
    {
        // Arrange
        var (nitro, nitroBehavior) = new NitroBuilder()
            .WithOnFloor(false)
            .WithVelocity(new Vector2(0, global::Nitro.JetVelocity))
            .Build();
    
        // Act
        nitro._PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.Should().Be(0);
        nitro.Velocity.Y.Should().BeLessThan(0);
        nitro.Velocity.Y.Should().BeGreaterThan(global::Nitro.JetVelocity);
    
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
            .WithVelocity(new Vector2(0, global::Nitro.TerminalFallVelocity + 10))
            .Build();
    
        // Act
        nitro._PhysicsProcess(0.1f);
        
        // Assert
        nitro.Velocity.X.Should().Be(0);
        nitro.Velocity.Y.Should().Be(global::Nitro.TerminalFallVelocity);
    
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }
}
