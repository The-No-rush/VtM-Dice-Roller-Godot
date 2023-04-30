using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class ButtonEditor : Node2D
{
	private List<Button> selectableButtons = new();

	private Callable actionFunctionCallable;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		actionFunctionCallable = new Callable(this, nameof(SetActionToButton));
		for (int i = 0; i < 4; i++)
		{
			Button button = GetNode<Button>("CustomButton" + (i + 1));
			button.Pressed += () => SetActionToButton(button.Text.Substring(button.Text.Length -1)); // Can't use i
			//with lambda so for temporary time using the texts substring
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void SwitchToDiceRoller()
	{
		GetTree().ChangeSceneToPacked((PackedScene)ResourceLoader.Load("res://Main.tscn"));
	}

	public void SetActionToButton(Variant buttonIndex)
	{
		GD.Print(buttonIndex);
	}
}
