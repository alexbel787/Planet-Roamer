using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{
    public float gravity;

    public Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void AddGravitation(GameObject obj)
    {
        Vector3 gravityUp = (obj.transform.position - transform.position).normalized;
        obj.GetComponent<Rigidbody>().AddForce(gravityUp * gravity);

        //obj.transform.up = gravityUp;
    }
}
