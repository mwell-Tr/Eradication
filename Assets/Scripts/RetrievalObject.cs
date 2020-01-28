using UnityEngine;

// class is respomsible for the one time item that the player can pick up

public class RetrievalObject : MonoBehaviour{

    public delegate void ObjectPickedUp();
    public static event ObjectPickedUp objectPickedUp;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) { 

            if(objectPickedUp != null) objectPickedUp();

            // update spawners
            // update ui            
            
            Destroy(gameObject);
        }
    }
}