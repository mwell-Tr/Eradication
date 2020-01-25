using UnityEngine;

public class SafeArea : MonoBehaviour{

    public delegate void MissionComplete();
    public static event MissionComplete missionComplete;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {

            Debug.Log("Safe Area");

            if (other.GetComponent<PlayerMovement>().hasPlayerAcquiredRetrievalObject() == true){

                if(missionComplete != null) missionComplete();

                Debug.Log("You did it without dying");
                // player succefully returned object
                // display win ui
                // end mission
            }
        }
    }
}
