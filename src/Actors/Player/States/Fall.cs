using Godot;
using System;

public class Fall : State
{
    public override String Handle(Player actor, float delta)
    {
        Vector2 direction = actor.GetDirection();

        if (actor.IsOnFloor()) {
            return "Idle";
        }

        Vector2 snap = actor.GetSnapPosition(direction);

        actor.Velocity = actor.CalculateVelocity(
            actor.Velocity,
            direction,
            actor.Speed
        );

        actor.Velocity = actor.MoveAndSlideWithSnap(
            actor.Velocity,
            snap,
            actor.FLOOR_NORMAL,
            true
        );

        return null;
    }
}
