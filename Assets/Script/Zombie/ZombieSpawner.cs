using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    //public GameObject[] zombiePrefabs;
    public Transform[] spawnPoints;

    //private ObjectPool objectPool;

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
        spawnTime = Random.Range(0, 0.1f);
        lastSpawnTime = 0;
        //objectPool = GameMgr.Instance.objectPool;

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

        for (int i = 0; i < GameMgr.Instance.objectPool.zombiePrefabs.Length; ++i)
        {
            float prob = GameMgr.Instance.objectPool.zombiePrefabs[i].GetComponent<Zombie>().probability;
            accumulation += prob;
            if (rand < accumulation)
            {
                typeNumber = i;
                break;
            }
        }
        Debug.Log(typeNumber);
        if (GameMgr.Instance.objectPool.GetFromPool(typeNumber) == null)
        {
            Debug.Log("Pool is null");
        }
        return GameMgr.Instance.objectPool.GetFromPool(typeNumber);
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
            GameMgr.Instance.AddScore(enemy.score);
        };
        zombies.Add(enemy);
        zombieCount = zombies.Count;
    }
}
