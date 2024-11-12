using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Drive : MonoBehaviour
{
    public GameObject muzzleFlash;
    public GameObject pointer;
    public Transform barrel;
    public bool canShoot=true;
    public GameObject shell;
    public float shotTimer;
    public Transform turret;
    public float maxTurretRot=32;
    //store the value of the x-axis rot of turret in a variable to allow angles to go negative
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public float bulletSpeed;
    public float turretRotateSpd=30.0f;

    void Start()
    {
        bulletSpeed=shell.GetComponent<Shell>().speed;
    }
    void Update()
    {
        RotateTurret();
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        // Make it move 10 meters per second instead of 10 meters per frame...
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(0, 0, translation);

        // Rotate around our y-axis
        transform.Rotate(0, rotation, 0);
         if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    public IEnumerator BeginCooldown()
    {
        yield return new WaitForSeconds(shotTimer);
        canShoot=true;
    }

    void Shoot()
    {
        if(canShoot){
            GameObject flash=Instantiate(muzzleFlash,barrel.position,barrel.rotation);
            //GET BULLET FROM OBPOOL CODE
            GameObject shell = ObjectPooler.poolerInstance.GetObjectFromPool();
            shell.transform.position=barrel.position;
            shell.transform.rotation=barrel.rotation;
            shell.GetComponent<Rigidbody>().velocity=shell.transform.forward*bulletSpeed;
            shell.GetComponent<Rigidbody>().angularVelocity=Vector3.zero;
            shell.SetActive(true); 
            //GET BULLET FROM OBPOOL CODE   
            Destroy(flash,0.2f);
            canShoot=false;
            StartCoroutine(BeginCooldown());
        }
    }

    float? CalculateAngle()
    {
        Vector3 targetDir=pointer.transform.position-transform.position;
        float y=targetDir.y-1;
        targetDir.y=0f;
        float x=targetDir.magnitude;
        float gravity=9.8f;
        float speedSqr=bulletSpeed*bulletSpeed;
        float underTheSqrRoot=(speedSqr*speedSqr)-gravity*(gravity*x*x+2*y*speedSqr);
        
        if(underTheSqrRoot>=0f)
        {
            float root=Mathf.Sqrt(underTheSqrRoot);
            float lowAngle=Mathf.Atan2(speedSqr-root,gravity*x)*Mathf.Rad2Deg;
            if(lowAngle<maxTurretRot && lowAngle>-5){
                return lowAngle;
            } else{
                return null;
            }
        } else{
            return null;
        }
    }

    void RotateTurret()
    {
        Vector3 direction=pointer.transform.position-transform.position;
        direction.y=direction.magnitude*Mathf.Tan(-turret.eulerAngles.x*Mathf.Deg2Rad);
        Quaternion lookRotation=Quaternion.LookRotation(direction);
        turret.rotation=Quaternion.RotateTowards(turret.transform.rotation, lookRotation, Time.deltaTime*turretRotateSpd);

        //vertically rotate the barrel
        float? angle=CalculateAngle();
        if(angle!=null){
            turret.rotation=Quaternion.Euler(360-(float)angle,turret.eulerAngles.y,0);
        }
    }
}

