using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shell : MonoBehaviour
{
    public float speed=40;
    public Rigidbody rb;
    public GameObject explosion;
    public Transform shellTransform;

    // Start is called before the first frame update
    void Awake()
    {
        rb=gameObject.GetComponent<Rigidbody>();
        shellTransform=gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame

    void OnCollisionEnter(Collision collision) 
    {
        GameObject shockwave=Instantiate(explosion, shellTransform.position, shellTransform.rotation);
        Destroy(shockwave, 0.5f);
        //SHELL DEACTIVATION CODE

        GameManager.manager.ReleaseObject(gameObject);

        //SHELL DEACTIVATION  CODE
    }
    void Update()
    {
        shellTransform.forward=rb.velocity.normalized;
    }
}
