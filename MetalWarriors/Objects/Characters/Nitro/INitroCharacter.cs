using Godot;

namespace MetalWarriors.Objects.Characters.Nitro;

public enum NitroDirection { Right, Left }

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
