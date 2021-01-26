using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManagerScript : MonoBehaviour
{
    public int foodEaten;
    public GameObject player;
    public GameObject planet;
    public List<GameObject> allObjectsList = new List<GameObject>();
    public int foodQuantity;
    public List<GameObject> foodList = new List<GameObject>();
    public int mineQuantity;
    public List<GameObject> mineList = new List<GameObject>();
    public GameObject[] prefabs;
    public bool disableInput;
    private bool isEnoughSpace;
    public Text scoreText;
    //private Vector2 startTouchPosition, endTouchPosition;
    public bool touch;

    private void Start()
    {
        allObjectsList.Add(player);
        for (int i = 1; i <= foodQuantity; i++) SpawnObject(prefabs[0]);
        for (int i = 1; i <= mineQuantity; i++) SpawnObject(prefabs[1]);

        if (((float)Screen.height / Screen.width) < 1.7f)
        {
            Camera.main.GetComponent<CameraConstantWidth>().WidthOrHeight = 1f;
            GameObject.Find("Canvas").GetComponent<CanvasScaler>().matchWidthOrHeight = 1f;
        }
    }

    private void Update()
    {
        if (foodList.Count < foodQuantity) SpawnObject(prefabs[0]);
        if (mineList.Count < mineQuantity) SpawnObject(prefabs[1]);

        if (!disableInput && Input.GetMouseButtonDown(0))
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    Debug.Log("Touched UI and return");
                    return;
                }
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clicked on UI and return");
                return;
            }

            touch = true;
        }

        if (Input.GetMouseButtonUp(0)) touch = false;
    }

    private void SpawnObject(GameObject prefab)
    {
        isEnoughSpace = false;
        Vector3 point = Vector3.zero;
        while (!isEnoughSpace)
        {
            point = Random.onUnitSphere * 2.65f + planet.transform.position;
            foreach (GameObject _obj in allObjectsList)
            {
                if (Vector3.Distance(point, _obj.transform.position) < 1.3f)
                {
                    isEnoughSpace = false;
                    break;
                }
                else isEnoughSpace = true;
            }
        }
        GameObject obj = Instantiate(prefab, point, Quaternion.identity);
        if (prefab.tag == "Food") foodList.Add(obj);
        else mineList.Add(obj);
        allObjectsList.Add(obj);
        obj.transform.SetParent(GameObject.Find("Enviroment").transform);
        obj.transform.LookAt(planet.transform.position);
    }

    public void RotateToPlanet(Transform t)
    {
        Vector3 gravityUp = (t.position - planet.transform.position).normalized;
        Quaternion targetRotation = Quaternion.FromToRotation(t.up, gravityUp) * t.rotation;
        t.rotation = Quaternion.Slerp(t.rotation, targetRotation, 50 * Time.deltaTime);
    }

    public IEnumerator GameOverCoroutine()
    {
        disableInput = true;
        GameObject blast = Instantiate(prefabs[2], player.transform.position, Quaternion.identity);
        float size;
        float timer = 0;
        while (timer < 1.5f)
        {
            timer += Time.deltaTime;
            size = Mathf.Lerp(.01f, 3f, timer);
            blast.transform.localScale = new Vector3(size, size, size);
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene(0);
    }

    private IEnumerator SwipePlanetCoroutine()
    {

        yield return new WaitUntil(() => !touch);

    }
}
