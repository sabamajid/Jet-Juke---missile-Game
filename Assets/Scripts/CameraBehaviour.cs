using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraBehaviour : MonoBehaviour
{
    public Transform target;
    public GameObject canvas;
    public GameObject missilePrefab;
    public GameObject[] cloudPrefab;
    public float speed = 5f;
    public bool gameOver;

    Vector3 offSet;
    List<GameObject> spawnedClouds = new List<GameObject>();

    Camera mainCamera;
    float cameraWidth, cameraHeight;

    // Use this for initialization
    void Start()
    {
        offSet = target.position - transform.position;
        mainCamera = Camera.main;

        // Calculate the size of the camera view
        cameraHeight = 2f * mainCamera.orthographicSize;
        cameraWidth = cameraHeight * mainCamera.aspect;

        StartCoroutine(startAnimation());
    }

    void LateUpdate()
    {
        if (!gameOver)
            transform.position = Vector3.Lerp(transform.position, target.position - offSet, speed * Time.deltaTime);
    }

    IEnumerator startAnimation()
    {
        makeClouds();
        yield return new WaitForSeconds(2f);
        canvas.SetActive(false);
        StartCoroutine(missileSpawner());
        StartCoroutine(cloudManager());
    }

    IEnumerator missileSpawner()
    {
        while (!gameOver)
        {
            int j = 0;
            int i = 0;
            if (target.rotation.z < 180)
            {
                i = 10;
                j = 8;
            }
            else
            {
                i = -10;
                j = -8;
            }
            Vector3 spawnPosition = target.position + new Vector3(Random.Range(j, i), Random.Range(j, i), 0f);
            GameObject missileTemp = Instantiate(missilePrefab, spawnPosition, missilePrefab.transform.rotation);
            missileTemp.GetComponent<MissileBehaviour>().target = target;
            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }

    void makeClouds()
    {
        // Spawn clouds only outside the current camera view
        float spawnBuffer = 20f; // Spawn clouds just outside visible area
        float minX = transform.position.x - cameraWidth / 2 - spawnBuffer;
        float maxX = transform.position.x + cameraWidth / 2 + spawnBuffer;
        float minY = transform.position.y - cameraHeight / 2 - spawnBuffer;
        float maxY = transform.position.y + cameraHeight / 2 + spawnBuffer;

        for (float i = minX; i < maxX; i += Random.Range(10, 15))
        {
            for (float j = minY; j < maxY; j += Random.Range(10, 15))
            {
                Vector3 spawnPosition = new Vector3(i, j, 0f);

                // Only spawn if no clouds already exist in that area
                if (!CloudExistsNear(spawnPosition, 5f))
                {
                    GameObject cloud = Instantiate(cloudPrefab[Random.Range(0, cloudPrefab.Length)], spawnPosition, Quaternion.identity);
                    spawnedClouds.Add(cloud);
                }
            }
        }
    }

    bool CloudExistsNear(Vector3 position, float distance)
    {
        foreach (GameObject cloud in spawnedClouds)
        {
            if (Vector3.Distance(cloud.transform.position, position) < distance)
                return true;
        }
        return false;
    }

    IEnumerator cloudManager()
    {
        while (!gameOver)
        {
            // Check and remove clouds that are far outside the camera view
            for (int i = spawnedClouds.Count - 1; i >= 0; i--)
            {
                if (IsFarOutsideCameraView(spawnedClouds[i].transform.position))
                {
                    Destroy(spawnedClouds[i]);
                    spawnedClouds.RemoveAt(i);
                }
            }

            // Spawn new clouds just outside the current camera view
            makeClouds();

            yield return new WaitForSeconds(2f); // Check every 2 seconds
        }
    }

    bool IsFarOutsideCameraView(Vector3 position)
    {
        float buffer = 10f; // Small buffer just outside visible area
        float minX = transform.position.x - cameraWidth / 2 - buffer;
        float maxX = transform.position.x + cameraWidth / 2 + buffer;
        float minY = transform.position.y - cameraHeight / 2 - buffer;
        float maxY = transform.position.y + cameraHeight / 2 + buffer;

        return position.x < minX || position.x > maxX || position.y < minY || position.y > maxY;
    }

    public void restart()
    {
        SceneManager.LoadScene("Main");
    }
}
