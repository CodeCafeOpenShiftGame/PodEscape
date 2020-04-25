using Godot;
using System;

public class Collectable : Node2D
{
    [Signal]
    delegate void ItemCollected(Collectable item);

    [Export]
    public int ScorePoints = 10;

    protected AnimationPlayer animationPlayer;
    private GameManager gameManager;

    public override void _Ready()
    {
        this.gameManager = GetNode<GameManager>("/root/GameManager");
        this.animationPlayer = this.GetNode<AnimationPlayer>("Area2D/AnimationPlayer");

        this.Connect(nameof(ItemCollected), this.gameManager, "_OnItemCollected");
    }

    public void _OnBodyEntered(PhysicsBody2D body)
    {
        EmitSignal(nameof(ItemCollected), this);
        this.animationPlayer.Play("fadeOut");
        if (GameManager.AudioOn)
        {
            AudioStreamPlayer audio = this.GetNode<AudioStreamPlayer>("AudioStreamPlayer");
            audio.Play();
        }
    }
}