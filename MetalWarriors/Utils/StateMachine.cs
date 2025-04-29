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
        
        _currentState = _states[initialState];
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
}

public abstract class State
{
    public virtual void Enter() { }
    public virtual void Exit() { }
    public abstract void PhysicsProcess(double delta);
    public abstract bool ShouldTransitionToAnotherState(out Type otherState);
}
