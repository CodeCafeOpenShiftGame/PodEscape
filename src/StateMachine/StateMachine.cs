using Godot;
using System;

public class StateListItem {
    public String Name;
    public State State;
}

public class StateMachine
{
    protected System.Collections.Generic.List<StateListItem> States;
	protected State CurrentState;
	protected State PreviousState;

	public StateMachine()
	{
        this.States = new System.Collections.Generic.List<StateListItem>();
    }

	public virtual void Handle(Player actor, float delta)
	{
        String updatedState = this.CurrentState.Handle(actor, delta);
        if (updatedState != null) {
            this.ChangeState(updatedState);
        }
    }

	public virtual void EnterState(State newState, State oldState)
	{}

	public virtual void ExitState(State oldState, State newState)
	{}

    protected State GetStateByName(String stateName)
    {
        StateListItem item = this.States.Find(s => s.Name == stateName);
        if (item == null) {
            return null;
        }

        return item.State;
    }

	public void ChangeState(String newStateName)
	{
        // GD.Print(newStateName);
        State newState = this.GetStateByName(newStateName);

		this.PreviousState = this.CurrentState;
		this.CurrentState = newState;

		if (this.PreviousState != null) {
			this.ExitState(this.PreviousState, newState);
		}

		if (newState != null) {
			this.EnterState(newState, this.PreviousState);
		}
	}

    public State GetCurrentState()
    {
        return this.CurrentState;
    }

    public void AddState(State state)
    {
        StateListItem stateItem = new StateListItem();
        stateItem.Name = state.Name;
        stateItem.State = state;
        this.States.Add(stateItem);
    }
}
