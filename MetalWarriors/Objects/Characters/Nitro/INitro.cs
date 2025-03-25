using Godot;

namespace MetalWarriors.Objects.Characters.Nitro;

// Represents Nitro as a concept, and doesn't worry about the implementation details.
public interface INitro
{
    Vector2 Velocity { get; set; }
    NitroDirection Direction { get; set; }
    bool IsOnFloor();
    Vector2 GetGravity();
    void PlayAnimation(string animation);
    string CurrentAnimation { get; }
}
