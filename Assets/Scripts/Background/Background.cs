using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    List<Transform> cloudList;
    float minYPosition = 0;
    float maxYPosition = 50;
    float CLOUD_DESTROY_X_POSITION = -100f;
    float cloudSpeed = 5f;
    void Awake()
    {
        cloudList = new List<Transform>();
    }


    void Start()
    {
        StartCoroutine(SpawnCloud());
    }
    void FixedUpdate()
    {
        HandleCloud();

    }
    void HandleCloud()
    {
        for (int i = 0; i < cloudList.Count; i++)
        {

            Transform cloud = cloudList[i];
            cloud.position += new Vector3(-1, 0, 0) * cloudSpeed * Time.deltaTime;
            if (cloud.position.x < CLOUD_DESTROY_X_POSITION)
            {
                cloudList.Remove(cloud);
                Destroy(cloud.gameObject);
                i--;
            }
        }
    }


    IEnumerator SpawnCloud()
    {
        while (true)
        {
            int randomNumber = Random.Range(1, 100);
            if (randomNumber < 20)
            {
                int randomCloud = Random.Range(1, 4);
                Transform cloud = Instantiate(GameAssets.GetInstance().GetCloudPrefab(randomCloud), new Vector3(70f, Random.Range(minYPosition, maxYPosition), 0f), Quaternion.identity);
                cloudList.Add(cloud);
            }
            yield return new WaitForSeconds(1f);

        }
    }
}
