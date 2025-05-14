using Godot;
using MetalWarriors.Utils;

namespace MetalWarriors.Objects.Characters.Pilot;

public interface IPilotCharacter : ICharacter
{
    ISnesController Controller { get; set; }
    string CurrentAnimation { get; }
    int CurrentAnimationFrame { get; }
    bool IsAnimationFinished { get; set; }
    bool OnFloor { get; }
    void PlayAnimation(string animation);
    void PauseAnimation();
}
