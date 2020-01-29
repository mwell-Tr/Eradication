using UnityEngine;

// unique area of the map that will check if the player has completed an objective or club house

public class SafeArea : MonoBehaviour{

    public delegate void MissionComplete();
    public static event MissionComplete missionComplete;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {            

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
