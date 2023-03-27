using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RustEffect : MonoBehaviour
{
    public GameObject RustParticles;
    // Start is called before the first frame update
    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("File"))
        {
            ExpelRust();
        }
    }

    void ExpelRust()
    {
        GameObject Rust = Instantiate(RustParticles, transform.position, Quaternion.identity);
        Rust.GetComponent<ParticleSystem>().Play();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
