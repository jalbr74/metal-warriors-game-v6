using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters;

public enum CharacterDirection { FacingRight, FacingLeft }

public interface ICharacter
{
    ISnesController Controller { get; set; }
    IConsolePrinter Console { get; set; }
    string CurrentAnimation { get; }
    int CurrentAnimationFrame { get; }
    bool IsAnimationFinished { get; set; }
    CharacterDirection Direction { get; set; }
    Vector2 Velocity { get; set; }
    bool OnFloor { get; }
    void PlayAnimation(string animation);
    void PauseAnimation();
}
