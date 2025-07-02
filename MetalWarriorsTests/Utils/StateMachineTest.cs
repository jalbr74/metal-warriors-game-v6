using MetalWarriors.Objects.Characters.Nitro.States;
using MetalWarriors.Utils;
using Shouldly;
using Xunit;

namespace MetalWarriorsTests.Utils;

internal class State1 : State
{
    public override Type? ProcessOrPass(double delta)
    {
        return typeof (State2);
    }
}

internal class State2 : State
{
    public override Type? ProcessOrPass(double delta)
    {
        return typeof (State3);
    }
}

internal class State3 : State
{
    public override Type? ProcessOrPass(double delta)
    {
        return null;
    }
}

public class StateMachineTest
{
    [Fact]
    public void TestOne()
    {
        var stateMachine = new StateMachine([
            new State1(),
            new State2(),
            new State3(),
        ], typeof(State1));
        
        stateMachine.PhysicsProcess(0.1f);

        stateMachine.CurrentState.ShouldNotBeNull();
        stateMachine.CurrentState.GetType().ShouldBe(typeof(State3));
    }
}
