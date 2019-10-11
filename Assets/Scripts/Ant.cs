using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// Need to remain to something more fitting
// Class for the primary enemy 
// Handle navigation commands, animation commands, and attacking the player
// Adding a pause/stop wandering feature could be nice for debugging and testing

public class Ant : MonoBehaviour {

    public GameObject target;
    public float maxAttackDistance;
    public float maxChaseDistnace;
    public float attackRate;

    private Gun gun;
    private NavMeshAgent agent;
    private NavMeshHit navMeshHit;
    private Vector3 desriedWanderTargetPoint;
    private Vector3 actualWanderTargetPoint;
    private Animator animator;
    private float distanceFromPlayer;
    private float wanderDistance;
    private bool isChasing;

    private void Start() {

        gun = GetComponent<Gun>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        wanderDistance = UnityEngine.Random.Range(25.0f, 75.0f);
        distanceFromPlayer = 0;
        isChasing = false;
        
        target = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(PursueTarget());
    }

    private IEnumerator PursueTarget() {

        while (true) {

            if (target != null) {
                // has a valid target
                distanceFromPlayer = Vector3.Distance(transform.position, target.transform.position);

                if (distanceFromPlayer < maxChaseDistnace) {
                    // within chasing radius
                    isChasing = true;
                    animator.SetBool("Idle", false);
                    animator.SetBool("Chasing", isChasing);
                } else {
                    // not within chasing radius
                    isChasing = false;
                    animator.SetBool("Chasing", isChasing);
                }
            } else {
                // does not have a valid target
                isChasing = false;
                animator.SetBool("Chasing", isChasing);
            }

            if (isChasing == true) {
                // chasing the target
                agent.SetDestination(target.transform.position);

                if (distanceFromPlayer < maxAttackDistance) {
                    // within attack radius

                    // gun.ShootStinger(target.transform.position);
                    animator.SetBool("Attacking", true);

                    // becasue of the waiting, shooting causes a "delayed" update when it comes to chasing the player
                    // might leave that in, will reconsider once balancing is relevant 
                    yield return new WaitForSeconds(attackRate);

                    animator.SetBool("Attacking", false);
                    animator.SetBool("Chasing", true);
                }

                yield return new WaitForEndOfFrame();

            } else if (isChasing == false) {
                // wander to a random point
                desriedWanderTargetPoint = (UnityEngine.Random.insideUnitSphere * wanderDistance) + transform.position;
                NavMesh.SamplePosition(desriedWanderTargetPoint, out navMeshHit, wanderDistance, -1);
                animator.SetBool("Idle", false);
                agent.SetDestination(navMeshHit.position);

                // would like to find alternatives to updating Vector3.Distance and maintaing the same functionality
                yield return new WaitUntil(() => agent.remainingDistance < 0.5f || Vector3.Distance(transform.position, target.transform.position) < maxChaseDistnace);
            }
        }
    }
}