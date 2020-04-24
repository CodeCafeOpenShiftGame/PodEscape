using Godot;
using System;

public class GameManager : Node
{

    public enum LevelDifficulty 
    {
        easy,
        medium,
        hard
    }

    [Signal]
    delegate void GameManagerReady();
    [Signal]
    delegate void UpdatedScore(int score);
    [Signal]
    delegate void UpdatedGracePeriod(int gracePeriod);
    [Signal]
    delegate void GracePeriodExpired();

    public static int Score { get; set; } = 0;
    public int GracePeriod { get; set; } = 0;
    private Timer everySecond = new Timer();
    private Timer updateScore = new Timer();

    public static Boolean AudioOn = true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        EmitSignal(nameof(GameManagerReady)); // just testing the C# signals
        this.AddChild(everySecond);
        this.AddChild(updateScore);
        everySecond.Connect("timeout", this, nameof(_on_Timer_timeout));
        updateScore.Connect("timeout", this, nameof(_on_Timer_Update_Score_timeout));
        //this.Connect("PlayerDied", Player, "_on_PlayerDied");
    }

    public void newGame()
    {
        Score = 0;
        GetTree().ChangeScene("res://src/Levels/World.tscn");
        GetTree().Paused = false;
        everySecond.Start(1);
        updateScore.Start(0.1f);
    }

    public void endGame()
    {
        //GD.Print("player decided to end the current game");
        GD.Print("GameManager::endGame()");
        // TODO: anything else - do they get a high score if they quit?
        this.everySecond.Stop();
        //GetTree().ChangeScene("res://src/Scenes/Main.tscn");
    }

    private void _OnItemCollected(Collectable item)
    {
        Score += item.ScorePoints;
        EmitSignal(nameof(UpdatedScore), Score);
    }

    private void _on_Timer_timeout()
    {
        // this.Score = this.Score + 10;  // TODO: this is just for demo, need to calculate based on dist traveled not just time
        // EmitSignal(nameof(UpdatedScore), Score);

        this.GracePeriod = this.GracePeriod - 1; // TODO: GracePeriod elapsing by variable due to game events?
        EmitSignal(nameof(UpdatedGracePeriod), this.GracePeriod);

        if (0 == GracePeriod)
        {
            this.everySecond.Stop();
            EmitSignal(nameof(GracePeriodExpired));
        }
    }

    private void _on_Timer_Update_Score_timeout()
    {
        EmitSignal(nameof(UpdatedScore), Score);
    }

    private void _on_PlayerDied(string howPlayerDied)
    {
        GD.Print("GameManager::_on_PlayerDied by " + howPlayerDied);
        this.endGame();
    }
}
