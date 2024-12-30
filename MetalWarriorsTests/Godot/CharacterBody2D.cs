namespace Godot;

public class CharacterBody2D : PhysicsBody2D
{
    public virtual Vector2 Velocity { get; set; } = Vector2.Zero;
    
    public virtual void MoveAndSlide()
    {
    }
    
    public virtual bool IsOnFloor()
    {
        return false;
    }
}
