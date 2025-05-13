using Godot;

namespace MetalWarriors.Objects.Characters.Nitro;

// Represents Nitro as a concept, and doesn't worry about the implementation details (scene/script stuff).
public interface INitroCharacter : ICharacter
{
    Vector2 AnimationOffset { get; set; }
    Vector2 GunOffset { get; set; }
}
