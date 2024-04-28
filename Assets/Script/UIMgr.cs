using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMgr : MonoBehaviour
{
    public static UIMgr Instance { get; private set; }

    public TMP_Text scoreText;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreText(GameMgr.Instance.score);
    }

    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score : " + newScore;
    }

    public void Update()
    {
        UpdateScoreText(GameMgr.Instance.score);
    }
}
