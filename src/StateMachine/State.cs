using Godot;
using System;

public class State : Node
{
    public virtual String Handle(Player actor, float delta)
    {
        return null;
    }
}
