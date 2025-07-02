using Godot;
using MetalWarriors.Objects.Characters;
using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriors.Objects.Characters.Nitro.States;
using MetalWarriors.Utils;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Nitro.States;

public class NitroPoweringDownStateTest(ITestOutputHelper testOutputHelper) : BaseNitroStateTest(testOutputHelper)
{
    [Fact]
    public void Test()
    {
        // Arrange
        var nitroCharacter = Substitute.For<INitroCharacter>();
        var controller = Substitute.For<ISnesController>();

        nitroCharacter.IsAnimationFinished.Returns(true);
        
        // Act
        new NitroPoweringDownState(nitroCharacter).ProcessOrPass(0.1f);

        // Assert
        // nitroCharacter.Velocity.ShouldBe();
    }
}
