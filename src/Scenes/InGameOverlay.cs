using Godot;
using System;
using Godot.Collections;

public class InGameOverlay : Control
{
	private ColorRect pauseOverlay;
	private Label scoreLabel;
	private Label titleLabel;
	private GameManager gameManager;
	
	private bool paused = false;
	public bool Paused
	{
		get => paused;
		set 
		{
			paused = value;
			GetTree().Paused = value;
			pauseOverlay.Visible = value;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pauseOverlay = GetNode<ColorRect>("PauseOverlay");
		scoreLabel = GetNode<Label>("Score");
		titleLabel = GetNode<Label>("PauseOverlay/Title");
		gameManager = GetNode<GameManager>("/root/GameManager");
		
		gameManager.Connect("UpdatedScore", this, "_on_ScoreUpdated");
		gameManager.Connect("PlayerDied", this, "_on_PlayerDied");
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.IsActionPressed("pause")) // TODO: and player state != dead
		{
			this.Paused = !this.Paused;
		}
	}
	
	private void _on_ContinueButton_button_up()
	{
		this.Paused = false;
	}

	private void _on_QuitButton_button_up()
	{
		gameManager.endGame();
	}
	
	private void _on_ScoreUpdated(int score)
	{
		String scoreText = ""+score;
		this.scoreLabel.Text = "Score: " + scoreText.PadLeft(6,'0');
	}
	
	private void _on_PlayerDied(string deathString)
	{
		// TODO: anything or just let game manager switch to gameover scene
	}
}

