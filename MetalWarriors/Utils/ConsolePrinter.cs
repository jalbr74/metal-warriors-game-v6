using Godot;

namespace MetalWarriors.Utils;

public class ConsolePrinter : IConsolePrinter
{
    public void Print(string message) => GD.Print(message);
}
