using UnityEngine;

public class EnemyAttack : MonoBehaviour{

    public void OnTriggerEnter(Collider collider) {        

        if (collider.gameObject.GetComponent<Damageable>() != null) {
            collider.gameObject.GetComponent<Damageable>().TakeDamage(1);
        }
    }
}
