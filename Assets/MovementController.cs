using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MovementController
{
    private float moveSpeed;

    public MovementController(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public void MoveTowardsTarget(Transform agentTransform, Vector3 target)
    {
        Vector3 direction = (target - agentTransform.position).normalized;
        agentTransform.position += direction * moveSpeed * Time.deltaTime;
    }

    public bool HasReachedTarget(Transform agentTransform, Vector3 target)
    {
        // Define a small threshold to consider the target as "reached"
        return Vector3.Distance(agentTransform.position, target) < 0.1f;
    }

    public Vector3 SetNewRandomTarget(Vector3 currentPosition, float range = 10f)
    {
        float randomX = Random.Range(currentPosition.x - range, currentPosition.x + range);
        float randomY = Random.Range(currentPosition.y - range, currentPosition.y + range);
        return new Vector3(randomX, randomY, currentPosition.z);
    }

    public void ClearTargetAndResumeWandering(Agent agent)
    {
        agent.targetSource = null;
        agent.currentState = Agent.AgentState.Wandering;
        agent.targetPosition = SetNewRandomTarget(agent.transform.position);
    }
}