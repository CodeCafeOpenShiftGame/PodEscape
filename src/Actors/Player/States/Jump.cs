using Godot;
using System;

public class Jump : State
{
    public override String Handle(Player actor, float delta)
    {
        // @FIXME The Jump and Run states must to be reviewd
        Vector2 direction = actor.GetDirection();

        if (!actor.IsOnFloor()) {
            return "Fall";
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

        actor.AnimationPlayer.Play("Jump");

        return null;
    }
}
