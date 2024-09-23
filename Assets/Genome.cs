using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class Genome
{
    public float health;
    public float moveSpeed;
    public float foodDepletionRate;
    public float waterDepletionRate;
    public float energyDepletionRate;
    public float restReplenishRate;
    public Color ethnicGroupColor; // Example of a phenotype

    // Example phenotype (can be any trait)
    public string phenotype;

    public Genome(float health, float moveSpeed, float foodDepletionRate, float waterDepletionRate,
                  float energyDepletionRate, float restReplenishRate, Color ethnicGroupColor)
    {
        this.health = health;
        this.moveSpeed = moveSpeed;
        this.foodDepletionRate = foodDepletionRate;
        this.waterDepletionRate = waterDepletionRate;
        this.energyDepletionRate = energyDepletionRate;
        this.restReplenishRate = restReplenishRate;
        this.ethnicGroupColor = ethnicGroupColor;

        // Set phenotype based on some genome characteristics (e.g., color, size)
        this.phenotype = ethnicGroupColor.ToString(); // For simplicity, using color as phenotype here
    }

    // Example method to splice two genomes together (for offspring)
    public static Genome Splice(Genome parent1, Genome parent2)
    {
        // Splicing logic, mix attributes of both parents to create a new genome
        return new Genome(
            (parent1.health + parent2.health) / 2,
            (parent1.moveSpeed + parent2.moveSpeed) / 2,
            (parent1.foodDepletionRate + parent2.foodDepletionRate) / 2,
            (parent1.waterDepletionRate + parent2.waterDepletionRate) / 2,
            (parent1.energyDepletionRate + parent2.energyDepletionRate) / 2,
            (parent1.restReplenishRate + parent2.restReplenishRate) / 2,
            Color.Lerp(parent1.ethnicGroupColor, parent2.ethnicGroupColor, 0.5f)
        );
    }
}
