using Godot;
using System;

public class Credits : Control
{
    private AnimationPlayer animationPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.animationPlayer = this.GetNode<AnimationPlayer>("AnimationPlayer");
        this.animationPlayer.CurrentAnimation = "CreditsAnimation";
        this.animationPlayer.Play();

        this.SetProcessInput(true);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsPressed())
        {
            this.GetTree().ChangeScene("res://src/Scenes/Main.tscn");
        }
    }
}
