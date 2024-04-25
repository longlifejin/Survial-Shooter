using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public ZombieData[] zombieDatas;
    public Transform[] spawnPoints;

    private int zombieCount; //���߿� UI�� �߰��� ��
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
        if(Time.time >= spawnTime + lastSpawnTime)
        {
            lastSpawnTime = Time.time;
            spawnTime = Random.Range(0, 1f);
            CreateZombie(PickZombie());
        }
    }

    private ZombieData PickZombie()
    {
        float rand = Random.Range(0, 100f);
        float accumulation = 0f;

        for(int i = 0; i <zombieDatas.Length; ++i)
        {
            accumulation += zombieDatas[i].probability;
            if (rand < accumulation)  
            {
                return zombieDatas[i];
            }
        }
        return null;
    }

    private void CreateZombie(ZombieData data)
    {
        int index = Random.Range(0, spawnPoints.Length);
        var newEnemy = Instantiate(data.GetComponent<GameObject>(), spawnPoints[index].position, spawnPoints[index].rotation);
        var enemy = newEnemy.GetComponent<Zombie>();

        enemy.onDeath += () =>
        {
            zombies.Remove(enemy);
            Destroy(enemy, 3f); //TO-DO : �ٴ����� ������� ȿ���� ����
            zombieCount = zombies.Count;

            //TO-DO : ���� �߰� �κ� �ֱ�

        };
        zombies.Add(enemy);
        zombieCount = zombies.Count;
    }
}
