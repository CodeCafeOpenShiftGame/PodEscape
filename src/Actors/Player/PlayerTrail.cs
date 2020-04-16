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
        if (player.IsOnFloor())
        {
            this.Emitting = true;
            // Note: we also turn off the trail in some state changes
        }
        else 
        {
            this.Emitting = false;
        }

        // you can change gravity to pull the trail in different directions
        // this.Gravity = new Vector2(0, 0);
  }
}
