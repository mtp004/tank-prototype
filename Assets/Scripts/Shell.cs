using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class Shell : MonoBehaviour
{
    public float speed=2;
    public Rigidbody rb;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        rb=gameObject.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward*speed,ForceMode.Impulse);
    }

    // Update is called once per frame

    void OnCollisionEnter(Collision collision) 
    {
        GameObject shockwave=Instantiate(explosion, gameObject.transform.position, transform.rotation);
        Destroy(shockwave, 0.5f);
        Destroy(gameObject);
    }
    void Update()
    {
        this.transform.forward=rb.velocity.normalized;
    }
}
