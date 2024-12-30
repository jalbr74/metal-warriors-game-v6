namespace Godot;

public class PhysicsBody2D : CollisionObject2D
{
    public virtual Vector2 GetGravity()
    {
        return new Vector2(0, 0);
    }
}
