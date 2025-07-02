using Godot;
using MetalWarriors.Objects.Characters;
using MetalWarriors.Objects.Characters.Pilot.States;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MetalWarriorsTests.Objects.Characters.Pilot.States;

public class PilotIdleStateTest(ITestOutputHelper output) : BasePilotStateTest(output)
{
}
