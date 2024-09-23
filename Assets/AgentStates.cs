using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public abstract class State
{
    protected Agent agent;
    protected StateMachine stateMachine;

    protected State(Agent agent, StateMachine stateMachine)
    {
        this.agent = agent;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void Exit() { }
}

public class WanderingState : State
{
    public WanderingState(Agent agent, StateMachine stateMachine) : base(agent, stateMachine) { }

    public override void Enter()
    {
        agent.targetPosition = agent.movementController.SetNewRandomTarget(agent.transform.position);
        Debug.Log("Entering Wandering State");
    }

    public override void LogicUpdate()
    {
        agent.movementController.MoveTowardsTarget(agent.transform, agent.targetPosition);

        if (agent.movementController.HasReachedTarget(agent.transform, agent.targetPosition))
        {
            agent.targetPosition = agent.movementController.SetNewRandomTarget(agent.transform.position);
        }

        // Transition to another state if conditions are met
        if (agent.needsManager.Food.Value <= 30f)
        {
            stateMachine.ChangeState(agent.GoingToFoodState);
        }
        else if (agent.needsManager.Water.Value <= 30f)
        {
            stateMachine.ChangeState(agent.GoingToWaterState);
        }
    }
}

public class GoingToState : State
{
    private string targetTag;

    public GoingToState(Agent agent, StateMachine stateMachine, string targetTag) : base(agent, stateMachine)
    {
        this.targetTag = targetTag;
    }

    public override void Enter()
    {
        agent.targetSource = agent.FindNearestSource(targetTag);
        if (agent.targetSource == null)
        {
            stateMachine.ChangeState(agent.WanderingState); // Go back to wandering if no target found
            Debug.Log("No target found, returning to Wandering State");
        }
        else
        {
            Debug.Log("Going to " + targetTag);
        }
    }

    public override void LogicUpdate()
    {
        if (agent.targetSource != null)
        {
            agent.movementController.MoveTowardsTarget(agent.transform, agent.targetSource.transform.position);

            if (agent.movementController.HasReachedTarget(agent.transform, agent.targetSource.transform.position))
            {
                if (targetTag == "FoodSource")
                {
                    stateMachine.ChangeState(agent.EatingState);
                }
                else if (targetTag == "WaterSource")
                {
                    stateMachine.ChangeState(agent.DrinkingState);
                }
            }
        }
    }
}

public class EatingState : State
{
    public EatingState(Agent agent, StateMachine stateMachine) : base(agent, stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Entering Eating State");
    }

    public override void LogicUpdate()
    {
        agent.ConsumeFromSource(agent.targetSource.GetComponent<Source>());
        if (agent.needsManager.Food.IsSatisfied())
        {
            stateMachine.ChangeState(agent.WanderingState);
        }
    }
}

public class DrinkingState : State
{
    public DrinkingState(Agent agent, StateMachine stateMachine) : base(agent, stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Entering Drinking State");
    }

    public override void LogicUpdate()
    {
        agent.ConsumeFromSource(agent.targetSource.GetComponent<Source>());
        if (agent.needsManager.Water.IsSatisfied())
        {
            stateMachine.ChangeState(agent.WanderingState);
        }
    }
}

public class RestingState : State
{
    public RestingState(Agent agent, StateMachine stateMachine) : base(agent, stateMachine) { }

    public override void Enter()
    {
        agent.spriteRenderer.color = agent.restingColor;
        Debug.Log("Entering Resting State");
    }

    public override void LogicUpdate()
    {
        agent.needsManager.ReplenishRest();
        if (agent.needsManager.IsFullyRested())
        {
            agent.needsManager.SetFullEnergy();
            stateMachine.ChangeState(agent.WanderingState);
            agent.spriteRenderer.color = agent.originalColor;
        }
    }
}

public class StateMachine
{
    public State CurrentState { get; private set; }

    public void Initialize(State startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }

    public void ChangeState(State newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        newState.Enter();
    }

    public void LogicUpdate()
    {
        CurrentState.LogicUpdate();
    }

    public void PhysicsUpdate()
    {
        CurrentState.PhysicsUpdate();
    }

    public void UpdateState()
    {
        // Use LogicUpdate for logic processing during each frame
        LogicUpdate();

        // If you have physics-related updates, use PhysicsUpdate, typically called in FixedUpdate()
        // PhysicsUpdate();  // Uncomment this if needed in FixedUpdate()
    }
}


