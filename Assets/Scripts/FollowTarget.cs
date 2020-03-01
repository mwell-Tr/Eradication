using UnityEngine;

public class FollowTarget : MonoBehaviour{

    public Transform target;
    public Vector3 offset;
    public float moveSpeed;
    private Vector3 targetPosition;

    private void Update(){

        // Define a target position above and behind the target transform
        targetPosition = target.TransformPoint(offset);

        // Smoothly move the camera towards that target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

    }
}
