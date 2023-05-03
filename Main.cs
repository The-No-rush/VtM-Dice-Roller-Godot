using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using VtMDiceRoller.Misc_items;

namespace VtMDiceRoller;

public partial class Main : Node2D
{
	private TextEdit rollNumEdit;
	private TextEdit successNumEdit;
	private RichTextLabel errorLabel;
	private List<RichTextLabel> rollsLabels = new();
	private RichTextLabel numOfSuccessesLabel;
	private DeckOfRandomThings deck;
	
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

	private void RollDice()
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
			if (!isDiceParsed)
			{
				ThrowOnScreenError("Dice Num is not a valid number");
				rollNumEdit.Text = "";
			}
			else if (!isSuccessParsed)
			{
				ThrowOnScreenError("Success Num is not a valid number");
				successNumEdit.Text = "";
			}
			else
				ThrowOnScreenError("Unknown FormatException");
			return;

		}
		catch (Exception ignored)
		{
			ThrowOnScreenError("Unknown");
			return;
		}

		if (diceToRoll < 0)
		{
			ThrowOnScreenError("Dice Num is below 0");
			rollNumEdit.Text = "";
			return;
		}
		
		if (successNum < 0)
		{
			ThrowOnScreenError("Success Num is below 0");
			successNumEdit.Text = "";
			return;
		}

		if (successNum > 10)
		{
			ThrowOnScreenError("Success Num is above 10, lowering success to 10");
			successNumEdit.Text = "10";
		}

		if (diceToRoll > 60)
		{
			ThrowOnScreenError("Dice Num is over 60, making dice to Roll 60");
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
			numOfSuccesses += DetermineSuccess(successNum, diceRolls[i]);
		}

		numOfSuccessesLabel.Text = numOfSuccesses.ToString();
	}

	private int DetermineSuccess(int successNum, int roll)
	{
		if (roll == 1)
			return -1;
		
		if (roll >= successNum)
			return 1;
		
		return 0;
	}

	public static async void ThrowOnScreenError(string source, RichTextLabel errorLabel)
	{
		errorLabel.Text = "Error " + source;
		errorLabel.Show();
		await Task.Delay(5000); // Using async, this function specifically
		// can be run in the background to wait for the Task Delay
		errorLabel.Hide();
	}

	//Mainly to keep the calls inside this class cleaner
	private void ThrowOnScreenError(string source)
	{
		ThrowOnScreenError(source, errorLabel);
	}

	private void PickCard()
	{
		numOfSuccessesLabel.Text = "";
		
		deck = (DeckOfRandomThings)randomNumGen.RandiRange(0, 21);
		string pulledCard;
		//Basic solution but if more cards are added look into descriptions or EnumMember Attribute
		if(deck == DeckOfRandomThings.TheFates)
			pulledCard = "The Fates";
		else if (deck == DeckOfRandomThings.TheVoid)
			pulledCard  = "The Void";
		else
			pulledCard = deck.ToString();
		rollsLabels[0].Text = @"[center]" + ((int)deck + 1) + ":" + pulledCard;
		rollsLabels[0].AddThemeFontSizeOverride("normal_font_size", 100);
		
		for (int i = 1; i < 4; i++)
		{
			rollsLabels[i].Text = "";
		}	
	}

	private void SwitchToButtonEditor()
	{
		//Look into packed vs scene file
		GetTree().ChangeSceneToPacked((PackedScene)ResourceLoader.Load("res://ButtonEditor.tscn")); 
	}
}