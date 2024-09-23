using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class FoodSource : Source
{
    // Implement the HandleDepletion method
    public override void HandleDepletion ()
    {
        Debug.Log($"FoodSource empty");

        // If the resource is fully depleted, destroy the food source
        if (resourceAmount <= 0)
        {
            Debug.Log("FoodSource is depleted and will be destroyed.");
            Destroy(gameObject); // Remove the food source from the game
        }
    }
}
