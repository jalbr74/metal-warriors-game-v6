namespace Godot;

public class Input
{
    private static readonly List<string> PressedActions = [];
    private static readonly List<string> HeldActions = [];
    
    public static void PressAction(string action)
    {
        PressedActions.Add(action);
    }

    public static void PressAndHoldAction(string action)
    {
        PressedActions.Add(action);
        HeldActions.Add(action);
    }

    public static Vector2 GetVector(string negativeX, string positiveX, string negativeY, string positiveY, float deadzone = -1f)
    {
        var x = 0;
        var y = 0;
        
        if (HeldActions.Contains(negativeX))
        {
            x = -1;
        }
        else if (HeldActions.Contains(positiveX))
        {
            x = 1;
        }
        
        if (HeldActions.Contains(negativeY))
        {
            y = -1;
        }
        else if (HeldActions.Contains(positiveY))
        {
            y = 1;
        }

        return new Vector2(x, y);
    }
    
    public static bool IsActionJustPressed(string action, bool exactMatch = false)
    {
        return PressedActions.Contains(action);
    }

    public static void Reset()
    {
        PressedActions.Clear();
        HeldActions.Clear();
    }
}
