using UnityEngine;

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    private int startingHealth;
    private int currentHealth;

    public void SetStartingHealth(int value)
    {
        startingHealth = value;
    }

    public int GetStartingHealth()
    {
        return startingHealth;
    }
}