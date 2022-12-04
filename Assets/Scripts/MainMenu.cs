using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject optionMenu;
    public GameObject mainMenu;

    private void Start()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Options()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void Back()
    {
        optionMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
