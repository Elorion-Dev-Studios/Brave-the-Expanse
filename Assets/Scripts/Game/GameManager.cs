using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //game over?
    private bool _gameOver = false;

    void Start()
    {
        _gameOver = false;
    }


    void Update()
    {
        //if game is over
        //and user presses R
        //restart game

        if (Input.GetKeyDown(KeyCode.R) && _gameOver)
        {
            SceneManager.LoadScene(1);
        }
    }


    public void UpdateGameOver()
    {
        _gameOver = true;
    }
}
