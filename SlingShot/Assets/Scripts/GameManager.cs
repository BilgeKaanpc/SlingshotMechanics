using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] Rope rope;

    [Header("------Texts")]
    [SerializeField] public TMP_Text counterText;
    [SerializeField] TMP_Text rateText;
    [SerializeField] TMP_Text startLevelText;
    [SerializeField] TMP_Text endLevelText;
    [SerializeField] TMP_Text endRateText;
    [SerializeField] TMP_Text inGameLevel;
    [SerializeField] TMP_Text inGameNextLevel;

    [Header("------Panels")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject gamePanel;

    [Header("------UI_Objects")]
    [SerializeField] GameObject winText;
    [SerializeField] GameObject loseText;
    [SerializeField] Button nextLevelButton;

    [Header("------Sliders")]
    [SerializeField] Slider levelSlider;

    [Header("------Int Values")]
    public int goal;
    public int activePieces;
    public int shotCounter;
    public int rate;

    void Start()
    {
        goal = GameObject.FindWithTag("region").GetComponent<collector>().ObjectList.Count;
        activePieces = goal;
        levelSlider.maxValue = goal;
        counterText.text = shotCounter + " Shot Left";
        startLevelText.text = "Level " + (SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PointUpdate()
    {
        activePieces--;
        int destroyedPieces = goal - activePieces;
        rate = (100 * destroyedPieces) / goal;
        rateText.text = "%"+ rate.ToString();
        levelSlider.value = destroyedPieces;
        
        if(rate == 100)
        {
            GameOver(true, rate);
        }
    }

    public void GameOver(bool result,int rate)
    {
        if (result)
        {
            winText.SetActive(true);
            gameOverPanel.SetActive(true);
            gamePanel.SetActive(false);
            endRateText.text = "%" + rate;
        }
        else
        {
            loseText.SetActive(true);
            gameOverPanel.SetActive(true);
            nextLevelButton.interactable = false;
            gamePanel.SetActive(false);
            endRateText.text = "%" + rate;
        }
    }

    void SetTexts()
    {
        inGameLevel.text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();
        inGameNextLevel.text = (SceneManager.GetActiveScene().buildIndex + 2).ToString();
        endLevelText.text = "Level " + (SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PlayGame()
    {
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
        SetTexts();
        StartCoroutine(rope.startDelay());

    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex == 6)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
