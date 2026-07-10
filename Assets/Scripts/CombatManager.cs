using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public float timer = 99f;
    public int roundsJoueur = 0;
    public int roundsIA = 0;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI statusText;
    public GameObject boutonRejouer;
    public GameObject boutonQuitter;

    private bool combatActif = true;

    void Start()
    {
        if (boutonRejouer != null)
            boutonRejouer.SetActive(false);
        if (boutonQuitter != null)
            boutonQuitter.SetActive(false);
    }

    void Update()
    {
        if (!combatActif) return;

        timer -= Time.deltaTime;
        if (timerText != null)
            timerText.text = Mathf.Ceil(timer).ToString();

        if (timer <= 0)
        {
            timer = 0;
            FinDeRound("timer");
        }
    }

    public void JoueurGagne()
    {
        FinDeRound("joueur");
    }

    public void IAGagne()
    {
        FinDeRound("ia");
    }

    void FinDeRound(string gagnant)
    {
        combatActif = false;
        Time.timeScale = 0f;

        PlayerController pc = FindAnyObjectByType<PlayerController>();
        if (pc != null) pc.ArreterCombat();

        AIController ai = FindAnyObjectByType<AIController>();
        if (ai != null) ai.enabled = false;

        if (gagnant == "joueur")
            AfficherStatus("DJIBRIL EST CHAMPION !");
        else if (gagnant == "ia")
            AfficherStatus("MARIAMA EST CHAMPIONNE !");
        else
            AfficherStatus("TEMPS ECOULE !");

        if (boutonRejouer != null)
            boutonRejouer.SetActive(true);
        if (boutonQuitter != null)
            boutonQuitter.SetActive(true);
    }

    public void Rejouer()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void RetourMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    void AfficherStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
        Debug.Log(message);
    }
}
