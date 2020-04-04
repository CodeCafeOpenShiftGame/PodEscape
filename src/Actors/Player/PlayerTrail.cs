using Godot;
using System;

public class PlayerTrail : CPUParticles2D
{   
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("PlayerTrail::_Ready()");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Player player = (Player)this.Owner;
        // Vector2 velocity = new Vector2(1,0); // get from Move state
        if (player.IsOnFloor()) // && velocity.x > 0.1) 
        {
            this.Emitting = true;
        }
        else 
        {
            this.Emitting = false;
        }

        // you can change gravity to pull the trail in different directions
        this.Gravity = new Vector2(0, 0);
  }
}
