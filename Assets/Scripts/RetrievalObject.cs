﻿using UnityEngine;

public class RetrievalObject : MonoBehaviour{

    public delegate void ObjectPickedUp();
    public static event ObjectPickedUp objectPickedUp;


    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) { 
            Debug.Log("Object acquired");

            if(objectPickedUp != null) objectPickedUp();

            // update player variable
            // update spawners here
            // update ui            
            
            Destroy(gameObject);
        }
    }
}