using Godot;
using System;

public class GameManager : Node
{
	[Signal]
	delegate void GameManagerReady();
	[Signal]
	delegate void PlayerDied(string howPlayerDied);
	[Signal]
	delegate void UpdatedScore(int score);
    [Signal]
    delegate void UpdatedGracePeriod(int gracePeriod);

    [Signal]
    delegate void GracePeriodExpired();

	public int Score { get; private set; } = 0;
    public int GracePeriod { get; set; } = 0;
	private Timer everySecond = new Timer();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		EmitSignal(nameof(GameManagerReady)); // just testing the C# signals
		this.AddChild(everySecond);
		everySecond.Connect("timeout", this, nameof(_on_Timer_timeout));
    }

	public void newGame()
	{
		this.Score = 0;
		GetTree().ChangeScene("res://src/Levels/World.tscn");
		GetTree().Paused = false;
        everySecond.Start(1);
	}

	public void endGame()
	{
		GD.Print("player decided to end the current game");
		// TODO: anything else - do they get a high score if they quit?
        this.everySecond.Stop();
		GetTree().ChangeScene("res://src/Scenes/Main.tscn");
	}

	private void _on_Timer_timeout()
	{
		this.Score = this.Score + 10;  // TODO: this is just for demo, need to calculate based on dist traveled not just time
		EmitSignal(nameof(UpdatedScore), this.Score);

        this.GracePeriod = this.GracePeriod - 1; // TODO: GracePeriod elapsing by variable due to game events?
        EmitSignal(nameof(UpdatedGracePeriod), this.GracePeriod);

        if (0 == GracePeriod)
        {
            this.everySecond.Stop();
            //EmitSignal(nameof(PlayerDied), "KILLed");
            EmitSignal(nameof(GracePeriodExpired));
        }
    }
}
