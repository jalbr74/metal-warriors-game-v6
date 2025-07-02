#nullable enable
using System;
using System.Collections.Generic;

namespace MetalWarriors.Utils;

public class StateMachine
{
    public State? CurrentState { get; private set; }

    // TODO: Stop using types, since it uses reflection and may be slow.
    private readonly Dictionary<Type, State> _states = new ();
    
    public StateMachine(State[] states, Type initialState)
    {
        foreach (var state in states)
        {
            _states[state.GetType()] = state;
        }
        
        TransitionToState(initialState);
    }
    
    public void PhysicsProcess(double delta)
    {
        if (CurrentState == null) return;

        var nextState = CurrentState.ProcessOrPass(delta);

        while (nextState != null)
        {
            CurrentState.Exit();
            
            CurrentState = _states[nextState];
            CurrentState.Enter();

            nextState = CurrentState.ProcessOrPass(delta);
        }
    }
    
    public void SetCurrentState(Type state)
    {
        CurrentState = _states[state];
    }

    // This is mainly used for testing, when you don't have an existing state to transition from
    public void TransitionToState(Type state)
    {
        CurrentState = new AutomaticallyTransitionToAnotherState(state);
    }
}

// TODO: Probably should be an interface, but this is easier for now
public abstract class State
{
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual Type? ProcessOrPass(double delta) { return null; }
}

internal class AutomaticallyTransitionToAnotherState(Type stateToTransitionTo) : State
{
    public override Type ProcessOrPass(double delta)
    {
        return stateToTransitionTo;
    }
}
