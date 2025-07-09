using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Nitro;

// Represents Nitro as a concept, and doesn't worry about the implementation details (scene/script stuff).
public interface INitroCharacter
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
    bool IsGunVisible { get; set; }
    Vector2 AnimationOffset { get; set; }
    Vector2 GunOffset { get; set; }

    // Methods
    void PlayAnimation(string animation);
    void PauseAnimation();
}
