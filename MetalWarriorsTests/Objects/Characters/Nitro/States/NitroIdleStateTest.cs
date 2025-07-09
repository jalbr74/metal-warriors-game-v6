using Godot;
using MetalWarriors.Objects.Characters;
using MetalWarriors.Objects.Characters.Nitro.States;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Nitro.States;

public class NitroIdleStateTest(ITestOutputHelper testOutputHelper) : BaseNitroStateTest(testOutputHelper)
{
    [Fact]
    public void TestTransitionToWalkingStateWhenWalkingRight()
    {
        // Arrange
        Controller.IsDPadRightPressed.Returns(true);
        
        // Act
        var passedToState = new NitroIdleState(NitroCharacter).ProcessOrPass(0.1f);

        // Assert
        passedToState.ShouldBe(typeof(NitroWalkingState));
    }
    
    [Fact]
    public void TestTransitionToWalkingStateWhenWalkingLeft()
    {
        // Arrange
        Controller.IsDPadLeftPressed.Returns(true);
        
        // Act
        var passedToState = new NitroIdleState(NitroCharacter).ProcessOrPass(0.1f);

        // Assert
        passedToState.ShouldBe(typeof(NitroWalkingState));
    }
    
    [Fact]
    public void TestTransitionToLaunchingState()
    {
        // Arrange
        NitroCharacter.OnFloor.Returns(true);
        
        Controller.IsButtonBPressed.Returns(true);
        
        // Act
        var passedToState = new NitroIdleState(NitroCharacter).ProcessOrPass(0.1f);

        // Assert
        passedToState.ShouldBe(typeof(NitroLaunchingState));
    }
    
    [Fact]
    public void TestTransitionToFallingState()
    {
        // Arrange
        NitroCharacter.OnFloor.Returns(false);
        
        // Act
        var passedToState = new NitroIdleState(NitroCharacter).ProcessOrPass(0.1f);

        // Assert
        passedToState.ShouldBe(typeof(NitroFallingState));
    }

    [Fact]
    public void TestGravityIsWorking()
    {
        // Arrange
        NitroCharacter.OnFloor.Returns(true);
        NitroCharacter.Direction.Returns(CharacterDirection.FacingRight);
        
        // Act
        var passedToState = new NitroIdleState(NitroCharacter).ProcessOrPass(0.1f);

        // Assert
        passedToState.ShouldBeNull();
        NitroCharacter.Direction.ShouldBe(CharacterDirection.FacingRight);
        NitroCharacter.Velocity.ShouldBe(Vector2.Zero);
    }

    [Fact]
    public void TestMechPoweringDown()
    {
        // Arrange
        NitroCharacter.OnFloor.Returns(true);
        NitroCharacter.Direction.Returns(CharacterDirection.FacingRight);
        
        Controller.WasSelectPressed.Returns(true);
        
        // Act
        var passedToState = new NitroIdleState(NitroCharacter).ProcessOrPass(0.1f);

        // Assert
        passedToState.ShouldBe(typeof(NitroPoweringDownState));
    }

    [Fact]
    public void MechShouldStopMovingWhenIdle()
    {
        // Arrange
        NitroCharacter.Velocity = new Vector2(100, 0);
        NitroCharacter.OnFloor.Returns(true);
        NitroCharacter.Direction.Returns(CharacterDirection.FacingRight);
        
        // Act
        var passedToState = new NitroIdleState(NitroCharacter).ProcessOrPass(0.1f);

        // Assert
        passedToState.ShouldBe(null);
        NitroCharacter.Velocity.ShouldBe(Vector2.Zero);
    }
}
