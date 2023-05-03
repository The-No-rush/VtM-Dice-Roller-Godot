namespace VtMDiceRoller;

public partial class ButtonEditor : Node2D
{
	private readonly List<Button> selectableButtons = new();

	private Callable actionFunctionCallable;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		actionFunctionCallable = new Callable(this, nameof(SetActionToButton));
		for (int i = 0; i < 4; i++)
		{
			Button button = GetNode<Button>("CustomButton" + (i + 1));
			int iLambda = i;
			//Using iLambda since I can't pass in i directly but I can pass in iLambda
			//as a variable since i is modified outside the loop
			button.Pressed += () => SetActionToButton(iLambda);
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

	public void SetActionToButton(int buttonIndex)
	{
		GD.Print(buttonIndex);
	}
}
