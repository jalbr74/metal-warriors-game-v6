using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro;

public enum NitroDirection { FacingRight, FacingLeft }

// Represents Nitro as a concept, and doesn't worry about the implementation details (scene/script stuff).
public interface INitroCharacter
{
    ISnesController Controller { get; set; }
    IConsolePrinter Console { get; set; }
    Vector2 Velocity { get; set; }
    NitroDirection Direction { get; set; }
    string CurrentAnimation { get; }
    int CurrentAnimationFrame { get; }
    bool IsAnimationFinished { get; set; }
    Vector2 GunPosition { get; set; }
    bool OnFloor { get; }
    
    void PlayAnimation(string animation);
    void PauseAnimation();
}
