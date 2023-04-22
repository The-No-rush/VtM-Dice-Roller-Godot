using Godot;

namespace VtMDiceRoller.Misc_items;

public class ButtonEditor : Node2D
{
    private void SwitchToDiceRoller()
    {
        GetTree().ChangeSceneToPacked((PackedScene)ResourceLoader.Load("res://Main.tscn"));
    }
}