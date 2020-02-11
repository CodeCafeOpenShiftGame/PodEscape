using Godot;
using System;

public class Player : Actor
{
    protected StateMachine StateMachine;

    public override void _Ready()
    {
        this.StateMachine = (StateMachine)this.GetNode("StateMachine");
    }
}
