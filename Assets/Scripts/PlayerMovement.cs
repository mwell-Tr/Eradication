using UnityEngine;
using System.Collections;

// Responsible for the player controls and input
// Consider renaming or restructuring.
// class scope has grown a lot

public class PlayerMovement : MonoBehaviour {

    public delegate void PlayerDied();
    public static event PlayerDied playerDied;

    public float normalMoveSpeed;
    public float strafeMoveSpeed;
    public float rotationXSpeed;
    public float rotationYSpeed;
    public float jumpForce;

    private float moveSpeedMultiplier; 
    private float verticalInput;
    private float horizontalInput;
    private float rotationXInput;
    private float rotationYInput;
    private float jumpInput;
    private float gravity;
    private float currentDeltaTime;
    private float jumpVelocity;    
    private bool allowedToMove;
    private bool allowedToShoot;
    private bool PlayerAcquiredRetrievalObject;

    private Gun gun;
    private Camera mainCamera;
    private CharacterController cc;
    private Vector3 directionToMove;

    private void Start() {

        RetrievalObject.objectPickedUp += consumePlayerPickUp;

        cc = gameObject.GetComponent<CharacterController>();
        gun = gameObject.GetComponent<Gun>();
        mainCamera = Camera.main;
        moveSpeedMultiplier = 1.5f;
        gravity = 200.0f;
        jumpVelocity = 0f;
        allowedToMove = true;
        allowedToShoot = true;
        PlayerAcquiredRetrievalObject = false;
    }

    private void Update() {

        UpdateInputValues();
        UpdateRotation();
        if (allowedToMove) MovePlayer();

        if (allowedToShoot) {
            if (Input.GetAxis("Fire1") > 0) gun.PullTrigger();
            if (Input.GetAxis("Fire1") < 1) gun.ReleaseTrigger();
            if (Input.GetAxis("Reload") > 0) gun.Reload();
        }
    }

    private void UpdateInputValues() {

        currentDeltaTime = Time.deltaTime;

        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        rotationXInput = Input.GetAxis("Rotate X");
        rotationYInput = Input.GetAxis("Rotate Y");
        jumpInput = Input.GetAxis("Jump");
    }

    private void UpdateRotation() {

        transform.Rotate(0, (rotationXInput * Time.deltaTime) * rotationXSpeed, 0);
        mainCamera.transform.Rotate((rotationYInput * Time.deltaTime) * rotationYSpeed, 0, 0);
    }

    private void MovePlayer() {

        directionToMove = (transform.forward * normalMoveSpeed) * verticalInput;
        directionToMove += (transform.right * strafeMoveSpeed) * horizontalInput;

        if (cc.isGrounded == true) {

            jumpVelocity = -gravity * currentDeltaTime;

            if (jumpInput > 0) {
                jumpVelocity = jumpForce;
            }

        } else {
            jumpVelocity -= gravity * currentDeltaTime;
        }

        directionToMove.y = jumpVelocity;
        cc.Move(directionToMove * currentDeltaTime);
    }

    IEnumerator ModifyMoveSpeed(){ 

        normalMoveSpeed = normalMoveSpeed * moveSpeedMultiplier;
                
        yield return new WaitForSeconds(5.0f);

        normalMoveSpeed = normalMoveSpeed / moveSpeedMultiplier;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {

        if(hit.gameObject.CompareTag("Loot")){
            hit.gameObject.GetComponent<Loot>().ProcessEffect(gameObject);            
        }
    }

    private void StartDeath(){        
        if (playerDied != null) playerDied();        
        allowedToMove = false;
        allowedToShoot = false;
    }

    private void consumePlayerPickUp(){
        PlayerAcquiredRetrievalObject = true;
    }

    public bool hasPlayerAcquiredRetrievalObject(){ 
        return PlayerAcquiredRetrievalObject;
    }
}