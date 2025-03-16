using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shell : MonoBehaviour
{
    public float speed=40;
    public Rigidbody rb;
    public GameObject explosion;
    public Transform shellTransform;
    public bool hitRegistered=false;

    // Start is called before the first frame update
    void Awake()
    {
        rb=gameObject.GetComponent<Rigidbody>();
        shellTransform=gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame

    void OnCollisionEnter(Collision collision) 
    {
        if(!hitRegistered){
            hitRegistered=true;
            gameObject.SetActive(false);
            GameObject shockwave=Instantiate(explosion, shellTransform.position, shellTransform.rotation);
            Destroy(shockwave, 0.5f);
        } else Debug.Log("Hitted two times");
    }
    void Update()
    {
        shellTransform.forward=rb.velocity.normalized;
    }

    void OnDisable(){
        hitRegistered=false;
    }
}
