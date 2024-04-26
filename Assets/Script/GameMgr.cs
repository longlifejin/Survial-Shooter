using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{ 
    public static GameMgr Instance {  get; private set; }
    public bool isGameOver {  get; private set; }

    public ObjectPool objectPool;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        isGameOver = false;
    }

    private void Start()
    {
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //일시정지
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        StopAllCoroutines();
        objectPool.ClearQueue();
        StartCoroutine(GameRestart());

    }

    public IEnumerator GameRestart()
    {
        yield return new WaitForSeconds(5f);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isGameOver = false;

    }

}
