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

        foreach (var state in states.Values)
        {
            state.StateMachine = this;
        }
        
        _currentState = _states[initialState];
    }

    public void PhysicsProcess(double delta)
    {
        var nextStateKey = _currentState?.HandleState(delta);
        if (nextStateKey == null) return;
        
        if (!_states.TryGetValue(nextStateKey, out var nextState)) return;
        if (nextState == _currentState) return;
        
        _console.Print("Transitioning to state: " + nextStateKey);
                    
        _currentState.Exit(delta);

        nextState.Enter(delta);
        nextState.HandleState(delta);

        _currentState = nextState;
    }
    
    public void SetCurrentState(string state)
    {
        _currentState = _states[state];
    }
}

public abstract class State
{
    public StateMachine StateMachine { get; set; }

    public virtual void Enter(double delta) { }
    public virtual void Exit(double delta) { }
    public abstract string HandleState(double delta);
}
