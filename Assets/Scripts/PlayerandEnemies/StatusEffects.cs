using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum StatusEffect
{
    None,
    Poison,
    Burn,
    Bleeding,
}

public class StatusEffects : MonoBehaviour
{
    private StatusEffect currentEffect;
    private float effectTimer;

    public void ApplyEffect(StatusEffect effect, float duration)
    {
        currentEffect = effect;
        effectTimer = duration;

        switch (effect)
        {
            case StatusEffect.Poison:
                // Set the enemy's properties to simulate poisoning
                Debug.Log("you been poisoned");
                break;

            case StatusEffect.Burn:
                // Set the enemy's properties to simulate burning
                Debug.Log("you are Burning");
                break;

            case StatusEffect.Bleeding:
                // Set the enemy's properties to simulate slowing
                Debug.Log("you are Bleeding");
                break;

                // Add more cases for other effects
        }
    }

    private void Update()
    {
        // Update the effect timer and reset the effect when it expires
        if (currentEffect != StatusEffect.None)
        {
            effectTimer -= Time.deltaTime;
            if (effectTimer <= 0)
            {
                currentEffect = StatusEffect.None;
                // Reset the enemy's properties
            }
        }
    }
}
