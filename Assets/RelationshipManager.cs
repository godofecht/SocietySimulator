using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class RelationshipManager
{
    public enum RelationshipStatus { Neutral, Friend, Enemy }

    [System.Serializable]
    public class KnownAgent
    {
        public Agent agent;
        public float affinity;
        public RelationshipStatus status;

        public KnownAgent(Agent agent)
        {
            this.agent = agent;
            this.affinity = 0f;
            this.status = RelationshipStatus.Neutral;
        }
    }

    [SerializeField] private List<KnownAgent> knownAgents = new List<KnownAgent>();

    public List<KnownAgent> GetKnownAgents() => knownAgents;

    public KnownAgent FindOrAddKnownAgent(Agent agent)
    {
        KnownAgent knownAgent = knownAgents.FirstOrDefault(ka => ka.agent == agent);
        if (knownAgent == null)
        {
            knownAgent = new KnownAgent(agent);
            knownAgents.Add(knownAgent);
        }
        return knownAgent;
    }

    public void UpdateAffinity(KnownAgent knownAgent, float affinityChange)
    {
        knownAgent.affinity += affinityChange;
        UpdateStatus(knownAgent);
    }

    private void UpdateStatus(KnownAgent knownAgent)
    {
        if (knownAgent.affinity >= 10f)
        {
            knownAgent.status = RelationshipStatus.Friend;
        }
        else if (knownAgent.affinity <= -10f)
        {
            knownAgent.status = RelationshipStatus.Enemy;
        }
        else
        {
            knownAgent.status = RelationshipStatus.Neutral;
        }
    }

    public IEnumerable<KnownAgent> GetFriends() => knownAgents.Where(ka => ka.status == RelationshipStatus.Friend);
}
