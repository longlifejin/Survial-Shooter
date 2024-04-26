using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{ 
    public static GameMgr Instance {  get; private set; }
    public bool isGameOver {  get; private set; }

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
    }
}
