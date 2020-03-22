using Godot;
using System;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Text;
using Godot.Collections;
using Array = System.Array;


public class TestScores : Godot.Panel
{
	// For testing: GET http://www.mocky.io/v2/5185415ba171ea3a00704eed and receive
	// {"hello": "world" }
	private HTTPRequest _httpRequest;
	private TextEdit _urlTextEdit;
	private string _strURL;

	private void PrintRequestCompleted(int result, int responseCode, Array<string> headers, Array<byte> body)
	{
		GD.Print("result: ", result);
		GD.Print("response_code: ", responseCode);

        GD.Print("Headers count " + headers.Count);
        for (int j = 0; j < headers.Count; ++j)
        {
            GD.Print("header " + j + " " + headers[j]);
        }

        byte[] bytes = new byte[body.Count];
        body.CopyTo(bytes, 0);
        string str = System.Text.Encoding.Default.GetString(bytes);
        GD.Print("str ", str);
        if (str.Empty())
        {
            return;
        }
		JSONParseResult jsonParseResult = JSON.Parse(str);
        GD.Print("jsonParseResult is ", jsonParseResult);
        GD.Print("jsonParseResult.Result is ", jsonParseResult.Result);
        GD.Print("jsonParseResult.Result.GetType() " + jsonParseResult.Result.GetType());
        Godot.Collections.Array arrResults = jsonParseResult.Result as Godot.Collections.Array;
        if (null == arrResults)
        {
            return;
        }
        GD.Print("arrResults count " + arrResults.Count);
        for (int i = 0; i < arrResults.Count; ++i)
        {
            Godot.Collections.Dictionary dict = arrResults[i] as Godot.Collections.Dictionary;
            GD.Print(dict["id"]);
            GD.Print(dict["name"]);
            GD.Print(dict["score"]);
        }
    }

    // Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("_Ready");

		// GET
		this.GetNode("ScoresButton").Connect("pressed", this, nameof(this._OnScoresButtonPressed));
		this.GetNode("ScoresCountButton").Connect("pressed", this, nameof(this._OnScoresCountButtonPressed));
		this.GetNode("TopTenButton").Connect("pressed", this, nameof(this._OnTopTenButtonPressed));

		// POST
		this.GetNode("POSTScoreButton").Connect("pressed", this, nameof(this._OnScoreButtonPressed));

		this._httpRequest = (Godot.HTTPRequest)GetNode("HTTPRequest");
		this._httpRequest.Connect("request_completed", this, nameof(this._OnRequestCompleted));

		this._urlTextEdit = (Godot.TextEdit)this.GetNode("URLTextEdit");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//
//  }

	public void _OnRequestCompleted(int result, int responseCode, Array<string> headers, Array<byte> body)
	{
		GD.Print("_OnRequestCompleted");
		this.PrintRequestCompleted(result, responseCode, headers, body);
	}

	public void _OnScoresButtonPressed()
	{
		GD.Print("_OnScoresButtonPressed");
		//"http://highscores-api-service-mongodb0.apps-crc.testing/scores"
		this._strURL = this._urlTextEdit.Text;
		this._httpRequest.Request(this._strURL + "/scores");
	}

	public void _OnScoresCountButtonPressed()
	{
		GD.Print("_OnScoresCountButtonPressed");
		this._strURL = this._urlTextEdit.Text;
		this._httpRequest.Request(this._strURL + "/scores/count");
	}

	public void _OnTopTenButtonPressed()
	{
		GD.Print("_OnTopTenButtonPressed");
		this._strURL = this._urlTextEdit.Text;
		this._httpRequest.Request(this._strURL + "/scores/topten");
	}

	public void _OnScoreButtonPressed()
	{
		GD.Print("_OnScoreButtonPressed");

		Random r = new Random();
		int playerNumber = r.Next(0, 9);
		int score = r.Next(0, 99999);
		string strJSON = "{\"name\":\"Player" + playerNumber + "\",\"score\":" + score + "}";
		string[] headers = { "Content-Type: application/json" };

		this._strURL = this._urlTextEdit.Text;
		this._httpRequest.Request(this._strURL + "/scores", headers, false, HTTPClient.Method.Post, strJSON);
	}
}
