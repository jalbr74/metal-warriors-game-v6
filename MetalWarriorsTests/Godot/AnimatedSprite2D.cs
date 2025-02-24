namespace Godot;

public class AnimatedSprite2D : Node2D
{
    private StringName _animation;

    public StringName Animation => _animation;
    
    public virtual void Play(StringName name = null, float customSpeed = 1f, bool fromEnd = false)
    {
        _animation = name;
    }
}
