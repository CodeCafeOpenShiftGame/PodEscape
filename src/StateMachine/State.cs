using Godot;
using Godot.Collections;
using System;

public class State : Node
{
    public StateMachine StateMachine;
    public State Parent;

    async public override void _Ready()
    {
        await ToSignal(this.Owner, "ready");
        this.Parent = this.GetParent() as State;
        this.StateMachine = this.GetStateMachine(this);
    }

    public virtual void UnhandledInput(InputEvent @event)
    {}

    public virtual void PhysicsProcess(float delta)
    {}

    public virtual void Enter(Dictionary msg = null)
    {}

    public virtual void Exit()
    {}

    public virtual StateMachine GetStateMachine(Node node = null)
    {
        if (node != null && !node.IsInGroup("state_machine")) {
            return this.GetStateMachine(node.GetParent());
        }
        return (StateMachine)node;
    }
}
