//using UnityEngine;
//using System.Linq;
//using System;

//public class MateFinder
//{
//    private Agent agent;

//    public MateFinder(Agent agent)
//    {
//        this.agent = agent;
//    }

//    public void HandleMating()
//    {
//        // Check if the agent is ready to mate (age >= 100 seconds) and is not pregnant
//        if (agent.age >= 100f && agent.isFemale && !agent.isPregnant)
//        {
//            Agent nearestMale = FindNearestMate();
//            if (nearestMale != null)
//            {
//                // 20% chance of getting pregnant
//                if (UnityEngine.Random.value <= 0.2f)
//                {
//                    agent.GetPregnant (nearestMale.genome);
//                }
//            }
//        }
//    }

//    private Agent FindNearestMate()
//    {
//        // Find all nearby agents
//        Agent[] allAgents = UnityEngine.Object.FindObjectsOfType<Agent>();

//        // Filter for the nearest male agent who is ready to mate (age >= 100 seconds)
//        return allAgents
//            .Where(agent => !agent.isFemale && agent.age >= 100f)
//            .OrderBy(agent => Vector3.Distance(this.agent.transform.position, agent.transform.position))
//            .FirstOrDefault();
//    }
//}
