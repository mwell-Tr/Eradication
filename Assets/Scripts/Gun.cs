using UnityEngine;

// Class/component that allows the player to attack another object

public class Gun : MonoBehaviour {

    public Transform barrelPoint;
    public GameObject projectile;
    public GunData data;
    
    private  Vector3 targetPoint;
    private Camera mainCamera;
    private RaycastHit hitInfo;
    private GameObject newGO;    
    private bool canShoot;
    private float timeSinceLastBullet;

    private void Start() {
        mainCamera = Camera.main;
        canShoot = true;
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
        newGO.transform.LookAt(targetPoint);
    }

    public void pullTrigger(){
        // setup stuff
        ShootWeapon();
    }

    public void releaseTrigger(){
        // cleanup stuff
        canShoot = true;
    }
}