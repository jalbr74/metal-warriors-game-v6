using System.Numerics;
using MetalWarriors.Objects.Characters;
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
}
