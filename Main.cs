using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

namespace VtMDiceRoller;

public partial class Main : Node2D
{
	private TextEdit rollNumEdit;
	private TextEdit successNumEdit;
	private RichTextLabel errorLabel;
	private List<RichTextLabel> rollsLabels = new();
	private RichTextLabel numOfSuccessesLabel;
	

	//Change name
	RandomNumberGenerator randomNumGen = new();
	public override void _Ready()
	{
		rollNumEdit = GetNode<TextEdit>("RollNumEdit");
		successNumEdit = GetNode<TextEdit>("SuccessNumEdit");
		errorLabel = GetNode<RichTextLabel>("ErrorLabel");
		errorLabel.Hide();
		for (int i = 0; i < 4; i++)
		{
			rollsLabels.Add(GetNode<RichTextLabel>("DiceRollLabels/Rolls" + (i + 1) + "Label"));
			rollsLabels[i].Text = "";
		}
		numOfSuccessesLabel = GetNode<RichTextLabel>("NumOfSuccessesLabel");
		numOfSuccessesLabel.Text = "";
	}

	public void rollDice()
	{
		int diceToRoll;
		bool isDiceParsed = false;
		int successNum;
		bool isSuccessParsed = false;
		int rollsPerLabel = 10;
		
		try
		{
			diceToRoll = Convert.ToInt32(rollNumEdit.Text);
			isDiceParsed = true;
			successNum = Convert.ToInt32(successNumEdit.Text);
			isSuccessParsed = true;
		}
		catch (FormatException ignored)
		{
			if(!isDiceParsed)
				throwOnScreenError("Dice Num is not a valid number");
			else if (!isSuccessParsed)
				throwOnScreenError("Success Num is not a valid number");
			else
				throwOnScreenError("Unknown FormatException");
			return;

		}
		catch (Exception ignored)
		{
			throwOnScreenError("Unknown");
			return;
		}

		if (diceToRoll < 0)
		{
			throwOnScreenError("Dice Num is below 0");
			rollNumEdit.Text = "";
			return;
		}
		
		if (successNum < 0)
		{
			throwOnScreenError("Success Num is below 0");
			successNumEdit.Text = "";
			return;
		}

		if (diceToRoll > 60)
		{
			throwOnScreenError("Dice Num is over 60, making dice to Roll 60");
			rollNumEdit.Text = "60";
			diceToRoll = 60;
		}

		List<int> diceRolls = new List<int>();
		for (int i = 0; i < diceToRoll; i++)
		{
			diceRolls.Add(randomNumGen.RandiRange(1, 10));
		}
		
		string spacing = " ";
		
		if (diceToRoll > 40)
		{
			rollsPerLabel = 15;
			for (int i = 0; i < 4; i++)
			{
				rollsLabels[i].Text = @"[center]";
				rollsLabels[i].AddThemeFontSizeOverride("normal_font_size", 80);
			}
		}
		else
		{
			for (int i = 0; i < 4; i++)
			{
				rollsLabels[i].Text = @"[center]";
				rollsLabels[i].AddThemeFontSizeOverride("normal_font_size", 100);
			}	
		}

		for (int i = 0; i < diceRolls.Count; i++)
		{
			switch (i /rollsPerLabel)
			{
				case 0:
					rollsLabels[0].Text += diceRolls[i];
					if (i % rollsPerLabel != rollsPerLabel - 1)
						rollsLabels[0].Text += spacing;
					break;
				case 1:
					rollsLabels[1].Text += diceRolls[i];
					if (i % rollsPerLabel != rollsPerLabel - 1)
						rollsLabels[1].Text += spacing;
					break;
				case 2:
					rollsLabels[2].Text += diceRolls[i];
					if (i % rollsPerLabel != rollsPerLabel - 1)
						rollsLabels[2].Text += spacing;
					break;
				case 3:
					rollsLabels[3].Text += diceRolls[i];
					if (i % rollsPerLabel != rollsPerLabel - 1)
						rollsLabels[3].Text += spacing;
					break;
			}
		}

		int numOfSuccesses = 0;
		for (int i = 0; i < diceRolls.Count; i++)
		{
			numOfSuccesses += determineSuccess(successNum, diceRolls[i]);
		}

		numOfSuccessesLabel.Text = numOfSuccesses.ToString();
	}

	private int determineSuccess(int successNum, int roll)
	{
		if (roll == 1)
			return -1;
		
		if (roll >= successNum)
			return 1;
		
		return 0;
	}

	private async void throwOnScreenError(string source)
	{
		errorLabel.Text = "Error " + source;
		errorLabel.Show();
		await Task.Delay(5000); // Using async, this function specifically
		// can be run in the background to wait for the Task Delay
		errorLabel.Hide();
	}
}