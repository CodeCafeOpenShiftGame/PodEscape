using Godot;
using System;

public class Player : Actor
{
    private GameManager gameManager;

    public StateMachine StateMachine;

    public AnimationPlayer AnimationPlayer;

    public Vector2 LookDirection = new Vector2(1, 1);

    public override void _Ready()
    {
        GD.Print("Player::_Ready()");
        this.StateMachine = this.GetNode("StateMachine") as StateMachine;
        this.AnimationPlayer = this.GetNode("AnimationPlayer") as AnimationPlayer;

        this.gameManager = GetNode<GameManager>("/root/GameManager");

        //this.gameManager.Connect("PlayerDied", this, "_on_PlayerDied");
        this.gameManager.Connect("GracePeriodExpired", this, "_on_GracePeriodExpired");
    }

    //    private void _on_PlayerDied(string deathString)
    //    {
    //        // TODO: anything or just let game manager switch to gameover scene
    //        // display game over overlay after player has died, i.e. death animation is complete
    //    }

    private void _on_GracePeriodExpired()
    {
        GD.Print("Player::_on_GracePeriodExpired()");
        this.StateMachine.TransitionTo("Die");
        this.AnimationPlayer.Play("Fall");
    }
}
