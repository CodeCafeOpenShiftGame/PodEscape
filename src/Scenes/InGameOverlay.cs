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

    private Label[] arrScoreLabels;

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

    [Export]
    public string highScoresAPIURL = "";

    private HTTPRequest _httpRequest;
    private string _strURL;


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

        this.arrScoreLabels = new Label[10];
        for (int i = 0; i < 10; ++i)
        {
            this.arrScoreLabels[i] = GetNode<Label>("HighScoresOverlay/VBoxContainer/ScoreLabel" + i);
        }

        this.gameManager.Connect("UpdatedScore", this, "_on_ScoreUpdated");
        this.gameManager.Connect("UpdatedGracePeriod", this, "_on_GracePeriodUpdated");
        this.gameManager.Connect("PlayerDied", this, "_on_PlayerDied");
        this.gameManager.Connect("GracePeriodExpired", this, "_on_GracePeriodExpired");

        this._httpRequest = (Godot.HTTPRequest)GetNode("HighScoresOverlay/HTTPRequest");
        this._httpRequest.Connect("request_completed", this, nameof(this._OnRequestCompleted));
        if (!this.highScoresAPIURL.Empty())
        {
            this._strURL = this.highScoresAPIURL;
        }
        else
        {
            this._strURL = "127.0.0.1";//"http://highscores-api-service-mongodb0.apps-crc.testing";
        }
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

    private void _on_AudioButton_button_up()
    {
        //AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), 0);

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
        this.gracePeriodLabel.Text = deathString;
        Timer waitForDeathAnimation = new Timer();
        waitForDeathAnimation.OneShot = true;
        waitForDeathAnimation.WaitTime = 2;
        waitForDeathAnimation.Connect("timeout", this, nameof(_on_DeathAnimationComplete));
        waitForDeathAnimation.Autostart = true;
    }

    private void _on_GracePeriodExpired()
    {
        this.gracePeriodLabel.Text = "EXPIRED";
        Timer waitForDeathAnimation = new Timer();
        waitForDeathAnimation.OneShot = true;
        waitForDeathAnimation.WaitTime = 2;
        waitForDeathAnimation.Connect("timeout", this, nameof(_on_DeathAnimationComplete));
        waitForDeathAnimation.Autostart = true;
    }

    private void _on_DeathAnimationComplete()
    {
        this._httpRequest.Request(this._strURL + "/scores/topten");
        this.HighScores = true;
    }
    public void _OnRequestCompleted(int result, int responseCode, Array<string> headers, Array<byte> body)
    {
        GD.Print("_OnRequestCompleted");

        do
        {
            if (0 != result)
            {
                GD.Print("FAIL: result was " + result);
                break;
            }

            if (200 != responseCode)
            {
                GD.Print("FAIL: responseCode was " + responseCode);
                break;
            }

            byte[] bytes = new byte[body.Count];
            body.CopyTo(bytes, 0);
            string str = System.Text.Encoding.Default.GetString(bytes);
            JSONParseResult jsonParseResult = JSON.Parse(str);
            Godot.Collections.Array arrResults = jsonParseResult.Result as Godot.Collections.Array;
            Godot.Collections.Dictionary dict;
            for (int i = 0; i < arrResults.Count; ++i)
            {
                dict = arrResults[i] as Godot.Collections.Dictionary;
                this.arrScoreLabels[i].Text = dict["name"] + " " + dict["score"];
            }
        } while (false);
    }
}
