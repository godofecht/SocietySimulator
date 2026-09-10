# SocietySimulator

Agents with needs, genomes and relationships, living in a Unity scene and
producing group behaviour nobody wrote down.

Every agent runs a state machine. States are Wandering, GoingTo, Eating,
Drinking, Resting, Mating, Pregnant, Aggressive and Grouping. Transitions come
from the needs, so an agent leaves Wandering when a need drops far enough and
there is a source it can reach.

**Needs** are food, water, energy and rest. Each depletes at a rate the agent's
genome sets, and each is replenished by a matching source type in the scene.
`NeedsManager` owns the mapping from source type to need.

**Genome** carries health, move speed, the four depletion rates, the rest
replenish rate, and a colour that stands in for a visible phenotype. Offspring
genomes are spliced from both parents.

**Affinity** rises between agents that share a phenotype. Agents with positive
affinity form groups, which is where the interesting behaviour comes from: the
colour is arbitrary, but grouping by it produces stable clusters that then
compete for the same food and water.

| Script | Responsibility |
|---|---|
| `Agent.cs` | The agent, its components and its state machine. |
| `AgentStates.cs` | Each state and its transition conditions. |
| `NeedsManager.cs`, `Need.cs` | Need levels, depletion, source matching. |
| `Genome.cs` | Traits, phenotype, splicing. |
| `RelationshipManager.cs`, `MateFinder.cs`, `PregnancyManager.cs` | Affinity, pairing, offspring. |
| `FoodSource.cs`, `WaterSource.cs` | Consumable sources. |
| `SimulationManager.cs` | Spawning and the simulation clock. |

## Run

Open the project in Unity and load the scene. Scripts are under `Assets/`.
