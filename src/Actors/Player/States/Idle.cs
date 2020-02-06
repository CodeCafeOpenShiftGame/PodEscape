using Godot;
using System;

public class Idle : State
{
    public override String Handle(Player actor, float delta)
    {
        if (actor.GetDirection() != Vector2.Zero) {
            return "Run";
        }

        return null;
    }

}
