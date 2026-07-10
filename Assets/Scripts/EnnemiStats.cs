using UnityEngine;
using UnityEngine.UI;

public class EnnemiStats : MonoBehaviour
{
    public float hp = 100f;
    public Slider hpSlider;
    private CombatManager combatManager;

    void Start()
    {
        MettreAJourBarreVie();
        combatManager = FindAnyObjectByType<CombatManager>();
    }

    public void RecevoirDegats(float degats)
    {
        hp -= degats;
        Debug.Log("Mariama HP : " + hp);
        MettreAJourBarreVie();

        if (hp <= 0)
        {
            Debug.Log("MARIAMA EST KO !");
            if (combatManager != null)
                combatManager.JoueurGagne();
            gameObject.SetActive(false);
        }
    }

    void MettreAJourBarreVie()
    {
        if (hpSlider == null) return;
        hpSlider.value = hp;

        Image fillImage = hpSlider.fillRect.GetComponent<Image>();
        if (fillImage != null)
        {
            if (hp > 60)
                fillImage.color = Color.green;
            else if (hp > 30)
                fillImage.color = new Color(1f, 0.6f, 0f);
            else
                fillImage.color = Color.red;
        }
    }
}