using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PregnancyManager
{
    [SerializeField] private bool isPregnant;
    [SerializeField] private float pregnancyTime;
    [SerializeField] private const float pregnancyDuration = 50f;
    [SerializeField] private Genome babyGenome;

    public bool IsPregnant => isPregnant;

    public void StartPregnancy(Genome motherGenome, Genome fatherGenome)
    {
        isPregnant = true;
        pregnancyTime = 0f;
        babyGenome = Genome.Splice(motherGenome, fatherGenome);
    }

    public void UpdatePregnancy(ref Agent.AgentState currentState)
    {
        if (!isPregnant) return;

        pregnancyTime += Time.deltaTime;
        if (pregnancyTime >= pregnancyDuration * 0.9f)
        {
            currentState = Agent.AgentState.Pregnant;
        }
    }

    public bool IsReadyToGiveBirth() => pregnancyTime >= pregnancyDuration;

    public Agent GiveBirth(GameObject agentPrefab, Transform parentTransform)
    {
        isPregnant = false;
        pregnancyTime = 0f;

        GameObject baby = GameObject.Instantiate(agentPrefab, parentTransform.position, Quaternion.identity);
        Agent babyAgent = baby.GetComponent<Agent>();
        babyAgent.genome = babyGenome;
        babyAgent.age = 0f;
        babyAgent.pregnancyManager.isPregnant = false;
        babyAgent.isFemale = UnityEngine.Random.value <= 0.5f;

        return babyAgent;
    }
}
