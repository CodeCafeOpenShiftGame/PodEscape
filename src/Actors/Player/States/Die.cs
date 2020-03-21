using Godot;
using Godot.Collections;
using System;

public class Die : Move
{
    public override void Enter(Dictionary<string, object> msg = null)
    {
        base.Enter(msg);

        base.moveDirection = Vector2.Zero;
    }
}
