using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    #region Lives_Props
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private Image _livesImg;
    #endregion

    #region Ammo_Props
    [SerializeField] private TMP_Text _ammoText;
    //0 - single laser, 1 - triple laser
    [SerializeField] private Sprite[] _ammoSprites; 
    [SerializeField] private Image _ammoImg;

    private float _noAmmoFlickerSpeed = 0.5f;
    #endregion

    #region GameOver_Props
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private TMP_Text _restartText;

    private bool _gameOver = false;
    [SerializeField] private string _gameOverVerbiage = "GAME OVER";
    [SerializeField] private float _gameOverFlickerSpeed = 1.0f;
    private WaitForSeconds _gameOverFlickerDelay; 
    #endregion

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
        _ammoText.text = "15";
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

    private IEnumerator NoAmmoFlashRoutine()
    {
        _ammoText.color = Color.red;
        yield return new WaitForSeconds(_noAmmoFlickerSpeed);
        _ammoText.color = Color.white;
    }

    public void UpdateScoreText(string scoreValue)
    {
        _scoreText.text = "Score: " + scoreValue;
    }

    public void UpdateLivesImg(int lives)
    {
        _livesImg.sprite = _livesSprites[lives];
    }

    public void UpdateGameOver()
    {
        _gameOver = true;
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlashRoutine());
    }

    public void UpdateAmmoImg(AmmoType activeAmmo)
    {
        _ammoImg.sprite = _ammoSprites[activeAmmo.GetHashCode()];
    }

    public void UpdateAmmoText(string ammoCount)
    {
        _ammoText.text = ammoCount;
    }

    public void AlertNoAmmo()
    {
        StartCoroutine(NoAmmoFlashRoutine());
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
