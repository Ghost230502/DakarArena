using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Jouer()
    {
        SceneManager.LoadScene("Arena_Sandaga");
    }

    public void Quitter()
    {
        Debug.Log("Quitter le jeu");
        Application.Quit();
    }
}
