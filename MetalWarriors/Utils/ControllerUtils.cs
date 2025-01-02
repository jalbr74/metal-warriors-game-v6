using Godot;
using Godot.Collections;

namespace MetalWarriors.Utils;

public static class ControllerUtils
{
    public static void RemapControllerForAndroid()
    {
        ReplaceControllerAction("Button_B", JoyButton.A);
        ReplaceControllerAction("Button_A", JoyButton.B);
    }
    
    private static void ReplaceControllerAction(string action, JoyButton joyButton)
    {
        InputMap.ActionEraseEvents(action);
        
        InputMap.ActionAddEvent(action, new InputEventJoypadButton
        {
            ButtonIndex = joyButton
        });
    }
}
