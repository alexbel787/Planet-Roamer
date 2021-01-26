using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{
    private GameManagerScript GMS;
    private Transform center;

    private void Awake()
    {
        GMS = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    }

    private void Start()
    {
        center = new GameObject().transform;
        center.parent = GMS.planet.transform;
        center.position = Vector3.zero;
        transform.parent = center;
    }

    private void Update()
    {
        if (!GMS.touch)
        {
            float s = 6 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, GMS.player.transform.position + (GMS.player.transform.position - GMS.planet.transform.position) * 2.5f, s);
            transform.LookAt(GMS.planet.transform.position + new Vector3(0, -1, 0));
        }
        else
        {
            center.position = GMS.planet.transform.position;
            if (Input.GetMouseButton(0))
                center.Rotate(-Input.GetAxis("Mouse Y") * .7f, Input.GetAxis("Mouse X") * .7f, 0.0f);
        }
    }
}
