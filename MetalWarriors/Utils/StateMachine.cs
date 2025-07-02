#nullable enable
using System;
using System.Collections.Generic;

namespace MetalWarriors.Utils;

public interface IState
{
    void Enter();
    void Exit();
    Type? ProcessOrPass(double delta);
}


public class StateMachine
{
    public IState? CurrentState { get; private set; }

    // TODO: Stop using types, since it uses reflection and may be slow.
    private readonly Dictionary<Type, IState> _states = new ();
    
    public StateMachine(IState[] states, Type initialState)
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

internal class AutomaticallyTransitionToAnotherState(Type stateToTransitionTo) : IState
{
    public void Enter() { }
    public void Exit() { }

    public Type ProcessOrPass(double delta)
    {
        return stateToTransitionTo;
    }
}
