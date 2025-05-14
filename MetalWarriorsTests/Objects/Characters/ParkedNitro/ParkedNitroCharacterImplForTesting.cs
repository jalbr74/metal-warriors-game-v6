using Godot;
using MetalWarriors.Objects.Characters;
using MetalWarriors.Objects.Characters.ParkedNitro;
using MetalWarriors.Objects.Characters.ParkedNitro.States;
using MetalWarriors.Utils;

namespace MetalWarriorsTests.Objects.Characters.ParkedNitro;

public class ParkedNitroCharacterImplForTesting : IParkedNitroCharacter
{
    public IConsolePrinter Console { get; set; }
    public CharacterDirection Direction { get; set; }
    public Vector2 Velocity { get; set; }
    
    public StateMachine StateMachine { get; set; }

    public void Initialize(
        bool onFloor
    )
    {
        StateMachine = new StateMachine([
            new ParkedNitroEnteringState(this),
            new ParkedNitroIdleState(this),
            new ParkedNitroExitingState(this),
        ], typeof(ParkedNitroIdleState));
    }
    
    public void _PhysicsProcess(double delta)
    {
        StateMachine.PhysicsProcess(delta);
    }
}
