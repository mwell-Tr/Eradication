using UnityEngine;

public class SafeArea : MonoBehaviour{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {

            Debug.Log("Safe Area");

            if (other.GetComponent<PlayerMovement>().hasPlayerAcquiredRetrievalObject() == true){
                Debug.Log("You did it without dying");
                // player succefully returned object
                // display win ui
                // end mission
            }
        }
    }
}
