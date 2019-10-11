using UnityEngine;

// Core class for the objects that will be used by the player to attack enemies

public class Projectile : MonoBehaviour {

    public GunData data;

    public void Start() {

        GetComponent<Rigidbody>().AddForce(transform.forward * data.TravelSpeed, ForceMode.Force);
    }
    public void OnCollisionEnter(Collision collision) {

        Debug.Log("Bullet Collision with " + collision.gameObject.name);

        if (collision.gameObject.GetComponent<Damageable>() != null) {
            collision.gameObject.GetComponent<Damageable>().TakeDamage(data.Damage);
            Destroy(gameObject);
        }
    }
}