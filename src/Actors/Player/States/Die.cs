using Godot;
using Godot.Collections;
using System;

public class Die : Move
{
    public override void Enter(Dictionary<string, object> msg = null)
    {
        base.Enter(msg);

        Player player = this.GetParent().GetParent() as Player;

        player.AnimationPlayer.Play("Fall");
        player.PlayerTrail.Emitting = false;

        base.moveDirection = Vector2.Zero;
    }
}
