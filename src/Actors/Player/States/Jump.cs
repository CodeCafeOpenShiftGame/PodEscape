using Godot;
using System;

public class Jump : State
{
    public override String Handle(Player actor, float delta)
    {
        Vector2 direction = actor.GetDirection();

        if (actor.IsOnFloor()) {
            if (direction != Vector2.Zero) {
                return "Run";
            }

            return "Idle";
        }

        return null;
    }
}
