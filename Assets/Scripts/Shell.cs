using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class Shell : MonoBehaviour
{
    public float speed=40;
    public Rigidbody rb;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        rb=gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame

    void OnCollisionEnter(Collision collision) 
    {
        if(collision.gameObject.tag=="Tank") Destroy(collision.gameObject);
        GameObject shockwave=Instantiate(explosion, gameObject.transform.position, transform.rotation);
        Destroy(shockwave, 0.5f);
        //SHELL DEACTIVATION CODE

        ObjectPooler.poolerInstance.ReleaseObject(gameObject);

        //SHELL DEACTIVATION  CODE
    }
    void Update()
    {
        this.transform.forward=rb.velocity.normalized;
    }
}
