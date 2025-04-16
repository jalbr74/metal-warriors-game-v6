using System.Collections.Generic;
using Godot;

namespace MetalWarriors.Utils;

public class StateMachine(Dictionary<string, State> states, string initialState)
{
    private State _currentState = states[initialState];

    public void TransitionTo(string newState)
    {
        _currentState?.Exit();

        if (states.TryGetValue(newState, out _currentState))
        {
            GD.Print("Transitioning to state: " + newState);
            _currentState.Enter();    
        }
        else
        {
            GD.PrintErr("State not found: " + newState);
        }
    }

    public void AddState(string name, State state)
    {
        state.StateMachine = this;
        states.Add(name, state);
    }

    public void PhysicsProcess(double delta)
    {
        var nextState = _currentState?.HandleState(delta);
        if (nextState != null)
        {
            TransitionTo(nextState);
        }
    }
}

public class State
{
    public StateMachine StateMachine { get; set; }
    
    public virtual void Enter() { }
    public virtual void Exit() { }

    public virtual string HandleState(double delta)
    {
        return null;
    }
}
