using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject[] zombiePrefabs;
    private Queue<GameObject> bunnyPool = new Queue<GameObject>();
    private Queue<GameObject> bearPool = new Queue<GameObject>();
    private Queue<GameObject> ElephantPool = new Queue<GameObject>();

    private int count = 30;

    private void Awake()
    {
        ClearQueue();

    }

    private void Start()
    {
        

        AddPrefabs(zombiePrefabs[0], ElephantPool, count);
        AddPrefabs(zombiePrefabs[1], bearPool, count);
        AddPrefabs(zombiePrefabs[2], bunnyPool, count);
    }

    public void ClearQueue()
    {
        ElephantPool.Clear();
        bearPool.Clear();
        bunnyPool.Clear();
    }

    private void AddPrefabs(GameObject prefab, Queue<GameObject> queue, int count)
    {
        for(int i = 0; i < count; ++i)
        {
            var newObj = Instantiate(prefab);
            newObj.SetActive(false);
            queue.Enqueue(newObj);
        }
    }

    public GameObject GetFromPool(int type)
    {
        switch (type)
        {
            case 0:
                return GetFromQueue(ElephantPool, zombiePrefabs[type]);
            case 1:
                return GetFromQueue(bearPool, zombiePrefabs[type]);
            case 2:
                return GetFromQueue(bunnyPool, zombiePrefabs[type]);
            default:
                Debug.Log("Failed getting from queue");
                return null;
        }
    }

    public GameObject GetFromQueue(Queue<GameObject> queue, GameObject prefab)
    {
        if(queue.Count == 0)
        {
            AddPrefabs(prefab, queue, count);
        }
        return queue.Dequeue();
    }

    public void ReturnToPool(GameObject obj, int type)
    {
        obj.SetActive(false);

        switch (type)
        {
            case 0:
                ElephantPool.Enqueue(obj);
                break;
            case 1:
                bearPool.Enqueue(obj);
                break;
            case 2:
                bunnyPool.Enqueue(obj);
                break;
            default:
                break;
        }
    }

}
