using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Pilot;

public interface IPilotCharacter
{
    // Properties
    IConsolePrinter Console { get; set; }
    CharacterDirection Direction { get; set; }
    Vector2 Velocity { get; set; }
    ISnesController Controller { get; set; }
    string CurrentAnimation { get; }
    int CurrentAnimationFrame { get; }
    bool IsAnimationFinished { get; set; }
    bool OnFloor { get; }
    
    // Methods
    void PlayAnimation(string animation);
    void PauseAnimation();
}
