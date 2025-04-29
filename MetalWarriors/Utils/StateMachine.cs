using System.Collections.Generic;

namespace MetalWarriors.Utils;

public class StateMachine(Dictionary<string, State> states, string initialState)
{
    private State _currentState = states[initialState];
    
    public void PhysicsProcess(double delta)
    {
        if (_currentState == null) return;

        if (_currentState.ShouldTransitionToAnotherState(out var otherState))
        {
            _currentState.Exit();
            
            _currentState = states[otherState];
            _currentState.Enter();
        }
        
        _currentState.PhysicsProcess(delta);
    }
    
    public void SetCurrentState(string state)
    {
        _currentState = states[state];
    }
}

public abstract class State
{
    public virtual void Enter() { }
    public virtual void Exit() { }
    public abstract void PhysicsProcess(double delta);
    public abstract bool ShouldTransitionToAnotherState(out string otherState);
}
