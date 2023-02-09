using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum to store the different overheating states
public enum OverheatState
{
    Cool,       // The system is not overheating
    Warning,    // The system is approaching overheating
    Overheated  // The system has overheated
}

// Class to handle the overheating mechanic
public class OverheatHandler: MonoBehaviour
{
    // Treshold settings for the overheating.
    [SerializeField] private int coolThreshold, warningTreshold, overheatThreshold;

    // The duration of the cooldown in seconds
    public float cooldownDuration = 2.0f;

    public float cooldownRate;

    // Boolean to track whether the system is currently cooling down
    private bool _isCoolingDown = false;

    // Method to check the current overheating state
    public OverheatState CheckOverheat(int overheating)
    {
        if(overheating <= coolThreshold)
        {
            // The system is not overheating
            _isCoolingDown = false;
            return OverheatState.Cool;
        }
        else if(overheating > warningTreshold)
        {
            // The system is approaching overheating
            return OverheatState.Warning;
        }
        else if(overheating > overheatThreshold)
        {
            // The system has overheated
            if (!_isCoolingDown)
            {
                // Start the cooldown if it's not already in progress
                StartCoroutine(CoolDown());                
            }
            return OverheatState.Overheated;
        }
        return OverheatState.Cool;
       
    }

    // Coroutine to handle the cooldown period
    public IEnumerator CoolDown()
    {
        // Set the bool to indicate that the cooldown has started
        _isCoolingDown = true;

        // Log a message indicating that the cooldown has started
        Debug.Log("Weapon is overheated, starting cooldown");

        // Wait for the cooldown duration
        yield return new WaitForSeconds(cooldownDuration);

        // Set the bool to indicate that the cooldown has completed
        _isCoolingDown = false;

        // Log a message indicating that the cooldown has completed
        Debug.Log("Weapon cooldown complete");
    }
}
