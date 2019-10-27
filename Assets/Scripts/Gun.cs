using UnityEngine;
using System.Collections;

// Class/component that allows the player to attack another object

public class Gun : MonoBehaviour {

    public delegate void ReloadingStart();
    public delegate void ReloadingEnd();
    public delegate void AmmoChanged(int AmmoDisplayValue);
    public event ReloadingStart reloadingStart;
    public event ReloadingEnd reloadingEnd;
    public event AmmoChanged ammoChanged;

    public Transform barrelPoint;
    public GameObject projectile;
    public GunData data;
    
    private  Vector3 targetPoint;
    private Camera mainCamera;
    private RaycastHit hitInfo;
    private GameObject newGO;    
    private AudioSource audioSource;
    private bool canShoot;
    private bool isReloading;
    private float timeSinceLastBullet;
    private int currentBullets;

    private void Start() {
        mainCamera = Camera.main;
        canShoot = true;
        currentBullets = data.AmmoCapacity;

        audioSource = GetComponent<AudioSource>();        
        audioSource.clip = data.FireSound;
    }

    private void Update() {        
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hitInfo, 1000)){
            targetPoint = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, hitInfo.distance));
        }else {
            targetPoint = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1000));
        }
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
        newGO = Instantiate(projectile, barrelPoint.transform.position, Quaternion.identity);
        currentBullets -= 1;
        if(ammoChanged != null) ammoChanged(currentBullets);        
        audioSource.Play();
        newGO.transform.LookAt(targetPoint);
    }

    public void PullTrigger(){
        // setup stuff

        if(currentBullets < 1) return;
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