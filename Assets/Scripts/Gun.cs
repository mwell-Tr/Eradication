using UnityEngine;
using System.Collections;

// Class/component that allows the player to attack another object

public class Gun : MonoBehaviour {

    public delegate void ReloadingStart();
    public delegate void ReloadingEnd();
    public delegate void AmmoChanged(int AmmoDisplayValue);
    public static event ReloadingStart reloadingStart;
    public static event ReloadingEnd reloadingEnd;
    public static event AmmoChanged ammoChanged;

    public Transform barrelPoint;
    public GameObject projectile;
    public GunData data;   
    
    private bool canShoot;
    private bool isReloading;
    private float timeSinceLastBullet;
    private int currentBullets;

    private Vector3 targetPoint;
    private Camera mainCamera;
    private RaycastHit hitInfo;
    private GameObject newProjectile;
    private AudioSource audioSource;

    private void Start() {
        mainCamera = Camera.main;
        canShoot = true;
        currentBullets = data.AmmoCapacity;

        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {        
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hitInfo, 1000)){
            targetPoint = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, hitInfo.distance));
        }else {
            targetPoint = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1000));
        }

        // transform.LookAt(targetPoint);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(targetPoint, 1);
    }

    private void ShootWeapon() {

        if(canShoot) SpawnProjectile();
        canShoot = false;

        if(data.mode == FireMode.Single){

        }else if(data.mode == FireMode.Auto){            
            
            timeSinceLastBullet += Time.deltaTime;
            if(timeSinceLastBullet > data.FireRate){
                canShoot = true;
                timeSinceLastBullet = 0;
            }
        }   
    }

    private void SpawnProjectile(){
        newProjectile = Instantiate(projectile, barrelPoint.transform.position, Quaternion.identity);
        newProjectile.GetComponent<Projectile>().setGunData(data);
        newProjectile.GetComponent<Projectile>().setOwner(gameObject);
        newProjectile.transform.LookAt(targetPoint);
        newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * data.TravelSpeed, ForceMode.Impulse);        
        currentBullets -= 1;
        if(ammoChanged != null) ammoChanged(currentBullets);
        audioSource.clip = data.FireSound;
        audioSource.volume = 0.25f;
        audioSource.Play();        
    }

    public void PullTrigger(){
        // setup stuff

        // need to find an efficient way to only go through this check only once for every trigger pull
        if(currentBullets < 1){
            audioSource.clip = data.DryFireSound;
            audioSource.volume = 1.0f;
            audioSource.Play();
            canShoot = false;
            return;
        } 
        if(isReloading) return;
        ShootWeapon();
    }

    public void Reload(){

        if (currentBullets >= data.AmmoCapacity) return;
        StartCoroutine(Reloading());
    }

    private IEnumerator Reloading(){
        if(reloadingStart != null ) reloadingStart();
        isReloading = true;
        yield return new WaitForSeconds(data.ReloadTime);
        currentBullets = data.AmmoCapacity;
        isReloading = false;
        if(ammoChanged != null) ammoChanged(currentBullets);        
        if(reloadingEnd != null ) reloadingEnd();
    }

    public void ReleaseTrigger(){
        // cleanup stuff
        canShoot = true;
    }
}