using System.Collections.Generic;
using Godot;

namespace MetalWarriors.Utils;

public class StateMachine
{
    private readonly Dictionary<string, State> _states;
    private State _currentState;
    private IConsolePrinter _console;
    
    public StateMachine(Dictionary<string, State> states, string initialState, IConsolePrinter console)
    {
        _states = states;
        _console = console;

        foreach (var state in states.Values)
        {
            state.StateMachine = this;
        }
        
        _currentState = _states[initialState];
    }

    public void TransitionTo(string newState, double delta)
    {
        _currentState?.Exit(delta);

        if (_states.TryGetValue(newState, out _currentState))
        {
            _console.Print("Transitioning to state: " + newState);

            _currentState.Enter(delta);
            _currentState.HandleState(delta);
        }
        else
        {
            throw new KeyNotFoundException("State not found: " + newState);
        }
    }

    public void PhysicsProcess(double delta)
    {
        _currentState?.HandleState(delta);
    }
}

public class State
{
    public StateMachine StateMachine { get; set; }
    
    public virtual void Enter(double delta) { }
    public virtual void Exit(double delta) { }
    public virtual void HandleState(double delta) { }
}
