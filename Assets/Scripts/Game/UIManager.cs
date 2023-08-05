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
    [SerializeField] private TMP_Text _restartText;
    
    //game over switch
    private bool _gameOver = false;
    [SerializeField] private string _gameOverVerbiage = "GAME OVER";
    [SerializeField] private float _gameOverFlickerSpeed = 1.0f;
    private WaitForSeconds _gameOverFlickerDelay;

    [SerializeField] private GameObject _quitMenu;

    private GameManager _gameManager;



    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Player failed to cache reference to Game Manager");
        }

        _scoreText.text = "Score: 0";
        _livesImg.sprite = _livesSprites[3];
        _gameOverText.text = _gameOverVerbiage;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
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
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlashRoutine());
    }

    public void QuitMenu()
    {
        _quitMenu.SetActive(true);
    }

    public void QuitConfirmed()
    {
        _quitMenu.SetActive(false);
        _gameManager.QuitGame();
    }

    public void QuitCancelled()
    {
        _quitMenu.SetActive(false);
        _gameManager.ResumeGame();
    }

}
