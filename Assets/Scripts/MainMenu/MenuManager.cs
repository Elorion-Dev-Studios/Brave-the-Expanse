using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject _titleScreenCanvas;
    [SerializeField] private GameObject _creditsScreenCanvas;

    void Start()
    {
        _titleScreenCanvas.SetActive(true);
        _creditsScreenCanvas.SetActive(false);
    }

    // to be called by New Game button OnClick
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadTitleScreen()
    {
        _titleScreenCanvas.SetActive(true);
        _creditsScreenCanvas.SetActive(false);
    }

    public void LoadCreditsScreen()
    {
        _titleScreenCanvas.SetActive(false);
        _creditsScreenCanvas.SetActive(true);
    }

}
