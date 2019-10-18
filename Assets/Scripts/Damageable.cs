using UnityEngine;

// Class/component that allows for an object to take damage and to be removed once health is low enough

public class Damageable : MonoBehaviour {

    public int maxHealth;        
    private int health;

    private void Start() {
        health = maxHealth;
    }
    
    public void TakeDamage(int damageDealt) {
        health -= damageDealt;

        if (health <= 0) KillSelf();
    }

    public void RecieveHealth(float amount) {
        RecieveHealth(Mathf.RoundToInt(amount));
    }

    public void RecieveHealth(int amount) {
        health += amount;

        if (health > maxHealth) health = maxHealth;
    }

    private void KillSelf() {
        SendMessageUpwards("StartDeath", SendMessageOptions.RequireReceiver);
    }

    public int GetMaxHealth(){
        return maxHealth;
    }
}