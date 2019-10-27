using UnityEngine;

// Scriptable object that allows for useful information to be 
// predetermined and easily passed from object to object when needed

public enum FireMode{Single, Auto};

[CreateAssetMenu(fileName = "GunData", menuName = "Gun Data Asset")]
public class GunData : ScriptableObject {

    public FireMode mode;
    public int Damage;
    public int TravelSpeed;
    public int AmmoCapacity;
    public float ReloadTime;
    public float FireRate;
    public AudioClip FireSound;
}