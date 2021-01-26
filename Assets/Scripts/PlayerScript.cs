using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //public float speed;

    private PlanetScript PScript;
    private Rigidbody rb;
    private FixedJoystick fixedJoystick;
    private GameManagerScript GMS;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PScript = GameObject.Find("Enviroment/Planet").GetComponent<PlanetScript>();
        GMS = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        fixedJoystick = GameObject.Find("Canvas/FixedJoystick").GetComponent<FixedJoystick>();
    }

    public void Update()
    {
        if (!GMS.disableInput)
        {
            if (Mathf.Abs(fixedJoystick.Vertical) > 0 || Mathf.Abs(fixedJoystick.Horizontal) > 0)
            {
                float speed = 100 * Time.deltaTime;
                if (fixedJoystick.Horizontal > Mathf.Abs(fixedJoystick.Vertical))
                    transform.Rotate(Vector3.up, speed, Space.Self);
                else if (fixedJoystick.Horizontal < 0 && Mathf.Abs(fixedJoystick.Horizontal) > Mathf.Abs(fixedJoystick.Vertical))
                    transform.Rotate(-Vector3.up, speed, Space.Self);
                else if (fixedJoystick.Vertical > Mathf.Abs(fixedJoystick.Horizontal))
                    rb.velocity = transform.forward * speed;
                else if (fixedJoystick.Vertical < 0 && Mathf.Abs(fixedJoystick.Vertical) > Mathf.Abs(fixedJoystick.Horizontal))
                    rb.velocity = transform.forward * -speed;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Food")
        {
            GMS.foodEaten++;
            GMS.scoreText.text = GMS.foodEaten.ToString();
            GMS.foodList.Remove(other.gameObject);
            GMS.allObjectsList.Remove(other.gameObject);
            Destroy(other.gameObject);
        }
        else if (other.collider.tag == "Mine")
        {
            StartCoroutine(GMS.GameOverCoroutine());
        }
    }

    private void FixedUpdate()
    {
        PScript.AddGravitation(gameObject);
        GMS.RotateToPlanet(gameObject.transform);
    }
}
