using Godot;

namespace MetalWarriors.Utils;

public class CollisionUtils
{
    public static void AddCollisionLayer(CharacterBody2D characterBody2D, int layerNumber)
    {
        if (layerNumber is < 1 or > 32) // Godot supports up to 32 layers
        {
            GD.PrintErr($"Invalid layer number: {layerNumber}. Layer number must be between 1 and 32.");
            return;
        }

        // Calculate the bitmask for the layer to add
        // Remember that layers are 0-indexed in bitwise operations (0 for layer 1, 1 for layer 2, etc.)
        var layerBit = 1u << (layerNumber - 1); // Use 'u' for unsigned integer literal

        // Use bitwise OR assignment to add the layer.
        // This sets the bit corresponding to layerBit to 1, without affecting other bits.
        characterBody2D.CollisionLayer |= layerBit;
    }

    public static void RemoveCollisionLayer(CharacterBody2D characterBody2D, int layerNumber)
    {
        if (layerNumber is < 1 or > 32) // Godot supports up to 32 layers
        {
            GD.PrintErr($"Invalid layer number: {layerNumber}. Layer number must be between 1 and 32.");
            return;
        }

        // Calculate the bitmask for the layer to remove
        // Remember that layers are 0-indexed in bitwise operations (0 for layer 1, 1 for layer 2, etc.)
        var layerBit = 1u << (layerNumber - 1); // Use 'u' for unsigned integer literal

        // Use bitwise AND with the bitwise NOT of the layer to remove
        // This effectively "turns off" that specific bit while leaving others unchanged.
        characterBody2D.CollisionLayer &= ~layerBit;
    }
}
