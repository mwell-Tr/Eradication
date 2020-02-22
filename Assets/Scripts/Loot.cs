using UnityEngine;

// Class that manages the collectibles for the player that comes from fallen enemies

public enum LootType {none, LargeHealth, Speed}

public class Loot : MonoBehaviour {

    public LootType type;

    private GameObject target;
    private AudioSource audioSource;
    private MeshCollider meshCollider;
    private MeshRenderer meshRenderer;

    private void Start() {

        audioSource = GetComponent<AudioSource>();
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ProcessEffect(GameObject target){

        this.target = target;

        PlaySoundEffect();

        meshCollider.enabled = false;
        meshRenderer.enabled = false;        

        switch (type) {
            case LootType.none: Debug.LogWarning("Default Loot Type");
                break;
            case LootType.LargeHealth: GiveLargeHealth();
                break;
            case LootType.Speed:GiveSpeedBuff();
                break;
            default: Debug.LogError("Invalid Loot Type");
                break;
        }

        Destroy(gameObject, 1.3f);
    }

    private void GiveLargeHealth() {
        target.GetComponent<Damageable>().RecieveHealth(15);
    }

    private void GiveSpeedBuff(){ 
        target.GetComponent<PlayerMovement>().StartCoroutine("ModifyMoveSpeed");
    }

    private void PlaySoundEffect(){
        audioSource.Play();
    }
}