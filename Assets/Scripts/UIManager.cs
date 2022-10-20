using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    //reference to score text
    [SerializeField] private TMP_Text _scoreText;


    void Start()
    {
        _scoreText.text = "Score: 0";
    }

    void Update()
    {
        
    }

    public void UpdateScoreText(string scoreValue)
    {
        //set score text to "Score: " + score value
        _scoreText.text = "Score: " + scoreValue;
    }
}
