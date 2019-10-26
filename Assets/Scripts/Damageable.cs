using UnityEngine;

// Class/component that allows for an object to take damage and to be removed once health is low enough

public class Damageable : MonoBehaviour {
    
    public delegate void HealthChanged(int healthDisplayValue);
    public event HealthChanged healthChanged;

    public int maxHealth;        
    private int currentHealth;
    
    private void Start() {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damageDealt) {

        currentHealth -= damageDealt;

        if (healthChanged != null) healthChanged(GetCurrentHealth());

        if (currentHealth <= 0) KillSelf();
    }

    public void RecieveHealth(float amount) {
        RecieveHealth(Mathf.RoundToInt(amount));
    }

    public void RecieveHealth(int amount) {
        currentHealth += amount;

        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (healthChanged != null) healthChanged(GetCurrentHealth());
    }

    private void KillSelf() {
        SendMessageUpwards("StartDeath", SendMessageOptions.RequireReceiver);
    }

    public int GetCurrentHealth(){
        return currentHealth;
    }

    public int GetMaxHealth(){
        return maxHealth;
    }
}