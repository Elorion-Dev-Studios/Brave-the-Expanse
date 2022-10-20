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


    void Start()
    {
        _scoreText.text = "Score: 0";
        _livesImg.sprite = _livesSprites[3];
    }

    void Update()
    {
        
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
}
