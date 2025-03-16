using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.UI;

public class AIDrive : MonoBehaviour
{
    private int health=100;
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
    private Transform tankTransform;
    public GameObject healthBarContainer;
    public Slider healthBar;
    private Coroutine renderHealthBar;
    void Awake()
    {
        turret.rotation=Quaternion.Euler(-30,0,0);
        bulletSpeed=shell.GetComponent<Shell>().speed;
        tankTransform=gameObject.GetComponent<Transform>();
    }

    void Start(){
    }

    void Update()
    {
        // check if the bullet can reach the player or not
        MaintainDistance();
        RotateTurret();
        if(canShoot){
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
        //Instantiate(shell,barrel.position,barrel.rotation);  BACKUP
        GameObject flash=Instantiate(muzzleFlash,barrel.position,barrel.rotation);

        //Get shell object from Object Pooler
        GameObject shell = GameManager.manager.GetObjectFromPool();
        shell.transform.position=barrel.position;
        shell.transform.rotation=barrel.rotation;
        shell.GetComponent<Rigidbody>().velocity=shell.transform.forward*bulletSpeed;
        shell.GetComponent<Rigidbody>().angularVelocity=Vector3.zero; 

        Destroy(flash,0.2f);
        canShoot=false;
        shell.SetActive(true);
        StartCoroutine(BeginCooldown());
    }

    float? CalculateAngle()
    {
        Vector3 targetDir=player.transform.position-tankTransform.position;
        float y=targetDir.y-1;
        targetDir.y=0;
        float x=targetDir.magnitude;
        float gravity=9.8f;
        float speedSqr=bulletSpeed*bulletSpeed;
        float underTheSqrRoot=(speedSqr*speedSqr)-gravity*(gravity*x*x+2*y*speedSqr);
        if(underTheSqrRoot>=0f)
        {
            float root=Mathf.Sqrt(underTheSqrRoot);
            float lowAngle=Mathf.Atan2(speedSqr-root,gravity*x)*Mathf.Rad2Deg;
            if(lowAngle<maxTurretRot || lowAngle>-5){
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
        Vector3 direction=player.transform.position-tankTransform.position;
        direction.y=direction.magnitude*Mathf.Tan((360-turret.eulerAngles.x)*Mathf.Deg2Rad);
        Quaternion lookRotation=Quaternion.LookRotation(direction);
        turret.rotation=Quaternion.RotateTowards(turret.rotation, lookRotation, turretRotateSpd*Time.deltaTime);

        //vertically rotate the turret
        float? angle=CalculateAngle();
        if(angle!=null){
            turret.rotation=Quaternion.Euler(360-(float)angle,turret.eulerAngles.y,0);
        }
    }

    void MaintainDistance(){
        Vector3 playerDirection=player.transform.position-tankTransform.position;
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
            float crossAngle=Vector3.Angle(leftCross, tankTransform.forward);
            if(crossAngle<=90)
            {
                targetRotation= Quaternion.LookRotation(leftCross);
            } else
            {
                targetRotation = Quaternion.LookRotation(-leftCross);
            }
        }
        tankTransform.rotation=Quaternion.RotateTowards(tankTransform.rotation, targetRotation, rotationSpeed*Time.deltaTime);
        tankTransform.Translate(Vector3.forward*Time.deltaTime*speed);
    } 

    private void OnCollisionEnter(Collision collision){
        if(collision.collider.CompareTag("ATProjectile")){
            //reset health bar UI render countdown
            if(renderHealthBar!=null){
                StopCoroutine(renderHealthBar);
                renderHealthBar=null;
            }
            
            health-=25;
            healthBar.value=(float)health/100;
            if(health==0){
                gameObject.SetActive(false);
                return;
            }

            //Render health bar and restart render timer
            renderHealthBar=StartCoroutine(RenderHealthBarTimed());
        }
    }

    //---------------------------
    //HEALTHBAR UI IMPLEMENTATION

    private IEnumerator RenderHealthBarTimed(){
        healthBarContainer.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        healthBarContainer.SetActive(false);
        renderHealthBar=null;
    }

    //HEALTHBAR UI IMPLEMENTATION
    //---------------------------
}


