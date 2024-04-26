using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ZombieSpawner : MonoBehaviour
{
    //public GameObject[] zombiePrefabs;
    public Transform[] spawnPoints;

    private ObjectPool objectPool;

    private int zombieCount; //나중에 UI에 추가할 것
    List<Zombie> zombies;
    private float spawnTime;
    private float lastSpawnTime;

    private void Awake()
    {
        zombies = new List<Zombie>();
    }
    private void Start()
    {
        zombies.Clear();
        spawnTime = Random.Range(0, 1f);
        lastSpawnTime = 0;
        objectPool = GameMgr.Instance.objectPool;

    }
    private void Update()
    {
        if(!GameMgr.Instance.isGameOver)
        {
            if (Time.time >= spawnTime + lastSpawnTime)
            {
                lastSpawnTime = Time.time;
                spawnTime = Random.Range(0.5f, 2f);
                CreateZombie(PickZombie());
            }
        }
    }

    private GameObject PickZombie()
    {
        float rand = Random.Range(0, 10f);
        float accumulation = 0f;
        int typeNumber = 0;

        for (int i = 0; i < objectPool.zombiePrefabs.Length; ++i)
        {
            Debug.Log(objectPool.zombiePrefabs[i]);
            float prob = objectPool.zombiePrefabs[i].GetComponent<Zombie>().probability;
            accumulation += prob;
            if (rand < accumulation)
            {
                typeNumber = i;
                Debug.Log(i);
                break;
            }
        }
        return objectPool.GetFromPool(typeNumber);
    }

    private void CreateZombie(GameObject zombiePrefab)
    {
        int index = Random.Range(0, spawnPoints.Length);
        var newEnemy = Instantiate(zombiePrefab, spawnPoints[index].position, spawnPoints[index].rotation);
        var enemy = newEnemy.GetComponent<Zombie>();
        enemy.gameObject.SetActive(true);
        enemy.onDeath += () =>
        {
            zombies.Remove(enemy);
            zombieCount = zombies.Count;

            //TO-DO : 점수 추가 부분 넣기

        };
        zombies.Add(enemy);
        zombieCount = zombies.Count;
        Debug.Log("CreateZombie");
    }


}
