namespace Godot;

public class StringName(string value = "")
{
    public override string ToString()
    {
        return value;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        
        return value == obj.ToString();
    }
    
    public override int GetHashCode()
    {
        return value.GetHashCode();
    }

    public static implicit operator StringName(string from) => new (from);
    public static implicit operator string?(StringName? from) => from?.ToString();
}
