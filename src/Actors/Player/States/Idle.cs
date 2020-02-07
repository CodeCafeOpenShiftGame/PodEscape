using Godot;
using System;

public class Idle : State
{
    public override String Handle(Player actor, float delta)
    {
        Vector2 direction = actor.GetDirection();

        if (!actor.IsOnFloor()) {
            return "Fall";
        }

        if (direction != Vector2.Zero) {

            if (direction.y < 0) {
                return "Jump";
            }

            return "Run";
        }

        return null;
    }

}
