using UnityEngine;

public class TestSound : MonoBehaviour{
    
    private AudioSource audioSource;

    private void Start(){
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.Y)) audioSource.Play();
    }
}