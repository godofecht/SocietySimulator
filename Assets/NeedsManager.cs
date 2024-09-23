using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

[System.Serializable]

public class NeedsManager
{
    [SerializeField] private Need food;
    [SerializeField] private Need water;
    [SerializeField] private Need energy;
    [SerializeField] private Need rest;

    // Constructor that infers parameters from the Genome object
    public NeedsManager(Genome genome)
    {
        if (genome == null)
        {
            Debug.LogError("Genome is null! NeedsManager cannot be initialized.");
            return;
        }

        // Ensure that all Need objects are properly initialized with values derived from the Genome
        food = new Need(50f, 100f, genome.foodDepletionRate, 0f);
        water = new Need(50f, 100f, genome.waterDepletionRate, 0f);
        energy = new Need(100f, 100f, genome.energyDepletionRate, 0f);
        rest = new Need(100f, 100f, genome.energyDepletionRate, genome.energyDepletionRate);
    }

    public void UpdateNeeds(Agent.AgentState currentState)
    {
        // Ensure that none of the needs are null before trying to update them
        if (food == null || water == null || energy == null || rest == null)
        {
            Debug.LogError("One or more Need objects are not initialized in NeedsManager!");
            return;
        }

        // Deplete food and water if the agent is not eating or drinking
        if (currentState != Agent.AgentState.Eating)
            food.Deplete(Time.deltaTime);

        if (currentState != Agent.AgentState.Drinking)
            water.Deplete(Time.deltaTime);

        // Deplete energy and rest while not resting
        if (currentState != Agent.AgentState.Resting)
        {
            energy.Deplete(Time.deltaTime);
            rest.Deplete(Time.deltaTime);
        }
    }

    public bool IsHealthCritical() => food.IsDepleted() || water.IsDepleted();
    public bool ShouldRest(float energyThreshold) => energy.Value <= energyThreshold && rest.Value < rest.MaxValue;

    public void ReplenishRest()
    {
        rest.Replenish(Time.deltaTime);
        energy.Replenish(Time.deltaTime);
    }

    public bool IsFullyRested() => rest.IsSatisfied();
    public void SetFullEnergy() => energy.SetValue(energy.MaxValue);

    // Public getters for accessing needs
    public Need Food => food;
    public Need Water => water;
    public Need Energy => energy;
    public Need Rest => rest;
}