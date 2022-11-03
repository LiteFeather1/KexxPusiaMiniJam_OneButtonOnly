using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TMP_Text txt_Score;
    [SerializeField] private TMP_Text txt_bestScore;
    [SerializeField] private GameObject _defeatScreen;
    [SerializeField] private Image _healthBar;
    private bool _canReplay;

    private static UiManager _instance;

    public static UiManager Instance { get => _instance; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void Update()
    {
        if(_canReplay)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                ButtonReplay();
            }
        }
    }

    public void ScoreToDisplay(int score)
    {
        txt_Score.text = $"Score : {score}";
    }

    public void DisplayDefeatScreen()
    {
        _canReplay = true;
        _defeatScreen.SetActive(true);
        txt_bestScore.text = PlayerPrefs.GetInt("BestScore").ToString();
    }

    public void ButtonReplay()
    {
        string scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene);
    }

    public void DisplayHealthBar(int currentHp)
    {
        float fCurrentHp = currentHp;
        _healthBar.fillAmount = fCurrentHp / 10;
    }
}
