using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shell : MonoBehaviour
{
    public float speed=40;
    public Rigidbody rb;
    public GameObject explosion;
    public Transform shellTransform;
    public int hitTimes;

    // Start is called before the first frame update
    void Awake()
    {
        rb=gameObject.GetComponent<Rigidbody>();
        shellTransform=gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame

    void OnCollisionEnter(Collision collision) 
    {
        hitTimes++;
        if(hitTimes==2) Debug.Log("Shell register hit two times");
        GameManager.manager.ReleaseObject(gameObject);
        GameObject shockwave=Instantiate(explosion, shellTransform.position, shellTransform.rotation);
        Destroy(shockwave, 0.5f);
    }
    void Update()
    {
        shellTransform.forward=rb.velocity.normalized;
    }

    void OnEnable(){
        hitTimes=0;
    }
}
