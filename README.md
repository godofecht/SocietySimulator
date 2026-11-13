# SocietySimulator

A society simulator that surfaces complex population dynamics.

## What this is

A 2D simulation made in [Unity](https://unity.com/) (a game engine) and written in C#. Fifty agents (simulated people) appear on screen as coloured squares and triangles. Each one has needs for food and water that run down over time. When a need gets low, the agent walks to the nearest food or water source, eats or drinks until it is full, then goes back to wandering.

Each agent carries a genome (a set of inherited values) that sets how fast it moves and how fast its needs run down. The project is planned to grow into a society with resting, death, relationships, groups and children. The code for those parts is written but most of it is not yet switched on. "Project status" below lists exactly what runs today.

## What you can learn from it

- Computer Science: finite state machines. Each agent is always in one state (Wandering, GoingTo, Eating, Drinking, Resting) and moves between states when a condition is met.
- Computer Science: object-oriented design in C#, with abstract classes (`Source`, `State`) and subclasses (`FoodSource`, `WaterSource`, `EatingState`).
- Biology: genotype and phenotype. `Genome.cs` holds the inherited values and uses the agent's colour as its visible trait. `Genome.Splice` shows one simple model of inheritance: a child gets the average of its parents' values.
- Psychology: in-group preference. `RelationshipManager.cs` raises affinity between agents that look alike and labels them Friend or Enemy past a threshold. This code exists but is not yet called.

## Run it

### On your own computer

**You will need:** [Unity Hub](https://unity.com/download) and the Unity Editor version **2022.3.43f1** (install it from Unity Hub's Installs tab, or the [Unity download archive](https://unity.com/releases/editor/archive)). Unity runs on Windows, macOS and Linux. No operating system has been tested for this guide.

1. Download the code:

   ```sh
   git clone https://github.com/godofecht/SocietySimulator.git
   ```

2. Open Unity Hub, choose **Add** (or **Open**), and pick the `SocietySimulator` folder.
3. When the project opens, go to the **Project** window and open `Assets/Scenes/SampleScene.unity`.
4. Press the **Play** button at the top of the editor.

You should see 50 squares and triangles in two colours moving around three food sources and one water source. The **Console** window fills with messages such as "Entering Wandering State" as agents change state.

#### Controls (in Play mode)

| Input | Does |
| --- | --- |
| Left mouse drag | move the camera |
| Mouse scroll wheel | zoom in and out |

## How it works

`SimulationManager` spawns the agents at random positions. Each agent picks a square (male) or triangle (female) shape at random, and one of the colours set on the `SimulationController` object in the scene.

Every frame, each agent does two things:

1. Its state machine runs the current state. Wandering walks to random points. When food drops to 30 or less, it switches to going to the nearest object tagged `FoodSource`; when water drops to 30 or less, to the nearest `WaterSource`. On arrival it eats or drinks 10 units a frame until the need reaches 100.
2. Its `NeedsManager` lowers food, water, energy and rest by the rates in its genome.

| File or folder | What is in it |
| --- | --- |
| `Assets/Agent.cs` | the agent, its components and its state machine |
| `Assets/AgentStates.cs` | each state and its transition conditions |
| `Assets/NeedsManager.cs`, `Assets/Need.cs` | need levels, depletion and refilling |
| `Assets/Genome.cs` | inherited values, the colour phenotype, and splicing two parents |
| `Assets/RelationshipManager.cs` | affinity and Friend or Enemy status |
| `Assets/PregnancyManager.cs` | pregnancy and birth |
| `Assets/MateFinder.cs` | choosing a mate (all commented out) |
| `Assets/Source.cs`, `Assets/FoodSource.cs`, `Assets/WaterSource.cs` | consumable sources |
| `Assets/MovementController.cs` | moving towards a target and picking random targets |
| `Assets/SimulationManager.cs` | spawning the population |
| `Assets/CameraController.cs` | camera drag and zoom |
| `Assets/Scenes/SampleScene.unity` | the scene: food, water, camera and the spawner |

## Ideas to try

1. Select **SimulationController** in the scene and change **Initial Population** in the Inspector. Add a third colour to **Group Colors**.
2. In `Assets/AgentStates.cs`, change the hunger and thirst threshold of `30f` and see how the crowd around the sources changes.
3. Make the food run out. In the scene, lower **Resource Amount** on a Food object. `Assets/FoodSource.cs` removes a source when it is empty.
4. Switch on death and rest. `Assets/Agent.cs` has a `CheckHealthAndRest` method that is never called. Call it from `Update` and watch agents that cannot reach food disappear. Count the survivors over time.
5. Switch on grouping. Call `HandleGrouping` from `Update` in `Assets/Agent.cs`, then colour agents by how many friends they have.
6. Ambitious: finish mating and births. Rewrite `Assets/MateFinder.cs` so it calls `PregnancyManager.StartPregnancy`, then plot population size over time to see real population dynamics.

## Technical details

The design, as described in the earlier README:

Agents with needs, genomes and relationships, living in a Unity scene and producing group behaviour nobody wrote down.

Every agent runs a state machine. States are Wandering, GoingTo, Eating, Drinking, Resting, Mating, Pregnant, Aggressive and Grouping. Transitions come from the needs, so an agent leaves Wandering when a need drops far enough and there is a source it can reach.

**Needs** are food, water, energy and rest. Each depletes at a rate the agent's genome sets, and each is replenished by a matching source type in the scene. `Agent.ConsumeFromSource` maps each source type to its need.

**Genome** carries health, move speed, the four depletion rates, the rest replenish rate, and a colour that stands in for a visible phenotype. Offspring genomes are spliced from both parents.

**Affinity** rises between agents that share a phenotype. Agents with positive affinity are meant to form groups: the colour is arbitrary, but grouping by it is meant to produce stable clusters that then compete for the same food and water.

Of the nine states, five have state classes in `AgentStates.cs` (Wandering, GoingTo, Eating, Drinking, Resting). Mating, Pregnant, Aggressive and Grouping are names in the `AgentState` list with no code behind them yet.

Settings from the scene: 50 agents, two colours, three food sources and one water source. The prefabs `Square 1.prefab` (male) and `Triangle 1.prefab` (female) start each agent with health 100, move speed 2, and all depletion rates at 1 per second. Food and water start at 50 out of 100.

## Project status

This is an early prototype. What runs today:

- Agents spawn, wander, and go to food and water when hungry or thirsty.
- Needs run down and refill.
- Camera drag and zoom.

Written but not yet called from anywhere:

- Resting, health loss and death (`CheckHealthAndRest` and `Rest` in `Agent.cs`; the Resting state exists but nothing switches to it).
- Affinity and grouping (`HandleGrouping` in `Agent.cs`; the "form group" step is an empty placeholder).
- Pregnancy and birth (`PregnancyManager.cs` is never started).
- Mate finding (`MateFinder.cs` is fully commented out and refers to methods that no longer exist).

Because nobody is born or dies yet, the population stays at 50.

## Licence

You can use, copy and change this code for any purpose, as long as you keep the copyright notice. See [LICENSE](LICENSE) (MIT).

## Credits

- Written by Abhishek Shivakumar. Citation details are in [CITATION.cff](CITATION.cff).
- Built with [Unity](https://unity.com/) 2022.3.43f1.
