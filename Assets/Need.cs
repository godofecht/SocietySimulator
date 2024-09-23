using UnityEngine;
using System.Linq;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

[System.Serializable]
public class Need
{
    [SerializeField] private float value;
    [SerializeField] private float maxValue;
    [SerializeField] private float depletionRate;
    [SerializeField] private float replenishRate;

    public Need(float initialValue, float maxValue, float depletionRate, float replenishRate)
    {
        this.value = initialValue;
        this.maxValue = maxValue;
        this.depletionRate = depletionRate;
        this.replenishRate = replenishRate;
    }

    public float Value => value;
    public float MaxValue => maxValue;

    // Deplete the need over time
    public void Deplete(float deltaTime)
    {
        value = Mathf.Max(0, value - depletionRate * deltaTime);
    }

    // Replenish the need over time
    public void Replenish(float deltaTime)
    {
        value = Mathf.Min(maxValue, value + replenishRate * deltaTime);
    }

    // Check if the need is fully depleted
    public bool IsDepleted()
    {
        return value <= 0;
    }

    // Check if the need is fully satisfied
    public bool IsSatisfied()
    {
        return value >= maxValue;
    }

    // Allow setting the need's value from the editor
    public void SetValue(float newValue)
    {
        value = Mathf.Clamp(newValue, 0, maxValue);
    }
}
