using UnityEngine;

// Class/component that allows an object to attack another 
// rename variable p

public class Gun : MonoBehaviour {

    public Transform barrelPoint;
    public GameObject projectile;
    public GunData data;
    public Vector3 p;

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
            p = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, hitInfo.distance));
        }else {
            p = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1000));
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(p, 1);
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

    public void ShootStinger(Vector3 targetPosition) {

        newGO = Instantiate(projectile, barrelPoint.position, Quaternion.identity);
        newGO.transform.LookAt(targetPosition);
    }

    private void SpawnProjectile(){
        newGO = Instantiate(projectile, barrelPoint.transform.position, Quaternion.identity);
        newGO.transform.LookAt(p);
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

// player "pulls" trigger
// bullet comes out

// if single
    // nothing
// if auto fire
    // set ready to shoot to true
    // wait fore a delay in turns of fire rate
    // loop back until player stops input/ammo/dead