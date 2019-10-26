using UnityEngine;

// Component that initiates the damage process when the enemy does damage

public class EnemyAttack : MonoBehaviour{

    public GunData damageData;

    public void OnTriggerEnter(Collider collider) {        

        if (collider.gameObject.GetComponent<Damageable>() != null) {
            collider.gameObject.GetComponent<Damageable>().TakeDamage(damageData.Damage);
        }
    }
}
