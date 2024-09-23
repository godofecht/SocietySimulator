using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;
using System.Diagnostics.SymbolStore;

public class Agent : MonoBehaviour
{
    public StateMachine stateManager;

    // Agent states
    public enum AgentState { Wandering, GoingTo, Eating, Drinking, Resting, Mating, Pregnant, Aggressive, Grouping }
    public AgentState currentState = AgentState.Wandering;

    [SerializeField] public Genome genome;
    [SerializeField] public float age = 0f;

    public bool isFemale = false;

    // Managers for different agent functionalities
    [SerializeField] public NeedsManager needsManager;
    [SerializeField] public PregnancyManager pregnancyManager;
    [SerializeField] public MovementController movementController;
    [SerializeField] public RelationshipManager relationshipManager;

    // Define the possible states
    public WanderingState WanderingState { get; private set; }
    public GoingToState GoingToFoodState { get; private set; }
    public GoingToState GoingToWaterState { get; private set; }
    public EatingState EatingState { get; private set; }
    public DrinkingState DrinkingState { get; private set; }
    public RestingState RestingState { get; private set; }

    [SerializeField] public SpriteRenderer spriteRenderer;
    public Vector3 targetPosition;
    public GameObject targetSource;
    public Color originalColor;
    public Color restingColor;

    void Start()
    {
        // Initialize the state manager
        stateManager = new StateMachine();

        // Initialize the states
        WanderingState = new WanderingState(this, stateManager);
        GoingToFoodState = new GoingToState(this, stateManager, "FoodSource");
        GoingToWaterState = new GoingToState(this, stateManager, "WaterSource");
        EatingState = new EatingState(this, stateManager);
        DrinkingState = new DrinkingState(this, stateManager);
        RestingState = new RestingState(this, stateManager);

        // Initialize the agent's components
        needsManager = new NeedsManager(genome);
        movementController = new MovementController(genome.moveSpeed);

        // Start with the Wandering state
        stateManager.Initialize(WanderingState);

        // Initialize sprite renderer and visual properties
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = genome.ethnicGroupColor;
            spriteRenderer.color = originalColor;
            restingColor = originalColor * 0.85f;
        }
    }

    void Update()
    {
        stateManager.UpdateState();
        needsManager.UpdateNeeds (currentState);
    }

    public GameObject FindNearestSource(string tag)
    {
        GameObject[] sources = GameObject.FindGameObjectsWithTag(tag);
        if (sources.Length == 0) return null;
        return sources.OrderBy(source => Vector3.Distance(transform.position, source.transform.position)).FirstOrDefault();
    }


    void CheckForNearbySources()
    {
        // Find the nearest food or water source and move towards it if a need is low
        if (needsManager.Food.Value <= 30f && currentState == AgentState.Wandering)
        {
            targetSource = FindNearestSource("FoodSource");
            if (targetSource != null)
            {
                currentState = AgentState.GoingTo;
            }
        }
        else if (needsManager.Water.Value <= 30f && currentState == AgentState.Wandering)
        {
            targetSource = FindNearestSource("WaterSource");
            if (targetSource != null)
            {
                currentState = AgentState.GoingTo;
            }
        }
    }

    void HandleGrouping()
    {
        List<Agent> nearbyAgents = GetNearbyAgents();
        foreach (Agent other in nearbyAgents)
        {
            var knownAgent = relationshipManager.FindOrAddKnownAgent(other);

            // Increase affinity if agents share the same phenotype
            if (other.genome.phenotype == genome.phenotype)
            {
                relationshipManager.UpdateAffinity(knownAgent, 1f);
            }

            // Form a group with agents that have positive affinity and are friends
            if (knownAgent.status == RelationshipManager.RelationshipStatus.Friend && !relationshipManager.GetFriends().Contains(knownAgent))
            {
                // Form group logic
            }
        }
    }

    void Wander()
    {
        movementController.MoveTowardsTarget(transform, targetPosition);
        if (movementController.HasReachedTarget(transform, targetPosition))
        {
            targetPosition = movementController.SetNewRandomTarget(transform.position);
        }
    }



    void Rest()
    {
        needsManager.ReplenishRest();
        if (needsManager.IsFullyRested())
        {
            needsManager.SetFullEnergy();
            currentState = AgentState.Wandering;
            spriteRenderer.color = originalColor;
        }
    }

    void CheckHealthAndRest()
    {
        if (needsManager.IsHealthCritical())
        {
            genome.health -= 10f * Time.deltaTime;
        }

        if (genome.health <= 0)
        {
            CeaseToExist();
        }

        if (needsManager.ShouldRest(30f) && currentState != AgentState.Resting)
        {
            currentState = AgentState.Resting;
            spriteRenderer.color = restingColor;
        }
    }

    void UpdatePregnancy()
    {
        if (pregnancyManager.IsPregnant)
        {
            pregnancyManager.UpdatePregnancy(ref currentState);
            if (pregnancyManager.IsReadyToGiveBirth())
            {
                Agent baby = pregnancyManager.GiveBirth(gameObject, transform);
                // Handle baby initialization logic
            }
        }
    }

    public void ConsumeFromSource(Source source)
    {
        if (source == null)
        {
            Debug.LogWarning("Source is null. Cannot consume.");
            return;
        }

        // Get needs from NeedsManager
        Need water = needsManager.Water;
        Need food = needsManager.Food;

        // Create a mapping from source types to the corresponding needs
        Dictionary<System.Type, Need> sourceNeedMap = new Dictionary<System.Type, Need>
        {
            { typeof(WaterSource), water },
            { typeof(FoodSource), food }
            // Add other source types and corresponding needs here if necessary
        };

        System.Type sourceType = source.GetType();

        if (sourceNeedMap.TryGetValue(sourceType, out Need need))
        {
            float neededAmount = 10f;
            float consumedAmount = source.Consume(neededAmount);

            need.SetValue(need.Value + consumedAmount);

            if (need.IsSatisfied())
            {
                movementController.ClearTargetAndResumeWandering(this);
            }
        }
        else
        {
            Debug.LogWarning($"No matching need found for source type {sourceType}");
        }
    }

    List<Agent> GetNearbyAgents()
    {
        Agent[] allAgents = FindObjectsOfType<Agent>();
        return new List<Agent>(allAgents).FindAll(agent => agent != this && Vector3.Distance(transform.position, agent.transform.position) < 5f);
    }

    void CeaseToExist()
    {
        Destroy(gameObject);
    }
}