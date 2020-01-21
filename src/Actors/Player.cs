using Godot;
using System;

public class Player : Actor
{
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        base._Process(delta);

        Vector2 direction = GetDirection();
        Vector2 snap = GetSnapPosition(direction);

        Velocity = CalculateMovementVelocity(
            Velocity,
            direction,
            Speed
        );

        Velocity = MoveAndSlideWithSnap(
            Velocity,
            snap,
            FLOOR_NORMAL,
            true
        );
    }

    private Vector2 GetSnapPosition(Vector2 direction)
    {
        return direction.y == 0 ? Vector2.Down * 60 : Vector2.Zero;
    }

    private Vector2 GetDirection()
    {
        return new Vector2(
            Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
            IsOnFloor() && Input.IsActionJustPressed("move_up") ? -Input.GetActionStrength("move_up") : 0
        );
    }

    private Vector2 CalculateMovementVelocity(
        Vector2 linearVelocity,
        Vector2 direction,
        Vector2 speed
    ) {
        Vector2 moveVelocity = linearVelocity;
        moveVelocity.x = speed.x * direction.x;
        if (direction.y != 0) {
            moveVelocity.y = speed.y * direction.y;
        }
        return moveVelocity;
    }
}
