namespace Godot;

public class Node2D : CanvasItem
{
    public virtual Vector2 Scale { get; set; } = Vector2.One;
    
    public virtual void _PhysicsProcess(double delta)
    {
    }
}
