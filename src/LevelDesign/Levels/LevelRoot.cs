using Godot;
using System;

public class LevelRoot : Node2D
{
    
    public enum Difficulty 
    {
        easy,
        medium,
        hard
    }

    [Export]
    public Difficulty difficulty;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
                
    }

}
