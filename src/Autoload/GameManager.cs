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
	
	public int Score { get; private set; } = 0;
	private Timer everySecond = new Timer();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		EmitSignal(nameof(GameManagerReady)); // just testing the C# signals
		this.AddChild(everySecond);
		everySecond.Connect("timeout", this, nameof(_on_Timer_timeout));
		everySecond.Start(1);
	}
	
	public void newGame()
	{
		this.Score = 0;
		GetTree().ChangeScene("res://src/Levels/World.tscn");
		GetTree().Paused = false;
	}
	
	public void endGame()
	{
		GD.Print("player decided to end the current game")	;
		// TODO: anything else - do they get a high score if they quit?
		GetTree().ChangeScene("res://src/Scenes/Main.tscn");
	}
	
	private void _on_Timer_timeout()
	{
		this.Score = this.Score + 10;  // TODO: this is just for demo, need to calculate based on dist traveled not just time
		EmitSignal(nameof(UpdatedScore), this.Score);
	}
}
