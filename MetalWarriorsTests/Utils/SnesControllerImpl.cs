using MetalWarriors.Utils;

namespace MetalWarriorsTests.Utils;

public class SnesControllerImpl(
    bool isDPadLeftPressed = false,
    bool isDPadRightPressed = false,
    bool isButtonBPressed = false
) : ISnesController
{
    public bool IsDPadLeftPressed => isDPadLeftPressed;
    public bool IsDPadRightPressed => isDPadRightPressed;
    public bool IsButtonBPressed => isButtonBPressed;
}
