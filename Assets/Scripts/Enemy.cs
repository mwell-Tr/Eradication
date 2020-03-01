using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// Class for the primary enemy 
// Handle navigation commands, animation commands, and attacking the player
// Adding a pause/stop wandering feature could be nice for debugging and testing

// maybe subscribe to player is dead event
// only realy benefit is to go back to wandering but it may
// already work witout needing an event to trigger it

public class Enemy : MonoBehaviour {    
    
    public float maxAttackDistance;
    public float maxChaseDistnace;
    public float attackRate;

    public GameObject target;
    public GameObject DamageBox;
    public GameObject[] possibleLoot;
    
    private int randomLootIndex;
    private float distanceFromPlayer;
    private float wanderDistance;
    private bool isChasing;
    private bool active;

    private static int rngBase;

    private NavMeshAgent agent;
    private NavMeshHit navMeshHit;
    private Vector3 desriedWanderTargetPoint;
    private Animator animator;
    private AudioSource audioSource;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();        
        
        randomLootIndex = Random.Range(0, possibleLoot.Length);
        wanderDistance = Random.Range(25.0f, 75.0f);
        rngBase = 72;
        distanceFromPlayer = 0;
        isChasing = false;
        active = true;       
        DamageBox.SetActive(false);    
        agent.avoidancePriority = Random.Range(0, 101);

        target = GameObject.FindGameObjectWithTag("Player");
        

        StartCoroutine("PursueTarget");
    }

    private IEnumerator PursueTarget() {
        
        while (active) {

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
                transform.LookAt(target.transform.position);
                agent.isStopped = false;

                if (distanceFromPlayer < maxAttackDistance) {
                    // within attack radius

                    agent.isStopped = true;
                    animator.SetBool("Attacking", true);

                    // important to find a balanced number 
                    // too long will cause extra animation loops to play
                    // too short will cause more damage to trigger
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
                
                if(active) yield return new WaitUntil(() => agent.remainingDistance < 10.0f || Vector3.Distance(transform.position, target.transform.position) < maxChaseDistnace);
            }
        }
    }

    public void TurnOnDamageBox(){ 
        DamageBox.SetActive(true);
    }

    public void TurnOffDamageBox() {
        DamageBox.SetActive(false);
    }

    // modify values to prepare for death animation and the destroying of the game object
    public void StartDeath(){

        if(active == true){
            StopCoroutine("PursueTarget");

            DamageBox.SetActive(false);
            active = false;            
            agent.isStopped = true;
            animator.SetBool("Alive", false);
            audioSource.Play();

            if(rngBase < Random.Range(0, 101)){
                Instantiate(possibleLoot[randomLootIndex], new Vector3(transform.position.x, transform.position.y + 15, transform.position.z), Quaternion.identity);
            }
            
            Destroy(gameObject, 1.5f);
        }
    }
}