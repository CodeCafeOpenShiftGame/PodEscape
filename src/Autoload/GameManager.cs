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
	private void _on_Timer_timeout()
	{
		this.Score = this.Score + 10;  // TODO: this is just for demo, need to calculate based on dist traveled not just time
		EmitSignal(nameof(UpdatedScore), this.Score);
	}
}
