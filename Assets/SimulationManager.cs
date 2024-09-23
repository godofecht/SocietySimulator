using System;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public GameObject malePrefab; // Prefab for square (male)
    public GameObject femalePrefab; // Prefab for triangle (female)
    public int initialPopulation = 1; // Number of agents to spawn
    public Color[] ethnicGroupColors; // Array of colors representing different ethnic groups

    void Start()
    {
        // Ensure prefabs are assigned before proceeding
        if (malePrefab == null || femalePrefab == null)
        {
            UnityEngine.Debug.LogError("Prefabs are not assigned in the Inspector!");
            return; // Exit the method to prevent further errors
        }

        // Ensure that ethnicGroupColors array has at least one color
        if (ethnicGroupColors.Length == 0)
        {
            UnityEngine.Debug.LogError("No ethnic group colors assigned!");
            return; // Exit the method to prevent further errors
        }

        // Spawn initial population
        for (int i = 0; i < initialPopulation; i++)
        {
            // Randomly choose male or female using UnityEngine.Random
            GameObject agentPrefab = (UnityEngine.Random.value > 0.5f) ? malePrefab : femalePrefab;

            // Instantiate the agent at a random position
            Vector2 randomPosition = new Vector2(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-4f, 4f));
            GameObject agent = Instantiate(agentPrefab, randomPosition, Quaternion.identity);

            // Assign a random ethnic group color to the agent
            Agent agentComponent = agent.GetComponent<Agent>();
            if (agentComponent != null)
            {
                agentComponent.genome.ethnicGroupColor = ethnicGroupColors[UnityEngine.Random.Range(0, ethnicGroupColors.Length)];
            }
            else
            {
                UnityEngine.Debug.LogError("Agent prefab is missing the Agent script!");
            }
        }

        direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    public float moveSpeed = 2.0f;  // Speed of the agent
    private Vector2 direction;  // Direction of movement

    void Update()
    {
        // Move the agent based on the direction and speed
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
}
