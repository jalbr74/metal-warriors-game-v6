namespace Godot.Utils;

public interface IInputState
{
    bool IsActionPressed(string action, bool exactMatch = false);
    bool IsActionJustPressed(string action, bool exactMatch = false);
    bool IsActionJustReleased(string action, bool exactMatch = false);
    Vector2 GetVector(string negativeX, string positiveX, string negativeY, string positiveY, float deadzone = -1f);
}
