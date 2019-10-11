using UnityEngine;

// Class/component that allows for an object to take damage and to be removed once health is low enough
// Update entity type once Ant has been renamed

public enum EntityType { Player, Ant }

public class Damageable : MonoBehaviour {

    public int maxHealth;    
    public EntityType type;
    public GameObject Loot;

    private int lootGenThreshold;
    private int health;

    private void Start() {
        lootGenThreshold = Random.Range(0, 101);
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

    // allows for the any types to drop loop when they die
    private void KillSelf() {

        switch (type) {
            case EntityType.Ant:
                Instantiate(Loot, transform.position, Quaternion.identity);
                Destroy(gameObject);
                break;
                
            default: Destroy(gameObject);
                break;
        }        
    }

    // spawn a loot item for the player to collect
    // dependent upon a random number
    private void DropLoot() {

        if(Loot == null) return;

        if (Random.Range(0, 101) >= lootGenThreshold) {
            Instantiate(Loot, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    public int GetMaxHealth(){
        return maxHealth;
    }
}