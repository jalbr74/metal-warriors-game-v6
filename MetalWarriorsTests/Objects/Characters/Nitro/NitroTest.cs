using FluentAssertions;
using Godot;
using Godot.Utils;
using Moq;
using NUnit.Framework;

namespace MetalWarriorsTests.Objects.Characters.Nitro;

public class NitroTest
{
    // [Test]
    // public void Input_should_be_mockable()
    // {
    //     var inputState = new Mock<IInputState>();
    //     
    //     inputState.Setup(x => x.IsActionPressed("Foo", false)).Returns(true);
    //     
    //     inputState.Object.IsActionPressed("Foo").Should().BeTrue();
    //     inputState.Object.IsActionPressed("Bar").Should().BeFalse();
    // }
    
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
        nitro.Velocity.Y.Should().BeLessThan(0);
    
        nitroBehavior.Verify(x => x.MoveAndSlide(), Times.Once);
    }
}
