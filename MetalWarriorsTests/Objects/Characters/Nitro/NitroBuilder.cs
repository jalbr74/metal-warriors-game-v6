using Godot;
using Moq;

namespace MetalWarriorsTests.Objects.Characters.Nitro;

public class NitroBuilder
{
    private readonly Mock<global::Nitro> _nitro = new() { CallBase = true };

    public NitroBuilder()
    {
        Input.Reset();
    }

    public (global::Nitro nitro, Mock<global::Nitro> nitroBehavior) Build()
    {
        return (_nitro.Object, _nitro);
    }

    public NitroBuilder WithOnFloor(bool isOnFloor)
    {
        _nitro.Setup(x => x.IsOnFloor()).Returns(isOnFloor);

        return this;
    }

    public NitroBuilder WithGravity(Vector2 gravity)
    {
        _nitro.Setup(x => x.GetGravity()).Returns(gravity);
        
        return this;
    }

    public NitroBuilder WithButtonPressed(string name)
    {
        Input.PressAction(name);

        return this;
    }

    public NitroBuilder WithButtonPressedAndHeld(string name)
    {
        Input.PressAndHoldAction(name);

        return this;
    }

    public NitroBuilder WithVelocity(Vector2 velocity)
    {
        _nitro.Object.Velocity = velocity;
        
        return this;
    }

    public NitroBuilder WithAnimations(AnimatedSprite2D animations)
    {
        _nitro.Object.NitroAnimations = animations;
        
        return this;
    }
}
