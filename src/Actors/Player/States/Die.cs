using Godot;
using System;

public class Die : State
{
    public override String Handle(Player actor, float delta)
    {
        GD.Print("Player is Dead");
        actor.IsDead = true;

        return null;
    }
}
