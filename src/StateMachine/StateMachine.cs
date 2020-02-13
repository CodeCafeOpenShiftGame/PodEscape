using Godot;
using Godot.Collections;
using System;

public class StateMachine : Node
{
    [Export]
    public NodePath InitialState;
    public Node Actor;

	protected State CurrentState;
    protected String CurrentStateName = null;

    public StateMachine() : base()
    {
        this.AddToGroup("state_machine");
        GD.Print("StateMachine Initialized");
    }

	async public override void _Ready()
	{
        await ToSignal(this.Owner, "ready");
        this.CurrentState = (State)this.GetNode(this.InitialState);
        this.CurrentState.Enter();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        this.CurrentState.UnhandledInput(@event);
    }

	public override void _PhysicsProcess(float delta)
	{
        this.CurrentState.PhysicsProcess(delta);
    }

    public virtual void TransitionTo(String newStatePath, Dictionary<string, object> msg = null)
    {
        // @TODO @FIXME Make sure that the newStatePath exists
        // if (this.HasNode(newStatePath)) {
        //     return;
        // }

        State newState = (State)this.GetNode(newStatePath);

        // GD.Print($"Transition from {this.CurrentState.Name} to {newState.Name}.");

        this.CurrentState.Exit();
        this.CurrentState = newState;
        this.CurrentState.Enter(msg);
    }

    public virtual void SetCurrentState(State state)
    {
        this.CurrentStateName = state.Name;
        this.CurrentState = state;
    }
}
