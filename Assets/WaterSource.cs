using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class WaterSource : Source
{
    // Implement the HandleDepletion method
    public override void HandleDepletion()
    {
        Debug.Log($"WaterSource empty");

        // If the resource is fully depleted, destroy the food source
        if (resourceAmount <= 0)
        {
            Debug.Log("WaterSource is depleted and will be destroyed.");
            Destroy(gameObject); // Remove the food source from the game
        }
    }
}
