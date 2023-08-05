using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _gameOver = false;

    void Start()
    {
        _gameOver = false;
    }


    void Update()
    {

        //restart game after game over
        if (Input.GetKeyDown(KeyCode.R) && _gameOver)
        {
            SceneManager.LoadScene(1);
        }

        //quit game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //quit play mode in editor
            if (UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
            else
            {
                //quit application
                Application.Quit();
            }
        }
    }


    public void UpdateGameOver()
    {
        _gameOver = true;
    }
}
