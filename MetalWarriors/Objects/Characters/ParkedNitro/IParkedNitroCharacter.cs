using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.ParkedNitro;

public interface IParkedNitroCharacter
{
    IConsolePrinter Console { get; set; }
    CharacterDirection Direction { get; set; }
    Vector2 Velocity { get; set; }
    string CurrentAnimation { get; }
    void PlayAnimation(string animation);
}
