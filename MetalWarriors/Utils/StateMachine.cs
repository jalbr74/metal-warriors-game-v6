using System.Collections.Generic;
using Godot;

namespace MetalWarriors.Utils;

public class StateMachine
{
    private State _currentState;
    
    private readonly Dictionary<string, State> _states;
    private IConsolePrinter _console;
    
    public StateMachine(Dictionary<string, State> states, string initialState, IConsolePrinter console)
    {
        _states = states;
        _console = console;

        _currentState = _states[initialState];
    }

    public void PhysicsProcess(double delta)
    {
        if (_currentState == null) return;

        if (_currentState.ShouldTransitionToAnotherState(out var otherState))
        {
            _currentState.Exit(delta);
            
            _currentState = _states[otherState];
            _currentState.Enter(delta);
        }
        
        _currentState.PhysicsProcess(delta);
    }
    
    public void SetCurrentState(string state)
    {
        _currentState = _states[state];
    }
}

public abstract class State
{
    public virtual void Enter(double delta) { }
    public virtual void Exit(double delta) { }
    public abstract void PhysicsProcess(double delta);
    public abstract bool ShouldTransitionToAnotherState(out string otherState);
}
