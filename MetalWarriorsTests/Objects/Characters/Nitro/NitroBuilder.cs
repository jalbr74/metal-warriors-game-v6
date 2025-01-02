using Godot;
using Godot.Utils;
using Moq;

namespace MetalWarriorsTests.Objects.Characters.Nitro;

public class NitroBuilder
{
    private readonly Mock<global::Nitro> _nitro = new() { CallBase = true };
    private readonly Mock<IInputState> _inputState = new();

    public NitroBuilder()
    {
        Input.InputState = _inputState.Object;
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

    public NitroBuilder WithActionPressed(string name)
    {
        _inputState.Setup(x => x.IsActionPressed(name, false)).Returns(true);

        return this;
    }

    public NitroBuilder WithActionJustPressed(string name)
    {
        _inputState.Setup(x => x.IsActionJustPressed(name, false)).Returns(true);
        
        return this;
    }

    public NitroBuilder WithActionJustReleased(string name)
    {
        _inputState.Setup(x => x.IsActionJustReleased(name, false)).Returns(true);
        
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
