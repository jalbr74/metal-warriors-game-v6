using Godot;
using MetalWarriors.Objects.Characters;
using MetalWarriors.Objects.Characters.Nitro;
using MetalWarriors.Objects.Characters.Nitro.States;
using MetalWarriors.Utils;
using MetalWarriorsTests.Utils;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Nitro.States;

public class NitroFlyingStateTest(ITestOutputHelper testOutputHelper) : BaseNitroStateTest(testOutputHelper)
{
}
