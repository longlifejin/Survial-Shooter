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
            //�Ͻ�����
        }
    }

    public void GameOver()
    {
        isGameOver = true;

        StartCoroutine(GameRestart());
    }

    public IEnumerator GameRestart() //TO-DO : ����� ��ư ������ ȣ���ϱ�
    {
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isGameOver = false;

    }
}
