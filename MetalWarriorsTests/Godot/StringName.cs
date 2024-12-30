namespace Godot;

public class StringName(string value = "")
{
    public override string ToString()
    {
        return value;
    }
    
    public static implicit operator StringName(string from) => new (from);
    public static implicit operator string?(StringName? from) => from?.ToString();
}
