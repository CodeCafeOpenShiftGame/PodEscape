using Godot;
using System;
using Godot.Collections;

public class InGameOverlay : Control
{
	private ColorRect pauseOverlay;
    private ColorRect highScoresOverlay;
	private Label scoreLabel;
    private Label gracePeriodLabel;
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

    private bool highScores = false;
    public bool HighScores
    {
        get => highScores;
        set
        {
            highScores = value;
            highScoresOverlay.Visible = value;
        }
    }

    [Export]
    public int gracePeriodTotal = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        this.gameManager = GetNode<GameManager>("/root/GameManager");
        this.gameManager.GracePeriod = this.gracePeriodTotal;

        this.pauseOverlay = GetNode<ColorRect>("PauseOverlay");
        this.highScoresOverlay = GetNode<ColorRect>("HighScoresOverlay");
        this.scoreLabel = GetNode<Label>("Score");
        this.gracePeriodLabel = GetNode<Label>("GracePeriod");
        this.titleLabel = GetNode<Label>("PauseOverlay/Title");

        this.gameManager.Connect("UpdatedScore", this, "_on_ScoreUpdated");
        this.gameManager.Connect("UpdatedGracePeriod", this, "_on_GracePeriodUpdated");
        this.gameManager.Connect("PlayerDied", this, "_on_PlayerDied");
        this.gameManager.Connect("GracePeriodExpired", this, "_on_GracePeriodExpired");
    }

	public override void _UnhandledInput(InputEvent @event)
	{
        if (this.HighScores)
        {
            this.gameManager.GracePeriod = this.gracePeriodTotal;
            this.gameManager.endGame();
            return;
        }

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

    private void _on_GracePeriodUpdated(int gracePeriod)
    {
        String gracePeriodText = ""+gracePeriod;
        this.gracePeriodLabel.Text = "Grace Period: " + gracePeriodText.PadLeft(3,'0');
    }

	private void _on_PlayerDied(string deathString)
	{
		// TODO: anything or just let game manager switch to gameover scene
        // display game over overlay after player has died, i.e. death animation is complete
    }

    private void _on_GracePeriodExpired()
    {
        this.gracePeriodLabel.Text = "EXPIRED";
        this.HighScores = true;
    }
}
