using Godot;
using MetalWarriors.Objects.Characters.Nitro.States;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro;

public enum NitroDirection { Right, Left }
public enum NitroState { Idle, Walking, Launching, Falling, Flying }

// Represents Nitro as a concept, and doesn't worry about the implementation details (scene/script stuff).
public interface INitroCharacter
{
    Vector2 Velocity { get; set; }
    NitroDirection Direction { get; set; }
    string CurrentAnimation { get; }
    bool OnFloor { get; }
    bool IsLaunchingAnimationComplete { get; set; }
    
    void PlayAnimation(string animation);
    void PauseAnimation();
}

// This class operates on the INitro interface so that it can be used with any implementation of INitro (useful for doing TDD).
public class NitroCharacterHandler(ISnesController controller, INitroCharacter nitro, IConsolePrinter console, string initialState)
{
    private readonly StateMachine _stateMachine = new StateMachine(new System.Collections.Generic.Dictionary<string, State>
    {
        { "idle", new NitroIdleState(controller, nitro, console) },
        {"walking", new NitroWalkingState(controller, nitro, console)},
        {"launching", new NitroLaunchingState(controller, nitro, console)},
        {"falling", new NitroFallingState(controller, nitro, console)},
        {"flying", new NitroFlyingState(controller, nitro, console)},
    }, initialState, console);
    
    public void PhysicsProcess(double delta)
    {
        _stateMachine.PhysicsProcess(delta);
    }
}
