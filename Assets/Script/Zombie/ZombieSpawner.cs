using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject[] zombiePrefabs;
    public Transform[] spawnPoints;

    private int zombieCount; //나중에 UI에 추가할 것
    List<Zombie> zombies;
    private float spawnTime;
    private float lastSpawnTime;

    private void Start()
    {
        zombies = new List<Zombie>();
        spawnTime = Random.Range(0, 1f);
        lastSpawnTime = 0;
    }
    private void Update()
    {
        if (Time.time >= spawnTime + lastSpawnTime)
        {
            lastSpawnTime = Time.time;
            spawnTime = Random.Range(0, 1f);
            CreateZombie(PickZombie());
        }
    }

    private GameObject PickZombie()
    {
        float rand = Random.Range(0, 10f);
        float accumulation = 0f;

        for (int i = 0; i < zombiePrefabs.Length; ++i)
        {
            float prob = zombiePrefabs[i].GetComponent<Zombie>().probability;
            accumulation += prob;
            if (rand < accumulation)
            {
                return zombiePrefabs[i];
            }
        }
        return null;
    }

    private void CreateZombie(GameObject zombiePrefab)
    {
        int index = Random.Range(0, spawnPoints.Length);
        var newEnemy = Instantiate(zombiePrefab, spawnPoints[index].position, spawnPoints[index].rotation);
        var enemy = newEnemy.GetComponent<Zombie>();

        enemy.onDeath += () =>
        {
            zombies.Remove(enemy);
            zombieCount = zombies.Count;

            //TO-DO : 점수 추가 부분 넣기

        };
        zombies.Add(enemy);
        zombieCount = zombies.Count;
    }
}
