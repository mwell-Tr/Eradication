using UnityEngine;

// Responsible for the player controls and input

public class PlayerMovement : MonoBehaviour {

    public delegate void PlayerDied();
    public event PlayerDied playerDied;

    public float normalMoveSpeed;
    public float strafeMoveSpeed;
    public float jumpForce;

    private float verticalInput;
    private float horizontalInput;
    private float mouseXInput;
    private float mouseYInput;
    private float jumpInput;
    private float gravity;
    private float currentDeltaTime;
    private float jumpVelocity;    
    private bool shootGun;
    private bool allowedToMove;

    private Gun gun;
    private Camera mainCamera;
    private CharacterController cc;
    private Vector3 directionToMove;

    private void Start() {

        cc = gameObject.GetComponent<CharacterController>();
        gun = gameObject.GetComponent<Gun>();
        mainCamera = Camera.main;
        gravity = 200.0f;
        jumpVelocity = 0f;
        allowedToMove = true;
    }

    private void Update() {

        UpdateInputValues();
        UpdateRotation();
        if(allowedToMove) MovePlayer();

        if (Input.GetKey(KeyCode.LeftControl)) gun.pullTrigger();
        if (Input.GetKeyUp(KeyCode.LeftControl)) gun.releaseTrigger();
    }

    private void UpdateInputValues() {

        currentDeltaTime = Time.deltaTime;

        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        mouseXInput = Input.GetAxis("Mouse X");
        mouseYInput = Input.GetAxis("Mouse Y");
        jumpInput = Input.GetAxis("Jump");
    }

    private void UpdateRotation() {
        transform.Rotate(0, mouseXInput, 0);

        mainCamera.transform.Rotate(-mouseYInput, 0, 0);
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

    private void OnControllerColliderHit(ControllerColliderHit hit) {

        if(hit.gameObject.CompareTag("Loot")){
            hit.gameObject.GetComponent<Loot>().ProcessEffect(this);
            Destroy(hit.gameObject);
        }
    }

    private void StartDeath(){        
        if (playerDied != null) playerDied();        
        allowedToMove = false;
    }
}