using Godot;
using System;

public class Player : Actor
{
    private GameManager gameManager;
    public StateMachine StateMachine;
    public AnimationPlayer AnimationPlayer;
    public Vector2 LookDirection = new Vector2(1, 1);
    public CPUParticles2D PlayerTrail;

    public override void _Ready()
    {
        GD.Print("Player::_Ready()");
        this.StateMachine = this.GetNode("StateMachine") as StateMachine;
        this.AnimationPlayer = this.GetNode("AnimationPlayer") as AnimationPlayer;
        this.PlayerTrail = this.GetNode("PlayerTrail") as CPUParticles2D;
        this.gameManager = GetNode<GameManager>("/root/GameManager");

        //this.gameManager.Connect("PlayerDied", this, "_on_PlayerDied");
        this.gameManager.Connect("GracePeriodExpired", this, "_on_GracePeriodExpired");
    }

    /*public virtual void Flip(Vector2 direction)
    {
        if (direction.x != 0 && this.LookDirection != direction) {
            Position2D bodyPivot = this.GetNode("BodyPivot") as Position2D;

            this.LookDirection = direction;

            bodyPivot.Scale = new Vector2(
                direction.x,
                1
            );
        }
    }*/
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
