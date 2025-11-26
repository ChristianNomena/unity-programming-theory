using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> animals;
    [SerializeField] private float startDelay = 2.0f;
    [SerializeField] private float spawnInterval = 1.5f;

    [SerializeField] private float spawnMinDistance = 30f;
    [SerializeField] private float spawnMaxDistance = 38f;

    void Start()
    {
        InvokeRepeating("SpawnRandomEnemy", startDelay, spawnInterval);
    }

    void SpawnRandomEnemy()
    {
        int index = Random.Range(0, animals.Count);

        bool chooseXAxis = Random.value < 0.5f;

        bool positiveSide = Random.value < 0.5f;

        float primaryCoord = positiveSide
            ? Random.Range(spawnMinDistance, spawnMaxDistance)
            : Random.Range(-spawnMaxDistance, -spawnMinDistance);

        float secondaryCoord = Random.Range(-spawnMaxDistance, spawnMaxDistance);

        float x, z;
        if (chooseXAxis)
        {
            x = primaryCoord;
            z = secondaryCoord;
        }
        else
        {
            z = primaryCoord;
            x = secondaryCoord;
        }

        Vector3 spawnPosition = new Vector3(x, 0f, z);
        Instantiate(animals[index], spawnPosition, animals[index].transform.rotation);
    }
}
