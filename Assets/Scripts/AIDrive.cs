using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class AIDrive : MonoBehaviour
{
    public GameObject muzzleFlash;
    public GameObject player;
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
    public float turretRotateSpd=25.0f;
    public float circlingMaxDis=15.0f;
    public float circlingMinDis=20.0f;


    void Start()
    {
        turret.rotation=Quaternion.Euler(-30,0,0);
        bulletSpeed=shell.GetComponent<Shell>().speed;
    }

    void LateUpdate(){
        if(canShoot){
            Shoot();
        }
    }
    void Update()
    {
        // check if the bullet can reach the player or not
        MaintainDistance();
        RotateTurret();
    }

    public IEnumerator BeginCooldown()
    {
        yield return new WaitForSeconds(shotTimer);
        canShoot=true;
    }

    void Shoot()
    {
        if(canShoot)
        {
            Instantiate(shell,barrel.position,barrel.rotation);
            GameObject flash=Instantiate(muzzleFlash,barrel.position,barrel.rotation);
            Destroy(flash,0.2f);
            canShoot=false;
            StartCoroutine(BeginCooldown());
        }
    }

    float? CalculateAngle()
    {
        Vector3 targetDir=player.transform.position-transform.position;
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
            if(lowAngle<maxTurretRot){
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
        //rotate the turret horizontally
        Vector3 direction=player.transform.position-transform.position;
        Quaternion lookRotation=Quaternion.LookRotation(direction)*Quaternion.Euler(turret.eulerAngles.x,0,0);
        turret.rotation=Quaternion.RotateTowards(turret.rotation, lookRotation, turretRotateSpd*Time.deltaTime);

        //vertically rotate the turret
        float? angle=CalculateAngle();
        if(angle!=null){
            turret.rotation=Quaternion.Euler(360-(float)angle,turret.eulerAngles.y,0);
        }
    }

    void MaintainDistance(){
        Vector3 playerDirection=player.transform.position-transform.position;
        Quaternion targetRotation=transform.rotation;
        
        if(playerDirection.magnitude>circlingMaxDis){
            //case to rotate towards the player if circlong max distance is exceeded
            targetRotation=Quaternion.LookRotation(playerDirection);
        } else if(playerDirection.magnitude<circlingMinDis){
            //case to rotate away the player if the player gets too close
            targetRotation=Quaternion.LookRotation(-playerDirection);
        } else{
            //case to circle the player if the distance is in the preferable range
            Vector3 leftCross=Vector3.Cross(playerDirection, Vector3.up);
            float crossAngle=Vector3.Angle(leftCross, transform.forward);
            if(crossAngle<=90)
            {
                targetRotation= Quaternion.LookRotation(leftCross);
            } else
            {
                targetRotation = Quaternion.LookRotation(-leftCross);
            }
        }
        transform.rotation=Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed*Time.deltaTime);
        transform.Translate(Vector3.forward*Time.deltaTime*speed);
    } 
}

