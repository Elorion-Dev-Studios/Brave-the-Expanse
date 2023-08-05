using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _gameOver = false;
    private bool _paused = false;
    private UIManager _uiManager;
    


    void Start()
    {
        _gameOver = false;

        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("Player failed to cache reference to UI Manager");
        }

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
            PauseGame();

            _uiManager.QuitMenu();
        }
    }


    public void UpdateGameOver()
    {
        _gameOver = true;
    }

    public void PauseGame()
    {
        _paused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        _paused = false;
        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
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
