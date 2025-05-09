namespace MetalWarriors.Utils;

public interface ISnesController
{
    bool IsDPadLeftPressed { get; }
    bool IsDPadRightPressed { get; }
    bool IsButtonBPressed { get; }
    bool IsSelectPressed { get; }
}
