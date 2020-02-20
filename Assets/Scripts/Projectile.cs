using UnityEngine;

// Core class for the objects that will be used by the player to attack enemies

public class Projectile : MonoBehaviour {

    private GunData data;
    private GameObject owner;

    public void Start() {        
        Destroy(gameObject, 5.0f);
    }
    
    public void OnCollisionEnter(Collision collision) {        

        if (collision.gameObject.GetComponent<Damageable>() != null) {
            if (collision.gameObject != owner){
                collision.gameObject.GetComponent<Damageable>().TakeDamage(data.Damage);
                Destroy(gameObject);
            }            
        }
    }

    public void setGunData(GunData newData){ 
        data = newData;
    }

    public void setOwner(GameObject newOwner){ 
        owner = newOwner;
    }
}