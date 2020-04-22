using Godot;
using System;

public class World : Node
{

	private AudioStreamPlayer MusicStreamPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.MusicStreamPlayer = this.GetNode<AudioStreamPlayer>("MusicStreamPlayer");
	}

 // Called every frame. 'delta' is the elapsed time since the previous frame.
 public override void _Process(float delta)
 {
	 if (GameManager.AudioOn) MusicStreamPlayer.StreamPaused = false; else MusicStreamPlayer.StreamPaused = true;
 }
}
