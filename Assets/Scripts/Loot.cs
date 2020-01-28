﻿using UnityEngine;

// Class that manages the collectibles for the player that comes from fallen enemies

public enum LootType {none, SmallHealth, LargeHealth, Weapon, Armor}

public class Loot : MonoBehaviour {

    public LootType type;

    private PlayerMovement target;
    private Damageable playersCombatStats;
    private AudioSource audioSource;
    private MeshCollider meshCollider;
    private MeshRenderer meshRenderer;

    private void Start() {

        audioSource = GetComponent<AudioSource>();
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        type = (LootType) Random.Range(1, 5);
    }

    public void ProcessEffect(PlayerMovement target){
        PlaySoundEffect();

        meshCollider.enabled = false;
        meshRenderer.enabled = false;        

        this.target = target;
        playersCombatStats = target.GetComponent<Damageable>();

        switch (type) {
            case LootType.none: Debug.LogWarning("Default Loot Type");
                break;
            case LootType.SmallHealth: GiveSmallHealth();
                break;
            case LootType.LargeHealth: GiveLargeHealth();
                break;
            case LootType.Weapon: GiveWeapon(0);
                break;
            case LootType.Armor: GiveArmor();
                break;
            default: Debug.LogError("Invalid Loot Type");
                break;
        }

        Destroy(gameObject, 1.3f);
    }

    private void GiveSmallHealth(){

        playersCombatStats.RecieveHealth(playersCombatStats.GetMaxHealth() * 0.15f);
    }

    private void GiveLargeHealth() {
        playersCombatStats.RecieveHealth(playersCombatStats.GetMaxHealth() * 0.33f);
    }

    private void GiveWeapon(int level) {
        Debug.LogWarning("Give Weapon Not Implmented");
    }

    private void GiveArmor(){
        Debug.LogWarning("Give Armor Not Implmented");
    }

    private void PlaySoundEffect(){
        audioSource.Play();
    }
}