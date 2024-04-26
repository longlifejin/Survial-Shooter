using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        StartCoroutine(GameRestart());
    }

    public IEnumerator GameRestart() //TO-DO : 재시작 버튼 누르면 호출하기
    {
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isGameOver = false;

    }
}
