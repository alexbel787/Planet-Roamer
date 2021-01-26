using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour
{
    private GameManagerScript GMS;

    void Start()
    {
        GMS = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(4, 6f));
        GMS.mineList.Remove(gameObject);
        GMS.allObjectsList.Remove(gameObject);
        Destroy(gameObject);
    }
}
