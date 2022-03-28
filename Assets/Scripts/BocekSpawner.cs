using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BocekSpawner : MonoBehaviour
{
    public static BocekSpawner instance;

    [SerializeField] private float spawnDelay;

    [SerializeField] private List<GameObject> bocekler = new List<GameObject>();


    [SerializeField] private Transform[] pos1;
    [SerializeField] private Transform[] pos2;
    [SerializeField] private Transform[] pos3;
    [SerializeField] private Transform[] pos4;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InvokeRepeating("SpawnRandomBocek", spawnDelay, spawnDelay);
    }

    private Vector3 GetRandomPosition()
    {
        int rndm = Random.Range(0, 4);
        switch (rndm)
        {
            case 0:
                return RandomPosition(pos1[0].position, pos1[1].position);
            case 1:
                return RandomPosition(pos2[0].position, pos2[1].position);
            case 2:
                return RandomPosition(pos3[0].position, pos3[1].position);
            case 3:
                return RandomPosition(pos4[0].position, pos4[1].position);
        }
        return RandomPosition(pos1[0].position, pos1[1].position);

    }
    private Vector3 RandomPosition(Vector3 posA, Vector3 posB)
    {
        float x = Random.Range(posA.x, posB.x);
        float y = Random.Range(posA.y, posB.y);
        float z = Random.Range(posA.z, posB.z);

        return new Vector3(x, y, z);
    }
    private void SpawnRandomBocek()
    {
        GameObject spawnlanacakBocek = bocekler[Random.Range(0, bocekler.Count)];

        Instantiate(spawnlanacakBocek, GetRandomPosition(), Quaternion.identity);
    }
}
