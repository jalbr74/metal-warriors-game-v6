using System;
using System.Collections.Generic;

namespace MetalWarriors.Utils;

public class StateMachine
{
    private readonly Dictionary<Type, State> _states = new ();
    private State _currentState;
    
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
        if (_currentState == null) return;

        if (_currentState.ShouldTransitionToAnotherState(out var otherState))
        {
            _currentState.Exit();
            
            _currentState = _states[otherState];
            _currentState.Enter();
        }
        
        _currentState.PhysicsProcess(delta);
    }
    
    public void SetCurrentState(Type state)
    {
        _currentState = _states[state];
    }

    // This is mainly used for testing, when you don't have an existing state to transition from
    public void TransitionToState(Type state)
    {
        _currentState = new AutomaticallyTransitionToAnotherState(state);
    }
}

public abstract class State
{
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void PhysicsProcess(double delta) { }

    public virtual bool ShouldTransitionToAnotherState(out Type otherState)
    {
        otherState = null;
        return false;
    }
}

internal class AutomaticallyTransitionToAnotherState(Type stateToTransitionTo) : State
{
    public override bool ShouldTransitionToAnotherState(out Type otherState)
    {
        otherState = stateToTransitionTo;
        return true;
    }
}
