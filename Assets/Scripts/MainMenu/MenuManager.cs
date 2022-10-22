using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    // to be called by New Game button OnClick
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

}
