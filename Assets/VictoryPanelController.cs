using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryPanelController : MonoBehaviour
{
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        // Предполагается, что вы имеете сцену главного меню с индексом 0
        SceneManager.LoadScene("MainMenu"); // Измените название на имя вашей сцены главного меню
    }
}
