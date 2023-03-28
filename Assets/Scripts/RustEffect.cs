using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RustEffect : MonoBehaviour
{
    public GameObject RustParticles;
    // Start is called before the first frame update

    public void ExpelRust()
    {
        Debug.Log("Hit Rust");
        if (GameManager.Instance.hasFile == true)
        {
            GameObject Rust = Instantiate(RustParticles, transform.position, Quaternion.identity);
            Rust.GetComponent<ParticleSystem>().Play();
            this.gameObject.SetActive(false);
        }

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
