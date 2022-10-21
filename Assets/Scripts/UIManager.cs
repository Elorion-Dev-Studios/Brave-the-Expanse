using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    //reference to score text
    [SerializeField] private TMP_Text _scoreText;
    //array of sprites for lives display
    [SerializeField] private Sprite[] _livesSprites;
    //reference to Lives img
    [SerializeField] private Image _livesImg;
    //reference to GameOver txt
    [SerializeField] private TMP_Text _gameOverText;
    //game over switch
    private bool _gameOver = false;
    [SerializeField] private string _gameOverVerbiage = "GAME OVER";
    [SerializeField] private float _gameOverFlickerSpeed = 1.0f;
    private WaitForSeconds _gameOverFlickerDelay;



    void Start()
    {
        _scoreText.text = "Score: 0";
        _livesImg.sprite = _livesSprites[3];
        _gameOverText.text = _gameOverVerbiage;
        _gameOverText.gameObject.SetActive(false);
        _gameOverFlickerDelay = new WaitForSeconds(_gameOverFlickerSpeed);
    }

    private IEnumerator GameOverFlashRoutine()
    {
        while (_gameOver)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return _gameOverFlickerDelay;
            _gameOverText.gameObject.SetActive(false);
            yield return _gameOverFlickerDelay;
        }
    }

    public void UpdateScoreText(string scoreValue)
    {
        //set score text to "Score: " + score value
        _scoreText.text = "Score: " + scoreValue;
    }

    public void UpdateLivesImg(int lives)
    {
        //set livesImg sprite to the _livesSprite at index = lives
        _livesImg.sprite = _livesSprites[lives];
    }

    public void UpdateGameOver()
    {
        _gameOver = true;
        StartCoroutine(GameOverFlashRoutine());
    }
    
}
