using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public static GameMgr Instance { get; private set; }
    public bool isGameOver { get; private set; }
    public ObjectPool objectPool;
    public int score;
    public AudioSource bgmAudio;
    public GameObject pauseMenu;
    public Button quitGameButton;
    public Button resumeButton;

    public bool isPause;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
        isGameOver = false;
        isPause = false;
        pauseMenu.SetActive(false);
        quitGameButton.enabled = false;
        resumeButton.enabled = false;
    }

    private void Start()
    {
        bgmAudio.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        quitGameButton.enabled = true;
        resumeButton.enabled = true;
        isPause = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        quitGameButton.enabled = false;
        resumeButton.enabled = false;
        isPause = false;
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
        if (!isGameOver)
        {
            score += addScore;
        }
    }

}
