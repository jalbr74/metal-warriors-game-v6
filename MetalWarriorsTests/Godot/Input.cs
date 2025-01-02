using Godot.Utils;

namespace Godot;

public class Input
{
    public static IInputState InputState { get; set; }
    
    public static Vector2 GetVector(string negativeX, string positiveX, string negativeY, string positiveY, float deadzone = -1f)
    {
        return InputState.GetVector(negativeX, positiveX, negativeY, positiveY, deadzone);
    }
    
    public static bool IsActionPressed(string action, bool exactMatch = false)
    {
        return InputState.IsActionPressed(action, exactMatch);
    }

    public static bool IsActionJustPressed(string action, bool exactMatch = false)
    {
        return InputState.IsActionJustPressed(action, exactMatch);
    }

    public static bool IsActionJustReleased(string action, bool exactMatch = false)
    {
        return InputState.IsActionJustReleased(action, exactMatch);
    }
}
