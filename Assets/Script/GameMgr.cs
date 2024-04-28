using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{ 
    public static GameMgr Instance {  get; private set; }
    public bool isGameOver { get; private set; }
    public ObjectPool objectPool;
    public int score;
    public AudioSource bgmAudio;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
           //Destroy(gameObject);
        }
        isGameOver = false;
    }

    private void Start()
    {
        bgmAudio.Play();
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
        Debug.Log("Game Over");
        isGameOver = true;
        StopAllCoroutines();
        StartCoroutine(GameRestart());

    }

    public IEnumerator GameRestart()
    {
        yield return new WaitForSeconds(5f);

        score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        objectPool.ResetQueue();
        isGameOver = false;
        bgmAudio.Play();

    }

    public void AddScore(int addScore)
    {
        if(!isGameOver)
        {
            score += addScore;
        }
    }

}
