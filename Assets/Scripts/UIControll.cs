using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIControll : MonoBehaviour
{
    public Image FadeImage;
    public GameObject UIElement;
    public Text CurPointsText;

    Player m_Player;
    EnemySpawner m_EnemySpawner;

    public Text WaveText;

    Animator m_Animator;
    string[] m_WaveNum = { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten" };

    PointManager m_PointManager;

    public Image LifeBar;

    private void Awake()
    {
        UIElement.SetActive(false);
        CurPointsText.text = "Points: 0";

        m_Player = FindObjectOfType<Player>() as Player;
        m_Player.PlayerDead += OnGameOver;

        m_EnemySpawner = FindObjectOfType<EnemySpawner>() as EnemySpawner;
        m_EnemySpawner.WaveChange += OnWaveChange;

        m_Animator = GetComponent<Animator>();

        m_PointManager = FindObjectOfType<PointManager>();
        m_PointManager.PointsChange += OnPointsChanged;

        m_Player.PlayerLifeChange += OnPlayerLifeChange;

    }

    void OnGameOver()
    {
        UIElement.SetActive(true);
        StartCoroutine(Fade());
    }

    void OnWaveChange(int iWave)
    {
        WaveText.text = "- Wave " + m_WaveNum[iWave - 1] + " -";
        m_Animator.SetTrigger("WaveBeginTrigger");
    }

    void OnPlayerLifeChange(float liferatio)
    {
        LifeBar.GetComponent<RectTransform>().localScale = new Vector3(liferatio, 1, 1);
    }

    IEnumerator Fade()
    {
        float fadingtime = 1f;
        float curtime = 0;
        Color targetcolor = new Color(0.8f, 0.8f, 0.8f, 1f);

        while(curtime < fadingtime)
        {
            curtime += Time.deltaTime;
            FadeImage.color = Color.Lerp(FadeImage.color, targetcolor, 0.1f);
            yield return null;
        }
    }

    public void OnPointsChanged(int points)
    {
        CurPointsText.text = "POINTS: " + points.ToString();
    }

    public void OnRestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnExitGame()
    {
        Application.Quit();
    }

    public void OnBackToMenu()
    {
        AudioManager.Instance.PlayMusic("Menu");
        SceneManager.LoadScene("Menu");
    }
}
