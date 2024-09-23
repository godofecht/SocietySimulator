using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public abstract class Source : MonoBehaviour
{
    public float resourceAmount = 1000000000f;  // Amount of resource available at this source
    public float depletionRate = 10f;    // Rate at which the resource is consumed

    // Method to allow an agent to consume the resource
    public virtual float Consume (float amount)
    {
        // If the requested amount is greater than available, return the available amount
        float consumedAmount = Mathf.Min(resourceAmount, amount);

        // Decrease the resource amount
        resourceAmount -= consumedAmount;

        if (resourceAmount < 0) { HandleDepletion(); }

        // Return the actual consumed amount
        return consumedAmount;
    }

    // Check if the source is depleted
    public bool IsDepleted()
    {
        return resourceAmount <= 0;
    }

    // Abstract method that must be implemented by derived classes
    public abstract void HandleDepletion ();
}
