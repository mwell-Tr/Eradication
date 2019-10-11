using System.Collections;
using UnityEngine;

// Component that spawns prefabs based on quantity, frequency, and location. 

public class Spawner : MonoBehaviour {

    public GameObject spawnee;
    public bool spawning;
    public float waveInterval;
    public float spawnDelay;
    public Transform[] spawnPoints;

    [Range(1, 4)]
    public int spawnCount;

    private int spawnIndex;

    public void Start() {

        StartCoroutine(SpawnEnemies());
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            spawning = false;
        }
    }

    // optimizatino consideration
    // instead of leaving the script and game object active find a way to leave the spawner always spawning and just control the spawner by 
    // setting the gameobject itself to active or not active
    // right now if it was in a update to check if spawning is true it would check that every frame where as if it always spawned no matter what it would actually
    // do absolutley nothing if the gameobject was disabled all together

    private IEnumerator SpawnEnemies() {

        while (spawning == true) {

            spawnIndex = Random.Range(0, spawnPoints.Length - 1);

            for (int i = 0; i < spawnCount; i++) {

                Instantiate(spawnee, spawnPoints[spawnIndex].position, Quaternion.identity);
                yield return new WaitForSecondsRealtime(spawnDelay);
            }

            yield return new WaitForSecondsRealtime(waveInterval);
        }
    }
}